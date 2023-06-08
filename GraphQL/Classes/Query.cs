using DBRepository;
using DBService;
using DBServices;
using Dtos.Database;
using GraphQL.Classes.dto;
using HotChocolate.Authorization;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GraphQL.Classes
{
    public class Query
    {
        [UseFiltering]
        [GraphQLName("news")]
        public async Task<IEnumerable<NewsDto>> GetAllNews([Service] INewsService newsService)
        {
            return await newsService.GetAllNewsAsync();
        }

        [GraphQLName("news")]
        public async Task<NewsDto> GetPieceOfNewsById(int id,[Service] INewsService newsService)
        {
            return await newsService.GetNewsByIdAsync(id);
        }

        [GraphQLName("users")]
        public async Task<IEnumerable<UserDto>> GetAllUsers([Service] IUsersService usersService)
        {
            return await usersService.GetAllUsersAsync();
        }

        [GraphQLName("user")]
        public async Task<UserDto> GetUserById(int id,[Service] IUsersService usersService)
        {
            return await usersService.GetUserByIdAsync(id);
        }


        [GraphQLName("watchLater")]
        public async Task<IEnumerable<WatchLaterDto>> GetAllWatchLater([Service] IWatchLaterService watchLaterService)
        {
            return await watchLaterService.GetAllWatchLaterAsync();
        }
        [Authorize]
        [GraphQLName("userWatchLater")]
        public async Task<IEnumerable<WatchLaterDto>> GetWatchLaterForUserByUserId(ClaimsPrincipal claimsPrincipal, [Service] IWatchLaterService watchLaterService)
        {
            var id = Convert.ToInt32(claimsPrincipal.FindFirstValue("id"));
            return await watchLaterService.GetWatchLaterByUserIdAsync(id);
        }

        [GraphQLName("loginUser")]
        public async Task<UserLoginPayload> LoginUser(UserLoginInput user, [Service] IUsersService usersService)
        {
            var userDb = await usersService.GetUserByUserNameAsync(user.Username);

            if(userDb is not null && user.Password == userDb.Password)
            {
                var jwtIssuer = Environment.GetEnvironmentVariable("JWT_AUTH_ISSUER") ?? throw new ArgumentException("Missing env var: AUTH_ISSUER");
                var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUTH_AUDIENCE") ?? throw new ArgumentException("Missing env var: AUTH_AUDIENCE");
                var jwtAuthKey = Environment.GetEnvironmentVariable("JWT_AUTH_KEY") ?? throw new ArgumentException("Missing env var: JWT_AUTH_KEY");

                var claims = new List<Claim> {
                    new Claim("id",userDb.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "user")
                };

                var jwt = new JwtSecurityToken(
                               issuer: jwtIssuer,
                               audience: jwtAudience,
                               claims: claims,
                               expires: DateTime.Now.AddMinutes(30),
                               signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(
                                   new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthKey)),
                                   SecurityAlgorithms.HmacSha256
                                   ));


                return new UserLoginPayload(user.Username, true, new JwtSecurityTokenHandler().WriteToken(jwt));
            }

            return new UserLoginPayload(user.Username, false, String.Empty);
        }



    }
}
