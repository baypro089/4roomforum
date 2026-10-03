using _4roomforum.DTOs;
using _4roomforum.Models;
using _4roomforum.Services.Interfaces;
using System.Threading;

namespace _4roomforum.Services.Implements
{
    public class ThreadServiceImpl : IThreadService
    {
        private readonly ILogger<ThreadServiceImpl> _logger;
        private readonly HttpClient _client;

        public ThreadServiceImpl(
            HttpClient httpClient,
            ILogger<ThreadServiceImpl> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _client = httpClient;
            _client.BaseAddress = new Uri(
                configuration["ServiceUrls:CatThreadService"]
                ?? throw new InvalidOperationException(
                    "ServiceUrls:CatThreadService chưa được cấu hình."));
        }

        public async Task<IEnumerable<ThreadDTO>> GetAllThreads()
        {
            try
            {
                var response = await _client.GetAsync("api/thread");
                if (response.IsSuccessStatusCode)
                {
                    var threads = await response.Content.ReadFromJsonAsync<IEnumerable<ThreadDTO>>();
                    return threads ?? new List<ThreadDTO>();
                }
                else
                {
                    _logger.LogError($"Failed to get threads. Status Code: {response.StatusCode}");
                    return new List<ThreadDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetAllThreads: {ex.Message}");
                return new List<ThreadDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetAllThreads : {ex.Message}");
                return new List<ThreadDTO>();
            }
        }

        public async Task<IEnumerable<ThreadDTO>> GetHotThreads()
        {
            try
            {
                var response = await _client.GetAsync("api/thread/hotThread");
                if (response.IsSuccessStatusCode)
                {
                    var hotthreads = await response.Content.ReadFromJsonAsync<IEnumerable<ThreadDTO>>();
                    return hotthreads ?? new List<ThreadDTO>();
                }
                else
                {
                    _logger.LogError($"Failed to get hot threads. Status Code: {response.StatusCode}");
                    return new List<ThreadDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetHotCategories: {ex.Message}");
                return new List<ThreadDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetHotCategories: {ex.Message}");
                return new List<ThreadDTO>();
            }
        }

        public async Task<ThreadDTO> GetThreadById(int threadID)
        {
            try
            {
                var url = $"api/thread/{threadID}";
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var thread = await response.Content.ReadFromJsonAsync<ThreadDTO>();
                    if (thread == null)
                    {
                        _logger.LogWarning($"Thread with ID {threadID} returned null.");
                    }
                    return thread;
                }
                else
                {
                    _logger.LogError($"Failed to get thread with ID {threadID}. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetThreadById for thread ID {threadID}: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetThreadById for thread ID {threadID}: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<ThreadDTO>> GetThreadsByCategoryId(int categoryId)
        {
            try
            {
                var url = $"api/thread/getThreadByCategory/{categoryId}";
                var response = await _client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var threads = await response.Content.ReadFromJsonAsync<IEnumerable<ThreadDTO>>();
                    return threads ?? new List<ThreadDTO>();
                }
                else
                {
                    _logger.LogError($"Failed to get threads with category ID {categoryId}. Status Code: {response.StatusCode}");
                    return new List<ThreadDTO>();
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetThreadsByCategoryId for category ID {categoryId}: {ex.Message}");
                return new List<ThreadDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetThreadsByCategoryId for category ID {categoryId}: {ex.Message}");
                return new List<ThreadDTO>();
            }
        }

        //dùng cho admin
        public Threads getThreadById(int threadID)
        {
            try
            {
                var url = $"api/thread/{threadID}";
                var response = _client.GetAsync(url).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var thread = response.Content.ReadFromJsonAsync<Threads>().GetAwaiter().GetResult();
                    if (thread == null)
                    {
                        _logger.LogWarning($"Thread with ID {threadID} returned null.");
                    }
                    return thread;
                }
                else
                {
                    _logger.LogError($"Failed to get thread with ID {threadID}. Status Code: {response.StatusCode}");
                    return null;
                }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError($"Request error in GetThreadById for thread ID {threadID}: {ex.Message}");
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Unexpected error in GetThreadById for thread ID {threadID}: {ex.Message}");
                    return null;
                }
        }

        public async Task<bool> EditThread(Threads thread)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"api/thread/{thread.ThreadId}", thread);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in EditThread: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in EditThread: {ex.Message}");
                return false;
            }
        }

    }
}
