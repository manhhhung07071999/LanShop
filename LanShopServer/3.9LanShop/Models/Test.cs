using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Test<T> : BsonData.Document
        where T: BsonData.IDocument
    {
        public List<T> Members { get; set; } = new List<T>();

        public void AddMember(T member)
        {
            Members.Add(member);
        }
        public void RemoveMember(T member)
        {
            
        }
    }
}
