using DBServices;
using Dtos.Database;
using HotChocolate.Subscriptions;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GraphQL.Classes
{
    public class Subscriptions
    {
        public async IAsyncEnumerable<int> OnPublishedStream(
            [Service] ITopicEventReceiver eventReceiver,
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            // TODO: get user id and recieve missed news
            var sourceStream = await eventReceiver.SubscribeAsync<int>(nameof(Mutation.AddPieceOfNews), cancellationToken);


            await foreach (var newsId in sourceStream.ReadEventsAsync())
            {
                yield return newsId;
            }
        }

        [Subscribe(With = nameof(OnPublishedStream))]
        public async Task<NewsDto> OnNewsAdded([EventMessage] int newsId, [Service] INewsService newsService)
        {
            return await newsService.GetNewsByIdAsync(newsId);
        }
        //[Subscribe]
        //[Topic(nameof(Mutation.AddPieceOfNews))]
        //public async Task<NewsDto> OnNewsAdded([EventMessage] int newsId, [Service] INewsService newsService)
        //{
        //    return await newsService.GetNewsByIdAsync(newsId);
        //}
    }
}
