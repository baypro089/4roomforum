
using System.Security.Cryptography.X509Certificates;
using _4roomforum.DTOs;
using _4roomforum.Models;
using _4roomforum.Services.Interfaces;
namespace _4roomforum.Services.Implements
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly ILogger<CategoryServiceImpl> _logger;
        private readonly HttpClient _client;
        public CategoryServiceImpl(
            HttpClient httpClient,
            ILogger<CategoryServiceImpl> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _client = httpClient;
            _client.BaseAddress = new Uri(
                configuration["ServiceUrls:CatThreadService"]
                ?? throw new InvalidOperationException(
                    "ServiceUrls:CatThreadService chưa được cấu hình."));
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllCategory()
        {
            try
            {
                var response = await _client.GetAsync("api/category");
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryDTO>>();
                    return categories ?? new List<CategoryDTO>();
                }
                else
                {
                    _logger.LogError($"Failed to get categories. Status Code: {response.StatusCode}");
                    return new List<CategoryDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetAllCategory: {ex.Message}");
                return new List<CategoryDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetAllCategory: {ex.Message}");
                return new List<CategoryDTO>();
            }
        }
        public async Task<IEnumerable<CategoryViewDTO>> GetHotCategories()
        {
            try
            {
                var response = await _client.GetAsync("api/category/hot-categories");
                if (response.IsSuccessStatusCode)
                {
                    var categories = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryViewDTO>>();
                    return categories ?? new List<CategoryViewDTO>();
                }
                else
                {
                    _logger.LogError($"Failed to get hot categories. Status Code: {response.StatusCode}");
                    return new List<CategoryViewDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetHotCategories: {ex.Message}");
                return new List<CategoryViewDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetHotCategories: {ex.Message}");
                return new List<CategoryViewDTO>();
            }
        }

        public async Task<bool> AddCategory(Category category)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/category", category);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in AddCategory: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in AddCategory: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EditCategory(Category category)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"api/category/{category.CategoryId}", category);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in EditCategory: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in EditCategory: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveCategory(int id)
        {
            try
            {
                var response = await _client.DeleteAsync($"api/category/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in RemoveCategory: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in RemoveCategory: {ex.Message}");
                return false;
            }
        }

        public Category getCategoryById(int id)
        {
            try
            {
                var response = _client.GetAsync($"api/category/{id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var category = response.Content.ReadFromJsonAsync<Category>().Result;
                    return category;
                }
                else
                {
                    _logger.LogError($"Failed to get category. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in getCategoryById: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in getCategoryById: {ex.Message}");
                return null;
            }
        }
        public List<Category> getAllCategory()
        {
            try
            {
                var response = _client.GetAsync("api/category").Result;
                if (response.IsSuccessStatusCode)
                {
                    var categories = response.Content.ReadFromJsonAsync<IEnumerable<Category>>().Result;
                    return categories.ToList();
                }
                else
                {
                    _logger.LogError($"Failed to get categories. Status Code: {response.StatusCode}");
                    return new List<Category>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in getAllCategory: {ex.Message}");
                return new List<Category>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in getAllCategory: {ex.Message}");
                return new List<Category>();
            }
        }
        
    }
}