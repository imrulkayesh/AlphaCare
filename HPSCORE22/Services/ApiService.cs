using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AlphaCare.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // =========================
        // GET BUSINESS UNITS FROM FIRST API
        // =========================
        public async Task<ApiResponse<BusinessUnit>> GetBusinessUnitsAsync()
        {
            try
            {
                var requestBody = new { dB_CONN_ID = "ttaposdb" };
                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Create request message to set headers
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://sanko.prangroup.com/CoreAPIxyz/api/Inventory/getdb"),
                    Content = content
                };

                // Add API Key header
                request.Headers.Add("API-KEY", "OFFxyz456Inv");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<BusinessUnit>>(jsonResponse);
                    return apiResponse;
                }

                return new ApiResponse<BusinessUnit>
                {
                    status = false,
                    message = $"API call failed with status: {response.StatusCode}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<BusinessUnit>
                {
                    status = false,
                    message = ex.Message
                };
            }
        }

        // =========================
        // LOGIN USING SECOND API
        // =========================
        public async Task<LoginApiResponse<LoginResult>> LoginAsync(
            string businessUnitId,
            string schemaId,
            string username,
            string password)
        {
            try
            {
                var apiUrl = "http://sanko.prangroup.com/CoreAPIxyz/api/inventory/getLoginInfo";

                var requestBody = new
                {
                    dB_CONN_ID = businessUnitId,
                    SCHEMA_ID = schemaId,
                    iN_PARAMS = new
                    {
                        userId = username,
                        password = password
                    }
                };

                var jsonContent = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Create request message to set headers
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(apiUrl),
                    Content = content
                };

                // Add API Key header
                request.Headers.Add("API-KEY", "OFFxyz456Inv");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<LoginApiResponse<LoginResult>>(jsonResponse);
                    return apiResponse;
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                return new LoginApiResponse<LoginResult>
                {
                    status = false,
                    message = $"API call failed with status: {response.StatusCode} - {errorContent}"
                };
            }
            catch (HttpRequestException ex)
            {
                return new LoginApiResponse<LoginResult>
                {
                    status = false,
                    message = $"Network error: {ex.Message}"
                };
            }
            catch (JsonException ex)
            {
                return new LoginApiResponse<LoginResult>
                {
                    status = false,
                    message = $"JSON parsing error: {ex.Message}"
                };
            }
            catch (Exception ex)
            {
                return new LoginApiResponse<LoginResult>
                {
                    status = false,
                    message = $"Unexpected error: {ex.Message}"
                };
            }
        }

        // =========================
        // GET BUSINESS UNIT BY ID (Helper Method)
        // =========================
        public async Task<BusinessUnit> GetBusinessUnitByIdAsync(string businessUnitId)
        {
            try
            {
                var apiResponse = await GetBusinessUnitsAsync();
                if (apiResponse != null && apiResponse.status && apiResponse.result != null)
                {
                    return apiResponse.result.FirstOrDefault(b => b.ID == businessUnitId);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        // =========================
        // GET BUSINESS UNITS AS SELECT LIST ITEMS (Helper Method)
        // =========================
        public async Task<List<SelectListItem>> GetBusinessUnitsAsSelectListAsync()
        {
            try
            {
                var apiResponse = await GetBusinessUnitsAsync();
                var businessUnits = new List<SelectListItem>();

                if (apiResponse != null && apiResponse.status && apiResponse.result != null)
                {
                    businessUnits = apiResponse.result.Select(bu => new SelectListItem
                    {
                        Value = bu.ID,
                        Text = bu.NAME
                    }).ToList();

                    businessUnits.Insert(0, new SelectListItem
                    {
                        Value = "",
                        Text = "Select Business Unit"
                    });
                }
                else
                {
                    // Fallback data if API fails
                    businessUnits = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "", Text = "Select Business Unit" },
                        new SelectListItem { Value = "TT", Text = "Tasty Treat" },
                        new SelectListItem { Value = "BB", Text = "Best Buy" },
                        new SelectListItem { Value = "DS", Text = "Daily Shopping" }
                    };
                }

                return businessUnits;
            }
            catch
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Select Business Unit" },
                    new SelectListItem { Value = "TT", Text = "Tasty Treat" }
                };
            }
        }
    }

    // =========================
    // RESPONSE MODELS
    // =========================

    // Response model for first API (Get Business Units)
    public class ApiResponse<T>
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int rowsAffected { get; set; }
        public int executionTime { get; set; }
        public List<T> result { get; set; }
    }

    // Business Unit model
    public class BusinessUnit
    {
        public string NAME { get; set; }
        public string ID { get; set; }
        public string CONN_ID { get; set; }
        public string SCHEMA_ID { get; set; }
    }

    // Response model for login API
    public class LoginApiResponse<T>
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int rowsAffected { get; set; }
        public int executionTime { get; set; }
        public List<T> result { get; set; }
    }

    // Login result model
    public class LoginResult
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string CODE { get; set; }
        public string NAMES { get; set; }
        public string ADDRESS { get; set; }
        public string CONTACT { get; set; }
    }

    // Select List Item model (if not using Microsoft.AspNetCore.Mvc.Rendering)
    public class SelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
    }
}