using Dtos.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepository
{
    public interface IWatchLaterRepository : IRepository<WatchLaterDto>
    {
        public Task<IEnumerable<WatchLaterDto>> GetByUserIdAsync(int userId);
    }
}
