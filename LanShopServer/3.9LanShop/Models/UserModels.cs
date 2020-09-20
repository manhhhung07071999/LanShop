using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Models
{
    public class UserInfo : BsonData.Document
    {
        public string Name => ObjectId;
        public Account Account { get; set; }
        public string ObjectId { get; set; }
        public DateTime LastLogin { get; private set; }

        [JsonIgnore]
        public BsonData.DataBase Database { get; set; }

        [JsonIgnore]
        public object Profile { get; set; }

        public UserInfo()
        {
            LastLogin = DateTime.Now;
        }
    }
}