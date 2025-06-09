using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Product.Core.Entities;
using Product.Core.RepositriesService;
using Product.Infrastructure.ProductContextDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Product.Infrastructure.Repositories
{
    public class ProductsRepository : IProductService
    {
        private readonly ProductDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductsRepository(ProductDbContext productDbContext,IMapper mapper) 
        {
            _dbContext = productDbContext;
            _mapper = mapper;
        }


        public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
        {
            var product= _mapper.Map<ProductItem>(productAddRequest);
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            if (product == null) { return null; }
            var productRepo = _mapper.Map<ProductResponse>(product);
            return productRepo;
        }

        public async Task<bool> DeleteProduct(int productID)
        {
            ProductItem? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == productID);
            if (existingProduct == null)
            {
                return false;
            }

            _dbContext.Products.Remove(existingProduct);
            int affectedRowsCount = await _dbContext.SaveChangesAsync();
            return affectedRowsCount > 0;
        }

       

        public async Task<ProductResponse?> GetProductByCondition(int productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (product == null) { return null; };
            var productResponse = _mapper.Map<ProductResponse>(product);
            return productResponse; 
        }

        public async Task<List<ProductResponse?>> GetProducts()
        {
            return _mapper.Map<List<ProductResponse?>>(await _dbContext.Products.ToListAsync());
        }

        public Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<ProductItem, bool>> conditionExpression)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
        {
            ProductItem? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == productUpdateRequest.ProductID);
            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.ProductName = productUpdateRequest.ProductName!;
            existingProduct.UnitPrice = productUpdateRequest.UnitPrice!;
            existingProduct.QuantityInStock = productUpdateRequest.QuantityInStock!;
            existingProduct.Category = productUpdateRequest.Category!;

            await _dbContext.SaveChangesAsync();
            var existingProductResponse = _mapper.Map<ProductResponse>(existingProduct);
            return existingProductResponse;
        }
    }
}
