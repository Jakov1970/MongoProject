using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace NisvilleFindAccommodation
{
    public class Reservation
    {
        public ObjectId Id { get; set; }

        public MongoDBRef participant { get; set; }

        public MongoDBRef accommodation { get; set; }
                
    }
}
