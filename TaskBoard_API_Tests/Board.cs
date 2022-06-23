using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace API_Tests
{
    public class Board
    {
        //[JsonPropertyName("id")]

        public int id { get; set; }

        //[JsonPropertyName("name")]

        public string name { get; set; }
    }
}