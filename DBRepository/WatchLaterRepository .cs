using Dapper;
using Dtos.Database;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepository
{
    public class WatchLaterRepository : IWatchLaterRepository
    {
        private readonly IDbConnection connection;
        public WatchLaterRepository(string connection_string)
        {
            connection = new NpgsqlConnection(connection_string);
        }

        public async Task<WatchLaterDto> AddAsync(WatchLaterDto entity)
        {
            return await connection.QueryFirstAsync<WatchLaterDto>("INSERT INTO watch_later(user_id, news_id) VALUES(@user_id, @news_id) RETURNING *", new { user_id = entity.UserId, news_id= entity.NewsId});
        }

        public async Task<WatchLaterDto> DeleteAsync(int id)
        {
            return await connection.QueryFirstAsync<WatchLaterDto>("DELETE FROM watch_later WHERE user_id = @id RETURNING *", new { id });
        }

        public async Task<IEnumerable<WatchLaterDto>> GetAllAsync()
        {
            return await connection.QueryAsync<WatchLaterDto>("SELECT * FROM watch_later");
        }

        public async Task<WatchLaterDto> GetByIdAsync(int userId)
        {
            //return await connection.QueryFirstOrDefaultAsync<WatchLaterDto>("SELECT * FROM watch_later WHERE user_id = @userId RETURNING *", new { userId });
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WatchLaterDto>> GetByUserIdAsync(int userId)
        {
            var response = await connection.QueryAsync("SELECT * FROM watch_later WHERE user_id = @userId", new { userId });
            var res = new List<WatchLaterDto>();
            foreach (var r in response)
            {
                res.Add(new WatchLaterDto { UserId = r.user_id, NewsId = r.news_id});
            }
            //var res = await connection.QueryAsync<WatchLaterDto>("SELECT * FROM watch_later WHERE user_id = @userId", new { userId });
            return res.AsEnumerable();
        }

        public async Task<WatchLaterDto> UpdateAsync(WatchLaterDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
