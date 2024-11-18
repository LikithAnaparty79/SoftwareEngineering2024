// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CloudUnitTesting;
internal class RestClient
{
    private readonly HttpClient _httpClient;
    private readonly string _url;

    public RestClient(string url)
    {
        _httpClient = new HttpClient();
        _url = url;
    }

    public async Task<> DownloadAsync(string team, string dataUri)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(_url + $"/{team}/{dataUri}");
        response.EnsureSuccessStatusCode();
        string result = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };

        var entity = JsonSerializer.Deserialize<>(result, options);
        return entity;

    }

    public async Task<> DownloadAsyncTest()
    {

    }

    public async Task<> DeleteAsyncTest()
    {

    }

    public async Task<> UpdateAsyncTest()
    {

    }

    public async Task<> ListBlobsAsyncTest()
    {

    }

    public async Task<> SearchJsonAsyncTest()
    {

    }

    public async Task<> ConfigRetrieveAsyncTest()
    {

    }
}
*/
