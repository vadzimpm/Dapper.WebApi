using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper.Infrastructure.Context;

namespace Dapper.Infrastructure.Services
{
    public class SqlConnectionService
    {
        private readonly DapperContext _context;
        private readonly string _sqlQuery;
        private IDbConnection _sqlConnection;

        public SqlConnectionService(DapperContext context, string sqlQuery)
        {
            _context = context;
            _sqlQuery = sqlQuery;
        }

        public delegate Task<T> ParamsAction<T>(params string[] args);

        public async Task<T> ExecuteSqlCommand<T>(ParamsAction<T> actionCallback)
        {
            using (_sqlConnection = _context.CreateConnection())
            {
                _sqlConnection.Open();
                var result = await actionCallback();
                return result;
            }
        }

        public async Task<int> AddAsync<T>(T entity)
        {
            var result = await ExecuteSqlCommand(async args => await _sqlConnection.ExecuteAsync(_sqlQuery, entity));
            return result;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var result = await ExecuteSqlCommand(async args => await _sqlConnection.ExecuteAsync(_sqlQuery, new { Id = id }));
            return result;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
        {
            var result = await ExecuteSqlCommand(async args => await _sqlConnection.QueryAsync<T>(_sqlQuery));
            return result;
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var result = await ExecuteSqlCommand(async args => await _sqlConnection.QuerySingleOrDefaultAsync<T>(_sqlQuery, new { Id = id }));
            return result;
        }

        public async Task<int> UpdateAsync<T>(T entity)
        {
            var result = await ExecuteSqlCommand(async args => await _sqlConnection.ExecuteAsync(_sqlQuery, entity));
            return result;
        }
    }
}
