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
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection connection;
        public UserRepository(string connection_string)
        {
            connection = new NpgsqlConnection(connection_string);
        }

        public async Task<UserDto> AddAsync(UserDto entity)
        {
            return await connection.QueryFirstAsync<UserDto>("INSERT INTO users(username, password) VALUES(@Username, @Password) RETURNING *", entity);
        }

        public async Task<UserDto> DeleteAsync(int id)
        {
           return await connection.QueryFirstAsync<UserDto>("DELETE FROM users WHERE id = @id RETURNING *", new { id });
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return  await connection.QueryAsync<UserDto>("SELECT * FROM users");
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            return await connection.QueryFirstOrDefaultAsync<UserDto>("SELECT * FROM users WHERE id = @id", new { id });
        }

        public async Task<UserDto> UpdateAsync(UserDto entity)
        {
            return await connection.QueryFirstAsync<UserDto>("UPDATE users SET username = @Username, password = @Password WHERE id = @Id RETURNING *", entity);
        }
    }
}
