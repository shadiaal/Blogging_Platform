# Blogging Platform Microservices with Docker

A simple microservices-based blogging platform built with **.NET Web API**, demonstrating how to separate responsibilities between a **Post Service** and a **Comment Service**. All services are containerized with **Docker** and orchestrated using **Docker Compose**.

## Architecture

This project contains two microservices:

### 1. PostService
- **Responsibilities**: Create and retrieve blog posts.
- **Endpoints**:
  - `POST /posts`: Create a new blog post.
  - `GET /posts`: Get all blog posts.

### 2. CommentService
- **Responsibilities**: Add and retrieve comments linked to blog posts.
- **Validates `postId`** by calling PostService before saving a comment.
- **Endpoints**:
  - `POST /comments`: Add a comment to a specific post.
  - `GET /comments/{postId}`: Retrieve comments for a given post.


## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)


###  Build & Run Using Docker Compose

1. Clone the repository:

```bash
git clone https://github.com/your-username/blogging-platform.git
cd blogging-platform
```

2. Run Docker Compose:

```bash
docker-compose up --build
```

- **PostService** will run on: `http://localhost:6001`
- **CommentService** will run on: `http://localhost:6002`


##  API Usage (Test with Postman)

###  Create a Blog Post

**Request**
```http
POST http://localhost:6001/posts
Content-Type: application/json
```

**Body**
```json
{
  "title": "Getting Started with .NET",
  "content": "This is a sample blog post."
}
```

**Expected Response**
```json
{
  "id": "GUID_HERE",
  "title": "Getting Started with .NET",
  "content": "This is a sample blog post."
}
```

### Add a Comment to the Post

**Request**
```http
POST http://localhost:6002/comments
Content-Type: application/json
```

**Body**
```json
{
  "postId": "GUID_HERE",
  "author": "Jane Doe",
  "text": "Great article!"
}
```

---

### Get Comments for a Post

**Request**
```http
GET http://localhost:6002/comments/GUID_HERE
```

**Expected Response**
```json
[
  {
    "author": "Jane Doe",
    "text": "Great article!"
  }
]
```

---

##  Configuration

Environment variables are passed via Docker Compose:

```yaml
environment:
  - PostServiceUrl=http://postservice:80
```

---

## Docker Compose 

```yaml
services:
  postservice:
    build: ./PostService
    ports:
      - "6001:80"

  commentservice:
    build: ./CommentService
    ports:
      - "6002:80"
    environment:
      - PostServiceUrl=http://postservice:80
```


