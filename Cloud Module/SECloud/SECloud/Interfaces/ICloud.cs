using SECloud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECloud.Interfaces
{
    public interface ICloud
    {
        Task<ServiceResponse<string>> UploadAsync(string blobName, Stream content, string contentType);
        Task<ServiceResponse<string>> UpdateAsync(string blobName, Stream content, string contentType);
        Task<ServiceResponse<Stream>> DownloadAsync(string blobName);
        Task<ServiceResponse<bool>> DeleteAsync(string blobName);
        Task<ServiceResponse<List<string>>> ListBlobsAsync();
        Task<ServiceResponse<JsonSearchResponse>> SearchJsonFilesAsync(string searchkey, string searchValue);
    }
}
