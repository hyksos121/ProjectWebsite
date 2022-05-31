using ProjectWebsite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProjectWebsite.Data
{
    public class DBService
    {
        private readonly testWebsiteDBContext _context;

        public DBService(testWebsiteDBContext dbcontext)
        {
            _context = dbcontext;
        }

        //-------------------------------------------------------------------------------------------

        #region User Functions
        public async Task<List<Users>> GetUserAsync(string AspId)
        {
            var userList = await _context.Users.Where(u => u.AspId == AspId).ToListAsync();
            return userList;
        }

        public async Task<string> GetUserNameAsync(int id)
        {
            Users user = _context.Users.Find(id);
            string userName = user.PublicUserName;
            return userName;
        }

        public async Task<List<Users>> GetAllUserAsync()
        {
            var userList = await _context.Users.ToListAsync();
            return userList;
        }
        #endregion
        
        //-------------------------------------------------------------------------------------------
        
        #region Products Functions
        public async Task<List<Products>> GetProductsAsync()
        {
            var ProductsList = await _context.Products.ToListAsync();
            return ProductsList;
        }

        public async Task<Products> CreateProductAsync(Products objProduct)
        {
            _context.Products.Add(objProduct);
            _context.SaveChanges();
            return await Task.FromResult(objProduct);
        }

        public Task<bool> DeleteProductAsync(Products product)
        {
            var ExistingProduct = _context.Products.Find(product.ProductId);
            
            if(ExistingProduct != null)
            {
                ExistingProduct.IsBought = true;
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        #endregion
        
        //-------------------------------------------------------------------------------------------
        
        #region ProductTransactions Functions
        public async Task<List<ProductTransactions>> GetProductTransactionsAsync(int userId)
        {
            var PTransList = await _context.ProductTransactions.Where(p => p.BuyerId == userId).ToListAsync();
            return PTransList;
        }

        public async Task<List<ProductTransactions>> GetAllProductTransactionsAsync()
        {
            var PTransList = await _context.ProductTransactions.ToListAsync();
            return PTransList;
        }

        public async Task<ProductTransactions> CommitProductTransaction(int BuyerId,Products product, DateTime now)
        {
            var objProdTrans = new ProductTransactions();
            int newId;

            try
            {
                newId = _context.ProductTransactions.Max(pt => pt.TransactionId) + 1;
            }
            catch
            {
                newId = 1;
            }

            objProdTrans.TransactionId = newId;
            objProdTrans.BuyerId = BuyerId;
            objProdTrans.SellerId = product.UserId;
            objProdTrans.ProductId = product.ProductId;
            objProdTrans.TransactionDate = now;

            _context.ProductTransactions.Add(objProdTrans);
            _context.SaveChanges();
            return await Task.FromResult(objProdTrans);
        }

        #endregion
        
        //-------------------------------------------------------------------------------------------
        
        #region Services Functions
        public async Task<List<Services>> GetServicesAsync()
        {
            var ServicesList = await _context.Services.ToListAsync();
            return ServicesList;
        }

        public Task<Services> CreateServiceAsync(Services objService)
        {
            _context.Services.Add(objService);
            _context.SaveChanges();
            return Task.FromResult(objService);
        }

        public Task<bool> DeleteServiceAsync(Services service)
        {
            var ExistingService = _context.Services.Find(service.ServiceId);

            if (ExistingService != null)
            {
                ExistingService.IsBought = true;
                _context.SaveChanges();
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        #endregion

        //-------------------------------------------------------------------------------------------

        #region ServiceTransactions Functions
        public async Task<List<ServiceTransactions>> GetServiceTransactionsAsync(int userId)
        {
            var STransList = await _context.ServiceTransactions.Where(s => s.BuyerId == userId).ToListAsync();
            return STransList;
        }

        public async Task<List<ServiceTransactions>> GetAllServiceTransactionsAsync()
        {
            var STransList = await _context.ServiceTransactions.ToListAsync();
            return STransList;
        }

        public async Task<ServiceTransactions> CommitServiceTransaction(int BuyerId, Services service, DateTime now)
        {
            var objServTrans = new ServiceTransactions();
            int newId;

            try
            {
                newId = _context.ServiceTransactions.Max(pt => pt.TransactionId) + 1;
            }
            catch
            {
                newId = 1;
            }


            objServTrans.TransactionId = newId;
            objServTrans.BuyerId = BuyerId;
            objServTrans.SellerId = service.UserId;
            objServTrans.ServiceId = service.ServiceId;
            objServTrans.TransactionDate = now;

            _context.ServiceTransactions.Add(objServTrans);
            _context.SaveChanges();
            return await Task.FromResult(objServTrans);
        }
        #endregion
        
        //-------------------------------------------------------------------------------------------

        #region Service Rating
        public async Task<double> GetAvgRating(int ServiceId)
        {
            double avgRating = _context.ServiceRating.Where(r => r.ServiceId == ServiceId)
                                                        .Select(r => r.Rating)
                                                        .DefaultIfEmpty()
                                                        .Average();
            return avgRating;
        }
        #endregion
    }
}
