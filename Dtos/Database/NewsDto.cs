using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dtos.Database
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Time { get; set; }
        public DateTime AddDateTime { get; set; }

    }
}
