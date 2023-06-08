using Dtos.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServices
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetAllNewsAsync();
        Task<NewsDto> GetNewsByIdAsync(int id);
        Task<NewsDto> AddNewsAsync(NewsDto news);
        Task UpdateNewsAsync(NewsDto news);
        Task DeleteNewsAsync(int id);
    }
}
