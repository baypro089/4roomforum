using _4roomforum.Services.Interfaces;
using _4roomforum.DTOs;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text;
namespace _4roomforum.Services.Implements
{
    public class PostServiceImpl : IPostService
    {
        private readonly ILogger<PostServiceImpl> _logger;
        private readonly HttpClient _client;

        public PostServiceImpl(
            HttpClient httpClient,
            ILogger<PostServiceImpl> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _client = httpClient;
            _client.BaseAddress = new Uri(
                configuration["ServiceUrls:PostService"]
                ?? throw new InvalidOperationException(
                    "ServiceUrls:PostService chưa được cấu hình."));
        }
        public async Task<LikeResult> LikePost(int postId, int userId)
        {
            try
            {
                var response = await _client.PutAsync($"api/post/like/{postId}/{userId}", null);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<JsonElement>(content);

                    var message = responseObject.GetProperty("message").GetString();
                    var likeCount = responseObject.GetProperty("likeCount").GetInt32();

                    if (message.StartsWith("Liked post"))
                    {
                        return new LikeResult(
                            true,
                            true,
                            likeCount,
                            "Successfully liked the post."
                        );
                    }
                    else if (message.StartsWith("Unliked post"))
                    {
                        return new LikeResult(
                            true,
                            false,
                            likeCount,
                            "Successfully unliked the post."
                        );
                    }
                }
                return new LikeResult(
                    false,
                    null,
                    0,
                    "Failed to process the request."
                );
            }
            catch (Exception ex)
            {
                return new LikeResult(
                    false,
                    null,
                    0,
                    ex.Message
                );
            }
        }
        public async Task<PagedResult<PostDTO>> GetPostsByThreadId(int id, int userId, int page)
        {
            try
            {
                var response = await _client.GetAsync($"api/post/with_thread/{id}/{userId}/{page}");

                if (response.IsSuccessStatusCode)
                {
                    var posts = await response.Content.ReadFromJsonAsync<PagedResult<PostDTO>>();
                    return posts ?? new PagedResult<PostDTO>();
                }
                else
                {
                    _logger.LogError($"Failed to get posts by thread id. Status Code: {response.StatusCode}");
                    return new PagedResult<PostDTO>();

                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetPostsByThreadId: {ex.Message}");
                return new PagedResult<PostDTO>();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetPostsByThreadId: {ex.Message}");
                return new PagedResult<PostDTO>();
            }
        }

        public async Task<PostDTO> GetPostById(int id)
        {
            try
            {
                var response = await _client.GetAsync($"api/post/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var post = await response.Content.ReadFromJsonAsync<PostDTO>();
                    return post;
                }
                else
                {
                    _logger.LogError($"Failed to get post. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in getapost: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in getapost: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> CreatePostAsync(CreatePostDTO postDTO)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/post", postDTO);

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Success in Create: {message}");
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Bad Request in Create: {error}");
                }
                else
                {
                    _logger.LogError($"Failed to create post. Status Code: {response.StatusCode}");
                }

                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in CreatePostAsync: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in CreatePostAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<PostDTO>> GetAllPostsAsync()
        {
            try
            {
                // Gọi API "api/post"
                var response = await _client.GetAsync("api/post");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize dữ liệu từ response
                    var posts = await response.Content.ReadFromJsonAsync<IEnumerable<PostDTO>>();
                    return posts ?? Enumerable.Empty<PostDTO>();
                }
                else
                {
                    _logger.LogError($"Failed to get all posts. Status Code: {response.StatusCode}");
                    return Enumerable.Empty<PostDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetAllPosts: {ex.Message}");
                return Enumerable.Empty<PostDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetAllPosts: {ex.Message}");
                return Enumerable.Empty<PostDTO>();
            }
        }



        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"api/post/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Success in Delete: {message}");
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Bad Request in Delete: {error}");
                }
                else
                {
                    _logger.LogError($"Failed to delete post. Status Code: {response.StatusCode}");
                }

                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in DeletePostAsync: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in DeletePostAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePostAsync(int id, UpdatePostDTO postDTO)
        {
            try
            {
                //var jsonContent = new StringContent(
                //    JsonSerializer.Serialize(postDTO),
                //    Encoding.UTF8,
                //    "application/json"
                //);

                // Gửi yêu cầu PUT đến API với id và nội dung cần cập nhật
                var response = await _client.PutAsJsonAsync($"api/post/{id}", postDTO);

                if (response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Success in Update: {message}");
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning($"Bad Request in Update: {error}");
                }
                else
                {
                    _logger.LogError($"Failed to update post. Status Code: {response.StatusCode}");
                }

                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in UpdatePostAsync: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in UpdatePostAsync: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> CheckLikeStatus(int postId, int userId)
        {
            try
            {
                var response = await _client.GetAsync($"api/post/checkLike/{postId}/{userId}");

                if (response.IsSuccessStatusCode)
                {

                    var content = await response.Content.ReadAsStringAsync();
                    return bool.Parse(content); 
                }
                else
                {
                    _logger.LogError($"Failed to check like status. Status Code: {response.StatusCode}");
                    return false; 
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in CheckLikeStatus: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in CheckLikeStatus: {ex.Message}");
                return false;
            }
        }


    

    }
}