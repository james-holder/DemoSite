using CashData.Library.Internal.DataAccess;
using CashData.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashData.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SQLDataAccess sql = new SQLDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetAll", new { }, "CashConnection");

            return output;
        }
        public ProductModel GetProductById(int productId)
        {
            SQLDataAccess sql = new SQLDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>("dbo.spProduct_GetById", new { Id = productId }, "CashConnection").FirstOrDefault();

            return output;
        }
    }
}
