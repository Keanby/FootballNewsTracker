using DBRepository;
using DBService;
using DBServices;
using Dtos.Database;

namespace GraphQL.Classes
{
    public class Query
    {

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
        [GraphQLName("userWatchLater")]
        public async Task<IEnumerable<WatchLaterDto>> GetWatchLaterForUserByUserId(int user_id,[Service] IWatchLaterService watchLaterService)
        {
            return await watchLaterService.GetWatchLaterByUserIdAsync(user_id);
        }



    }
}
