using DBServices;
using Dtos.Database;
using GraphQL.Classes.dto;
using HotChocolate.Subscriptions;

namespace GraphQL.Classes
{
    public class Mutation
    {
        [GraphQLName("addWatchLater")]
        public async Task<WatchLaterDto> AddWatchLater(WatchLaterInput input,[Service] IWatchLaterService watchLaterService)
        {
            return await watchLaterService.AddWatchLaterAsync(new Dtos.Database.WatchLaterDto { UserId = input.userId,NewsId = input.newsId });
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
