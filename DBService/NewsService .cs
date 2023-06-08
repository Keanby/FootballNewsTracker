using DBRepository;
using Dtos.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServices
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IEnumerable<NewsDto>> GetAllNewsAsync()
        {
            return await _newsRepository.GetAllAsync();
        }

        public async Task<NewsDto> GetNewsByIdAsync(int id)
        {
            return await _newsRepository.GetByIdAsync(id);
        }

        public async Task<NewsDto> AddNewsAsync(NewsDto news)
        {
            return await _newsRepository.AddAsync(news);
        }

        public async Task UpdateNewsAsync(NewsDto news)
        {
            await _newsRepository.UpdateAsync(news);
        }

        public async Task DeleteNewsAsync(int id)
        {
            await _newsRepository.DeleteAsync(id);
        }
    }
}
