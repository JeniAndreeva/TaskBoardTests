using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Tests
{
    public class Task
    {
        //[JsonPropertyName("id")]
        public int id { get; set; }

        //[JsonPropertyName("title")]
        public string title { get; set; }

        //[JsonPropertyName("description")]
        public string description { get; set; }

        //[JsonPropertyName("board")]
        public Board board { get; set; }
    }
}
