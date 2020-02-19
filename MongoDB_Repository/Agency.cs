using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace NisvilleFindAccommodation
{
    public class Agency
    {
        public ObjectId Id { get; set; }
        public string city { get; set; }
        public string name { get; set; }
        public string telephone { get; set; }
        public List<MongoDBRef> Accommodations { get; set; }

        public Agency()
        {
            Accommodations = new List<MongoDBRef>();
        }
    }
}

