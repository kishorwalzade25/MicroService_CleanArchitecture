using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Product.Core.Entities;
using Product.Core.RabbitMQ;
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
        private readonly IRabbitMQPublisher _rabbitMQPublisher;

        public ProductsRepository(ProductDbContext productDbContext, IMapper mapper, IRabbitMQPublisher rabbitMQPublisher)
        {
            _dbContext = productDbContext;
            _mapper = mapper;
            _rabbitMQPublisher = rabbitMQPublisher;
        }


        public async Task<ProductResponse?> AddProduct(ProductAddRequest productAddRequest)
        {
            var product = _mapper.Map<ProductItem>(productAddRequest);
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

            if (affectedRowsCount > 0)
            {
                //for Direct and Topic excahnge
                // string routingKey = "product.delete";

                var headers = new Dictionary<string, object>()
                {
                      { "event", "product.delete" },
                      { "RowCount", 1 }
                 };



                ProductDeletionMessage productDeletionMessage = new ProductDeletionMessage()
                {
                    ProductID = existingProduct.ProductID,
                    ProductName = existingProduct.ProductName,
                };
                //for Direct exchange
                //_rabbitMQPublisher.Publish<ProductDeletionMessage>(routingKey, productDeletionMessage);

                //for header exchange
                _rabbitMQPublisher.Publish<ProductDeletionMessage>(headers, productDeletionMessage);
            }


            return affectedRowsCount > 0;
        }



        public async Task<ProductResponse?> GetProductByCondition(int productId)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (product == null) { return null; }
            ;
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

            //Check if product name is changed
            bool isProductNameChanged = productUpdateRequest.ProductName != existingProduct.ProductName;

            existingProduct.ProductName = productUpdateRequest.ProductName!;
            existingProduct.UnitPrice = productUpdateRequest.UnitPrice!;
            existingProduct.QuantityInStock = productUpdateRequest.QuantityInStock!;
            existingProduct.Category = productUpdateRequest.Category!;



            await _dbContext.SaveChangesAsync();


            //Publish product.update.name message to the exchange
            if (isProductNameChanged)
            {
                // for direct exchange
                //string routingKey = "product.update.name";

                //for header exchange
                //var headers = new Dictionary<string, object>()
                //{  
                //       { "event", "product.update" },
                //       { "field", "name" },
                //       { "RowCount", 1 }
                //};



                //var message = new ProductNameUpdateMessage()
                //{ ProductID = productUpdateRequest.ProductID, NewName = existingProduct.ProductName };

                //for direct exchange
                //_rabbitMQPublisher.Publish<ProductNameUpdateMessage>(routingKey, message);

                //for header exchange
                //_rabbitMQPublisher.Publish<ProductNameUpdateMessage>(headers, message);


                //for caching update
                var headers = new Dictionary<string, object>()
                {
                       { "event", "product.update" },
                     
                       { "RowCount", 1 }
                };

                _rabbitMQPublisher.Publish<ProductItem>(headers, existingProduct);

            }



            var existingProductResponse = _mapper.Map<ProductResponse>(existingProduct);
            return existingProductResponse;
        }
    }
}
