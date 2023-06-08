using Dtos.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBServices
{
    public interface IWatchLaterService
    {
        Task<IEnumerable<WatchLaterDto>> GetAllWatchLaterAsync();
        Task<IEnumerable<WatchLaterDto>> GetWatchLaterByUserIdAsync(int userId);
        Task<WatchLaterDto> AddWatchLaterAsync(WatchLaterDto watchLater);
        Task<WatchLaterDto> DeleteWatchLaterAsync(int userId, int newsId);
    }
}
