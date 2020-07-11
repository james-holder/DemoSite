using CashData.Library.DataAccess;
using CashData.Library.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AuthManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [HttpPost]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userId);
        }

    }
}
