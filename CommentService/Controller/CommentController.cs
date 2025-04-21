
﻿// Controllers/CommentsController.cs
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using System.Text.Json;

namespace CommentService.Controller;

[ApiController]
[Route("comments")]
public class CommentsController : ControllerBase
{
	private static readonly Dictionary<Guid, List<CommentDto>> CommentsByPost = new();
	private readonly HttpClient _httpClient;
	private readonly IConfiguration _config;

	public CommentsController(IHttpClientFactory httpClientFactory, IConfiguration config)
	{
		_httpClient = httpClientFactory.CreateClient();
		_config = config;
	}

	[HttpPost]
	public async Task<IActionResult> AddComment(CommentDto comment)
	{
		var postServiceUrl = _config["PostServiceUrl"];
		var response = await _httpClient.GetAsync($"{postServiceUrl}/posts");

		if (!response.IsSuccessStatusCode)
			return StatusCode(500, "Failed to contact PostService");

		var posts = JsonSerializer.Deserialize<List<PostDto>>(
			await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

		if (posts?.All(p => p.Id != comment.PostId) == true)
			return BadRequest("Invalid postId");

		if (!CommentsByPost.ContainsKey(comment.PostId))
			CommentsByPost[comment.PostId] = new List<CommentDto>();

		CommentsByPost[comment.PostId].Add(comment);
		return Ok(comment);
	}

	[HttpGet("{postId}")]
	public IActionResult GetComments(Guid postId)
	{
		CommentsByPost.TryGetValue(postId, out var comments);
		return Ok(comments ?? new List<CommentDto>());
	}
}