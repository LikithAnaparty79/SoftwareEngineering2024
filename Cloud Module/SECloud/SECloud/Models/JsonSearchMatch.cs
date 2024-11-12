using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SECloud.Models
{
    public class JsonSearchMatch
    {
        public string? FileName { get; set; }
        public JsonElement Content { get; set; }
    }
}
