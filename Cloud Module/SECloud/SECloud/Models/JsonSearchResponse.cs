using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SECloud.Models
{
    public class JsonSearchResponse
    {
        public string? SearchKey { get; set; }
        public string? SearchValue { get; set; }
        public int MatchCount {  get; set; }
        public List<JsonSearchMatch>? Matches { get; set; }

    }
}
