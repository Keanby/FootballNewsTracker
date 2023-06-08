using DBServices;
using Dtos.Database;
using GraphQL.Classes.dto;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using System.Security.Claims;

namespace GraphQL.Classes
{
    public class Mutation
    {
        [Authorize]
        [GraphQLName("addWatchLater")]
        public async Task<WatchLaterDto> AddWatchLater(WatchLaterInput input, ClaimsPrincipal claimsPrincipal,[Service] IWatchLaterService watchLaterService)
        {
            var id = Convert.ToInt32(claimsPrincipal.FindFirstValue("id"));
            return await watchLaterService.AddWatchLaterAsync(new Dtos.Database.WatchLaterDto { UserId = id, NewsId = input.newsId });
        }
        [GraphQLName("addPieceOfNews")]
        public async Task<NewsDto> AddPieceOfNews(
            PieceOfNews input,
            [Service] INewsService newsService,
            [Service] ITopicEventSender eventSender,
            CancellationToken cancellationToken)
        {
            var added = await newsService.AddNewsAsync(
                new NewsDto 
                { 
                    Link = input.link,
                    Time = input.time,
                    Title = input.title,
                    AddDateTime = input.addDateTime,
            });

            await eventSender.SendAsync(nameof(AddPieceOfNews), added.Id, cancellationToken);
            return added;
        }

    }
}
