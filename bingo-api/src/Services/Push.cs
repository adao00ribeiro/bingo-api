using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bingo_api.src.Services;

public class Push
{
    public async Task CriarPix()
    {
        var client = new HttpClient();

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.pushinpay.com.br/api/pix/cashIn");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "34432|1vsemHuaQShQjzdy4bWKMTLLeCFvnVAJMu5PLGCd8d9040bf");
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var json = @"
        {
            ""value"": 10,
            ""webhook_url"": ""http://homologation-bingo-api.srv813210.hstgr.cloud/api/webhook"",
            ""split_rules"": []
        }";

        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);

        response.EnsureSuccessStatusCode();

        string result = await response.Content.ReadAsStringAsync();
        Console.WriteLine(result);

    }
    public async Task ConsultarPix()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.pushinpay.com.br/api/transactions/{ID}");
        request.Headers.Add("Authorization", "Bearer");
        request.Headers.Add("Accept", "application/json");
        var content = new StringContent(string.Empty);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());
    }

}