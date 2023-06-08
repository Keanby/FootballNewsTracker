using DBRepository;
using Dtos.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServices
{
    public class WatchLaterService : IWatchLaterService
    {
        private readonly IWatchLaterRepository _watchLaterRepository;

        public WatchLaterService(IWatchLaterRepository watchLaterRepository)
        {
            _watchLaterRepository = watchLaterRepository;
        }

        public async Task<IEnumerable<WatchLaterDto>> GetAllWatchLaterAsync()
        {
            return await _watchLaterRepository.GetAllAsync();
        }

        public async Task<IEnumerable<WatchLaterDto>> GetWatchLaterByUserIdAsync(int userId)
        {
            return await _watchLaterRepository.GetByUserIdAsync(userId);
            //var all = await _watchLaterRepository.GetAllAsync();
            //return all.Where(p => p.UserId == userId);
        }

        public async Task<WatchLaterDto> AddWatchLaterAsync(WatchLaterDto watchLater)
        {
            return await _watchLaterRepository.AddAsync(watchLater);
        }

        public async Task<WatchLaterDto> UpdateWatchLaterAsync(WatchLaterDto watchLater)
        {
            // not needed for this service
            throw new NotImplementedException();
        }

        public async Task<WatchLaterDto> DeleteWatchLaterAsync(int userId, int newsId)
        {
            throw new NotImplementedException();
        }
    }
}
