using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.dbcontext;
using DataAccessLayer.models;
using DataAccessLayer.ViewModels;
using System.Text.Json;

namespace Frontendui.Pages.TestPage
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "https://localhost:7086/api/Test";

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IList<Test> Test { get; set; } = new List<Test>();

        public async Task OnGetAsync()
        {
            try
            {
                Console.WriteLine($"Calling API: {_apiBaseUrl}");

                var response = await _httpClient.GetAsync(_apiBaseUrl);

                Console.WriteLine($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    Console.WriteLine($"API Response: {jsonString}");

                    if (string.IsNullOrEmpty(jsonString))
                    {
                        Console.WriteLine("Response is empty");
                        Test = new List<Test>();
                        return;
                    }

                    Test = JsonSerializer.Deserialize<List<Test>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<Test>();

                    Console.WriteLine($"Deserialized {Test.Count} items");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API call failed with status: {response.StatusCode}");
                    Console.WriteLine($"Error content: {errorContent}");
                    Test = new List<Test>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP Exception: {httpEx.Message}");
                Test = new List<Test>();
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Exception: {jsonEx.Message}");
                Test = new List<Test>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                Test = new List<Test>();
            }
        }
    }
}