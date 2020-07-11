using CashData.Library.Internal.DataAccess;
using CashData.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashData.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo, string cashierId)
        {
            //MAKE this SOLID
            // Start filling in the models that we need to save to the database
            List<SaleDetailDBModel> details = new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate();

            foreach(var item in saleInfo.SaleDetails)
            {
                var detail = new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                //Get the information about this product
                var productInfo = products.GetProductById(item.ProductId);

                if(productInfo == null)
                {
                    throw new Exception($"The productId of {item.ProductId} could not be found int he database!");
                }

                detail.PurchasePrice = productInfo.RetailPrice * detail.Quantity;

                if(productInfo.IsTaxable)
                {
                    detail.Tax = detail.PurchasePrice * taxRate;
                }

                details.Add(detail);
            }

            //Create the sale model
            SaleDBModel sale = new SaleDBModel
            {
                SubTotal = details.Sum(x => x.PurchasePrice),
                Tax = details.Sum(x => x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;


            //Save the sale detail model

            SQLDataAccess sql = new SQLDataAccess();
            sql.SaveData("dbo.spSale_Insert", sale, "CashConnection");
            sale.Id = sql.LoadData<int, dynamic>("sp_SaleLookup", new { sale.CashierId, sale.SaleDate }, "CashConnection").FirstOrDefault();
            details.ForEach(x =>
            {
                x.SaleId = sale.Id;
                sql.SaveData("dbo.spSaleDetail_Insert", details, "CashConnection");

            });



        }
    }
}
