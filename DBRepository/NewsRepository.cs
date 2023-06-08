using Dapper;
using Dtos.Database;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DBRepository
{
    public class NewsRepository : INewsRepository
    {
        private readonly IDbConnection connection;

        public NewsRepository(string connection_string)
        {
            connection = new NpgsqlConnection(connection_string);
        }

        public async Task<NewsDto> AddAsync(NewsDto entity)
        {
            return await connection.QueryFirstOrDefaultAsync<NewsDto>("INSERT INTO news (link, title, time, add_datetime) VALUES (@Link, @Title, @Time, @AddDateTime) ON CONFLICT ON CONSTRAINT unique_link DO NOTHING RETURNING *", entity);
        }

        public async Task<NewsDto> DeleteAsync(int id)
        {
            return await connection.QueryFirstAsync<NewsDto>("DELETE FROM news WHERE id = @id RETURNING *", id);
        }

        public async Task<IEnumerable<NewsDto>> GetAllAsync()
        {
            return await connection.QueryAsync<NewsDto>("SELECT * FROM news");
        }

        public async Task<NewsDto> GetByIdAsync(int id)
        {
            return await connection.QueryFirstOrDefaultAsync<NewsDto>("SELECT * FROM news WHERE id = @id", new { id });
        }

        public async Task<NewsDto> UpdateAsync(NewsDto entity)
        {
            return await connection.QueryFirstAsync<NewsDto>("UPDATE news SET link = @Link, title = @Title, time = @Time, add_datetime = @AddDateTime WHERE id = @Id RETURNING *", entity);
        }
    }
}
