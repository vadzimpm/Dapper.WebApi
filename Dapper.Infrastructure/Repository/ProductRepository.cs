using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.Application.Interfaces;
using Dapper.Core.Entities;
using Dapper.Infrastructure.Context;
using Dapper.Infrastructure.Services;

namespace Dapper.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly DapperContext _context;
        private SqlConnectionService _sqlConnectionService;

        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Product entity)
        {
            entity.AddedOn = DateTime.Now;
            var sql = "Insert into Products (Name,Description,Barcode,Rate,AddedOn) VALUES (@Name,@Description,@Barcode,@Rate,@AddedOn)";

            this._sqlConnectionService = new SqlConnectionService(_context, sql);
            var result = await _sqlConnectionService.AddAsync(entity);
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Products WHERE Id = @Id";

            this._sqlConnectionService = new SqlConnectionService(_context, sql);

            var result = await _sqlConnectionService.DeleteAsync(id);
            return result;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var sql = "SELECT * FROM Products";

            this._sqlConnectionService = new SqlConnectionService(_context, sql);

            var result = await _sqlConnectionService.GetAllAsync<Product>();
            return result.ToList();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id = @Id";

            this._sqlConnectionService = new SqlConnectionService(_context, sql);

            var result = await _sqlConnectionService.GetByIdAsync<Product>(id);
            return result;
        }

        public async Task<int> UpdateAsync(Product entity)
        {
            entity.ModifiedOn = DateTime.Now;
            var sql = "UPDATE Products SET Name = @Name, Description = @Description, Barcode = @Barcode, Rate = @Rate, ModifiedOn = @ModifiedOn  WHERE Id = @Id";

            this._sqlConnectionService = new SqlConnectionService(_context, sql);

            var result = await _sqlConnectionService.UpdateAsync(entity);
            return result;
        }
    }
}
