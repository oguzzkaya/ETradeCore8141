﻿using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
using DataAccess.Repositories;
using System.Globalization;

namespace Business.Services
{
    public interface IProductService : IService<ProductModel>
    {

    }

    public class ProductService : IProductService
    {
        private readonly ProductRepoBase _productRepo;

        public ProductService(ProductRepoBase productRepo)
        {
            _productRepo = productRepo;
        }

        public Result Add(ProductModel model)
        {
            // 1. yöntem:
            //Product existingProduct = _productRepo.Query().SingleOrDefault(p => p.Name.ToLower() == model.Name.ToLower().Trim());
            //if (existingProduct is not null)
            //    return new ErrorResult("Product with same name exists!");
            // 2. yöntem:
            //if (_productRepo.Query().Any(p => p.Name.ToLower() == model.Name.ToLower().Trim()))
            if (_productRepo.Exists(p => p.Name.ToLower() == model.Name.ToLower().Trim()))
                return new ErrorResult("Product with same name exists!");

            if (model.ExpirationDate.HasValue && model.ExpirationDate.Value <= DateTime.Today) // DateTime.Now.Date
            {
                return new ErrorResult("Expiration date must be after today!"); // data annotation
            }

            Product entity = new Product()
            {
                //CategoryId = model.CategoryId.HasValue ? model.CategoryId.Value : 0,
                //CategoryId = model.CategoryId ?? 0,
                CategoryId = model.CategoryId.Value,
                //Description = string.IsNullOrWhiteSpace(model.Description) ? null : model.Description.Trim(),
                Description = model.Description?.Trim(),
                ExpirationDate = model.ExpirationDate,
                Name = model.Name.Trim(),
                StockAmount = model.StockAmount.Value,
                UnitPrice = model.UnitPrice.Value,

                ProductStores = model.StoreIds?.Select(sId => new ProductStore()
                {
                    StoreId = sId
                }).ToList()
            };
            _productRepo.Add(entity);
            return new SuccessResult("Product added successfully.");
        }

        public Result Delete(int id)
        {
            _productRepo.Delete<ProductStore>(ps => ps.ProductId == id);

            //_productRepo.Delete(p => p.Id == id);
            _productRepo.Delete(id);
            return new SuccessResult("Product deleted successfully.");
        }

        public void Dispose()
        {
            _productRepo.Dispose();
        }

        public IQueryable<ProductModel> Query()
        {
            return _productRepo.Query(p => p.Category, p => p.ProductStores).Select(p => new ProductModel()
            {
                CategoryId = p.CategoryId,
                Description = p.Description,
                ExpirationDate = p.ExpirationDate,
                Guid = p.Guid,
                Id = p.Id,
                Name = p.Name,
                StockAmount = p.StockAmount,
                UnitPrice = p.UnitPrice,

                UnitPriceDisplay = p.UnitPrice.ToString("C2"), // "tr-TR"

                //ExpirationDateDisplay = p.ExpirationDate != null ? p.ExpirationDate.Value.ToString("MM/dd/yyyy", new CultureInfo("en-US")) : "",
                ExpirationDateDisplay = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("MM/dd/yyyy") : "",

                CategoryNameDisplay = p.Category.Name,

                StoreIds = p.ProductStores.Select(ps => ps.StoreId).ToList(),

                StoreNamesDisplay = string.Join("<br />", p.ProductStores.Select(ps => ps.Store.Name))
            });
        }

        public Result Update(ProductModel model)
        {
            if (_productRepo.Exists(p => p.Name.ToLower() == model.Name.ToLower().Trim() && p.Id != model.Id))
                return new ErrorResult("Product with same name exists!");

            _productRepo.Delete<ProductStore>(ps => ps.ProductId == model.Id);

			Product entity = new Product()
			{
				Id = model.Id,
				CategoryId = model.CategoryId.Value,
				Description = model.Description?.Trim(),
				ExpirationDate = model.ExpirationDate,
				Name = model.Name.Trim(),
				StockAmount = model.StockAmount.Value,
				UnitPrice = model.UnitPrice.Value,

                ProductStores = model.StoreIds?.Select(sId => new ProductStore()
                {
                    StoreId = sId
                }).ToList()
			};
            _productRepo.Update(entity);
            return new SuccessResult("Product updated sucessfully.");
		}
    }
}
