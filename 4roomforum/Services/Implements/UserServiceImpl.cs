using _4roomforum.DTOs;
using _4roomforum.Services.Interfaces;

namespace _4roomforum.Services.Implements
{
    public class UserServiceImpl : IUserService
    {
        private readonly ILogger<UserServiceImpl> _logger;
        private readonly HttpClient _client;

        public UserServiceImpl(
            HttpClient httpClient,
            ILogger<UserServiceImpl> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _client = httpClient;
            _client.BaseAddress = new Uri(
                configuration["ServiceUrls:UserService"]
                ?? throw new InvalidOperationException(
                    "ServiceUrls:UserService chưa được cấu hình."));
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers() {

            try
            {
                var response = await _client.GetAsync("api/user");
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<IEnumerable<UserDTO>>();
                    return users ?? new List<UserDTO>();
                }
                else
                {
                    _logger.LogError($"Failed to retrieve users. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetAllUsers: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetAllUsers: {ex.Message}");
                return null;
            }
        }
        public async Task<UserDTO> Login(string email, string password)
        {
            try
            {
                var loginData = new { Email = email, Password = password };
                var response = await _client.PostAsJsonAsync("api/user/login", loginData);
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return user;
                }
                else
                {
                    _logger.LogError($"Login failed. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in Login: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in Login: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDTO> GetUserProfile(int userId)
        {
            try
            {
                var response = await _client.GetAsync($"api/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var userProfile = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return userProfile;
                }
                else
                {
                    _logger.LogError($"Failed to get user profile. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetUserProfile: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetUserProfile: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDTO?> RegisterUserAsync(UserDTO userDTO)
        {
            try
            {
                var response = await _client.PostAsJsonAsync("api/user/register", userDTO);

                if (response.IsSuccessStatusCode)
                {
                    var newUser = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return newUser;
                }
                else
                {
                    _logger.LogError($"Register failed. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in RegisterUserAsync: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in RegisterUserAsync: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> UpdateUser(int userId, UserDTO userUpdateDto)
        {
            try
            {
                var response = await _client.PutAsJsonAsync($"api/user/{userId}", userUpdateDto);
                if (response.IsSuccessStatusCode)
                {
                    return true; // Cập nhật thành công
                }
                else
                {
                    _logger.LogError($"Failed to update user. Status Code: {response.StatusCode}");
                    return false; // Cập nhật thất bại
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in UpdateUser: {ex.Message}");
                return false; // Có lỗi trong quá trình gửi yêu cầu
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in UpdateUser: {ex.Message}");
                return false; // Lỗi không mong đợi
            }
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            try
            {
                var response = await _client.GetAsync($"api/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return user;
                }
                else
                {
                    _logger.LogError($"Failed to retrieve user. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetUserById: {ex.Message}");
                return null;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetUserById: {ex.Message}");
                return null;
            }
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            try
            {
                var response = await _client.GetAsync($"api/user/email/{email}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return user;
                }
                else
                {
                    _logger.LogError($"Failed to retrieve user. Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Request error in GetUserByEmail: {ex.Message}");
                return null;
            }

            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error in GetUserByEmail: {ex.Message}");
                return null;
            }
        }
    }
}