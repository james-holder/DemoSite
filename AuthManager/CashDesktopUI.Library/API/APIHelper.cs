using CashDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CashDesktopUI.Library.API
{
    public class APIHelper : IAPIHelper
    {
        private HttpClient aPIClient;
        private ILoggedInUserModel _loggedInUser;
        public APIHelper(ILoggedInUserModel loggedInUser)
        {
            InitializeClient();
            _loggedInUser = loggedInUser;
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
        public async Task GetLoggedInUserInfo(string token)
        {
            aPIClient.DefaultRequestHeaders.Clear();
            aPIClient.DefaultRequestHeaders.Accept.Clear();
            aPIClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            aPIClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using(HttpResponseMessage response = await aPIClient.GetAsync("api/User"))
            {
                if(response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();
                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.EmailAddress = result.EmailAddress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Token = token;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
