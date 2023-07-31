using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BuisnessLogic.Collector
{
    public class RequestClient
    {
        private HttpClient Client = new HttpClient();

        public async Task<string> GetAsync(string url)
        {
            using (HttpResponseMessage response = await Client.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
        }
    }
}
