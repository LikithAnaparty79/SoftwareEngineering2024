using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SECloud.Interfaces;
using SECloud.Models;

namespace SECloud.Services
{
    public class CloudService : ICloud
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _team;
        private readonly string _sasToken;
        private readonly ILogger<CloudService> _logger;

        public CloudService(
            string baseUrl,
            string team,
            string sasToken,
            HttpClient httpClient,
            ILogger<CloudService> logger)
        {
            _httpClient = httpClient;
            _baseUrl = baseUrl;
            _team = team;
            _sasToken = sasToken;
            _logger = logger;

            _logger.LogInformation("CloudService initialized for team: {Team}", team);
        }

        private string GetRequestUrl(string endpoint)
        {
            var url = $"{_baseUrl}/{endpoint}?{_sasToken}";
            _logger.LogDebug("Generated request URL for endpoint: {Endpoint}", endpoint);
            return url;
        }

        public async Task<ServiceResponse<string>> UploadAsync(string blobName, Stream content, string contentType)
        {
            _logger.LogInformation("Starting upload for blob: {BlobName}, ContentType: {ContentType}", blobName, contentType);

            try
            {
                var requestUrl = GetRequestUrl($"upload/{_team}");

                // Create multipart form data content
                using var multipartContent = new MultipartFormDataContent();
                using var streamContent = new StreamContent(content);

                // Set the content type and disposition headers to match the Azure Function's expectations
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                // Add the file content to the multipart form data
                // The name "file" is arbitrary but must be present for proper multipart formatting
                multipartContent.Add(streamContent, "file", blobName);

                _logger.LogDebug("Sending upload request for blob: {BlobName}", blobName);
                var response = await _httpClient.PostAsync(requestUrl, multipartContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Successfully uploaded blob: {BlobName}", blobName);
                    return new ServiceResponse<string>
                    {
                        Success = true,
                        Data = responseContent,
                        Message = $"Successfully uploaded {blobName}"
                    };
                }

                var errorMessage = $"Upload failed: {response.StatusCode} - {response.ReasonPhrase}";
                _logger.LogError("Upload failed for blob: {BlobName}. Status: {StatusCode}, Reason: {Reason}",
                    blobName, response.StatusCode, response.ReasonPhrase);

                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while uploading blob: {BlobName}", blobName);
                throw;
            }
        }

