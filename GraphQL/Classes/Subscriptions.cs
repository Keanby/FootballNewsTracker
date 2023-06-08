using DBServices;
using Dtos.Database;

namespace GraphQL.Classes
{
    public class Subscriptions
    {
        [Subscribe]
        [Topic(nameof(Mutation.AddPieceOfNews))]
        public async Task<NewsDto> OnNewsAdded([EventMessage] int newsId, [Service] INewsService newsService) {
            return await newsService.GetNewsByIdAsync(newsId);
        }
    }
}
