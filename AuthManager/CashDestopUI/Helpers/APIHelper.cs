using CashDestopUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CashDestopUI.Helpers
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient aPIClient;

        public APIHelper()
        {
            InitializeClient();
        }
        private void InitializeClient()
        {
            aPIClient = new HttpClient();
            aPIClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["AuthApi"]);
            aPIClient.DefaultRequestHeaders.Accept.Clear();
            aPIClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            try
            {


                var data = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
                using(HttpResponseMessage response = await aPIClient.PostAsync("/token", data))
                {
                    if(response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsAsync<AuthenticatedUser>();
                        return result;
                    }
                    else
                    {
                        throw new Exception(response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