        public async Task<ServiceResponse<Stream>> DownloadAsync(string blobName)
        {
            _logger.LogInformation("Starting download for blob: {BlobName}", blobName);

            try
            {
                var requestUrl = GetRequestUrl($"download/{_team}/{blobName}");
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    _logger.LogInformation("Successfully downloaded blob: {BlobName}", blobName);
                    return new ServiceResponse<Stream> { Success = true, Data = stream };
                }

                _logger.LogError("Download failed for blob: {BlobName}. Status: {StatusCode}, Reason: {Reason}",
                    blobName, response.StatusCode, response.ReasonPhrase);
                return new ServiceResponse<Stream> { Success = false, Message = $"Download failed: {response.ReasonPhrase}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while downloading blob: {BlobName}", blobName);
                throw;
            }
        }

        public async Task<ServiceResponse<bool>> DeleteAsync(string blobName)
        {
            _logger.LogInformation("Starting delete operation for blob: {BlobName}", blobName);

            try
            {
                var requestUrl = GetRequestUrl($"delete/{_team}/{blobName}");
                var response = await _httpClient.DeleteAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully deleted blob: {BlobName}", blobName);
                }
                else
                {
                    _logger.LogError("Delete failed for blob: {BlobName}. Status: {StatusCode}, Reason: {Reason}",
                        blobName, response.StatusCode, response.ReasonPhrase);
                }

                return new ServiceResponse<bool>
                {
                    Success = response.IsSuccessStatusCode,
                    Data = response.IsSuccessStatusCode,
                    Message = response.IsSuccessStatusCode ? null : $"Delete failed: {response.ReasonPhrase}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting blob: {BlobName}", blobName);
                throw;
            }
        }

        public async Task<ServiceResponse<List<string>>> ListBlobsAsync()
        {
            _logger.LogInformation("Starting list operation for blobs");

            try
            {
                // Adjust request URL to call Azure Function without prefix
                var requestUrl = GetRequestUrl($"list/{_team}");
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize response into BlobListResponse and get the list of blob names
                    var blobListResponse = await response.Content.ReadFromJsonAsync<BlobListResponse>();
                    var blobs = blobListResponse?.Blobs;

                    _logger.LogInformation("Successfully listed blobs. Found {Count} blobs", blobs?.Count ?? 0);
                    return new ServiceResponse<List<string>> { Success = true, Data = blobs };
                }

                _logger.LogError("List operation failed. Status: {StatusCode}, Reason: {Reason}",
                    response.StatusCode, response.ReasonPhrase);
                return new ServiceResponse<List<string>> { Success = false, Message = $"List failed: {response.ReasonPhrase}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while listing blobs");
                throw;
            }
        }



        public async Task<ServiceResponse<string>> RetrieveConfigAsync(string setting)
        {
            _logger.LogInformation("Starting config retrieval for setting: {Setting}", setting);

            try
            {
                var requestUrl = GetRequestUrl($"config/{_team}/{setting}");
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var config = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Successfully retrieved config for setting: {Setting}", setting);
                    return new ServiceResponse<string> { Success = true, Data = config };
                }

                _logger.LogError("Config retrieval failed for setting: {Setting}. Status: {StatusCode}, Reason: {Reason}",
                    setting, response.StatusCode, response.ReasonPhrase);
                return new ServiceResponse<string> { Success = false, Message = $"Config retrieval failed: {response.ReasonPhrase}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while retrieving config for setting: {Setting}", setting);
                throw;
            }
        }

        public async Task<ServiceResponse<string>> UpdateAsync(string blobName, Stream content, string contentType)
        {
            _logger.LogInformation("Starting update operation for blob: {BlobName}, ContentType: {ContentType}", blobName, contentType);

            try
            {
                var requestUrl = GetRequestUrl($"put/{_team}/{blobName}");

                // Create HTTP content from the stream
                using var streamContent = new StreamContent(content);

                // Set the content type header to match Azure Function's expectations
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                // Send PUT request
                var response = await _httpClient.PutAsync(requestUrl, streamContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Successfully updated blob: {BlobName}", blobName);
                    return new ServiceResponse<string>
                    {
                        Success = true,
                        Data = responseContent,
                        Message = $"Successfully updated {blobName}"
                    };
                }

                var errorMessage = $"Update failed: {response.StatusCode} - {response.ReasonPhrase}";
                _logger.LogError("Update failed for blob: {BlobName}. Status: {StatusCode}, Reason: {Reason}",
                    blobName, response.StatusCode, response.ReasonPhrase);

                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while updating blob: {BlobName}", blobName);
                throw;
            }
        }

        public async Task<ServiceResponse<JsonSearchResponse>> SearchJsonFilesAsync(string searchKey, string searchValue)
        {
            _logger.LogInformation("Starting JSON search with key: {SearchKey}, value: {SearchValue}", searchKey, searchValue);

            try
            {
                if (string.IsNullOrEmpty(searchKey) || string.IsNullOrEmpty(searchValue))
                {
                    return new ServiceResponse<JsonSearchResponse>
                    {
                        Success = false,
                        Message = "Both searchKey and searchValue are required"
                    };
                }

                var requestUrl = GetRequestUrl($"search/{_team}") + $"&key={Uri.EscapeDataString(searchKey)}&value={Uri.EscapeDataString(searchValue)}";
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    var searchResponse = await response.Content.ReadFromJsonAsync<JsonSearchResponse>();
                    _logger.LogInformation("Successfully completed JSON search. Found {MatchCount} matches", searchResponse?.MatchCount ?? 0);

                    return new ServiceResponse<JsonSearchResponse>
                    {
                        Success = true,
                        Data = searchResponse,
                        Message = $"Search completed successfully. Found {searchResponse?.MatchCount ?? 0} matches."
                    };
                }

                _logger.LogError("JSON search failed. Status: {StatusCode}, Reason: {Reason}",
                    response.StatusCode, response.ReasonPhrase);

                return new ServiceResponse<JsonSearchResponse>
                {
                    Success = false,
                    Message = $"Search failed: {response.ReasonPhrase}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during JSON search");
                throw;
            }
        }
    }
}