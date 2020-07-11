﻿using Caliburn.Micro;
using CashDesktopUI.Library.API;
using CashDesktopUI.Library.Helpers;
using CashDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashDestopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> _products;
		private int _itemQuantity = 1;
		IConfigHelper _configHelper;
		private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
		IProductEndPoint _productEndPoint;
		ISaleEndPoint _saleEndPoint;

		public SalesViewModel(IProductEndPoint productEndPoint, IConfigHelper configHelper, ISaleEndPoint saleEndPoint)
		{
			_configHelper = configHelper;
			_productEndPoint = productEndPoint;
			_saleEndPoint = saleEndPoint;
		}
		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProducts();
		}
		public async Task LoadProducts()
		{
			var productList = await _productEndPoint.GetAll();
			Products = new BindingList<ProductModel>(productList);	
			
		}
		public BindingList<ProductModel> Products
		{
			get { return _products; }
			set 
			{ 
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}
		private ProductModel _selectedProduct;

		public ProductModel SelectedProduct
		{
			get { return _selectedProduct; }
			set 
			{
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public BindingList<CartItemModel> Cart
		{
			get { return _cart; }
			set 
			{ 
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}
		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set 
			{ 
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public string SubTotal
		{
			get 
			{
				return CalculateSubTotal().ToString("C");
			}
		}
		private decimal CalculateSubTotal()
		{
			decimal subtotal = 0;
			foreach(var item in Cart)
			{
				subtotal += item.Product.RetailPrice * item.QuantityInCart;
			}
			return subtotal;

		}
		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate() / 100;

			taxAmount = Cart
				.Where(x => x.Product.IsTaxable)
				.Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

			return taxAmount;
		}
		public string Total
		{
			get
			{
				decimal total = CalculateSubTotal() + CalculateTax();
				return total.ToString("C");
			}
		}
		public string Tax
		{
			get
			{
				return CalculateTax().ToString("C");
			}
		}
		public bool CanAddToCart
		{
			get
			{
				bool output = false;

				if(ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}

				return output;
			}
		}
		public void AddToCart()
		{
			CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
				Cart.Remove(existingItem);
				Cart.Add(existingItem);

			}
			else
			{
				CartItemModel item = new CartItemModel()
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity

				};
				Cart.Add(item);
			}
			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange (() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);

		}
		public bool CanRemoveFromCart
		{
			get
			{
				bool output = false;

				if (SelectedProduct?.QuantityInStock >= ItemQuantity)
				{
					output = true;
				}

				return output;
			}
		}
		public void RemoveFromCart()
		{
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
			NotifyOfPropertyChange(() => CanCheckOut);
		}
		public bool CanCheckOut
		{
			get
			{
				bool output = false;

				if (Cart.Count > 0)
				{
					output = true;
				}				

				return output;
			}
		}
		public async Task CheckOut()
		{
			SaleModel sale = new SaleModel();

			foreach (var item in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel
				{
					ProductId = item.Product.Id,
					Quantity = item.QuantityInCart
				});
			}
			await _saleEndPoint.PostSale(sale);
		}

	}
}
