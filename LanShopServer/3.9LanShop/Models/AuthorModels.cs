using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Author : BsonData.Document
    {
        public const string SupperAdmin = "Admin";
        public const string Worker = "Worker";
        public const string Company = "Company";
        public const string Spam = "Spam";
        public string Text { get; set; }
    }
}
