using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DongGoi : BsonData.Document
    {
        public List<string> Names { get; set; } = new List<string>();

        public bool Add(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var v = value.ToLower();
            foreach (var s in Names)
            {
                if (s.ToLower() == v)
                    return false;
            }

            Names.Add(value);
            Names.Sort();

            return true;
        }
    }
}
