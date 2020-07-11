using CashDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CashDesktopUI.Library.API
{
    public class SaleEndPoint : ISaleEndPoint
    {
        private IAPIHelper _apiHelper;
        public SaleEndPoint(IAPIHelper aPIHelper)
        {
            _apiHelper = aPIHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using(HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("api/Sale", sale))
            {
                if(response.IsSuccessStatusCode)
                {
                    //Log successful call
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

        }
    }
}
