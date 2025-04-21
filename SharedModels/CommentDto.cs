using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
	public class CommentDto
	{
		public Guid PostId { get; set; }
		public string Author { get; set; }
		public string Text { get; set; }
	}
}
