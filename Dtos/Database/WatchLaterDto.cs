using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Database
{
    [Table("watch_later")]
    public class WatchLaterDto
    {
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("news_id")]
        public int NewsId{ get; set; }
    }
}
