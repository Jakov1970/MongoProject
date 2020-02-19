using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace NisvilleFindAccommodation
{
    public class Accommodation
    {
        public ObjectId Id { get; set; }
        public string location { get; set; }
        public float size { get; set; }
        public int beds { get; set; }
        public string available { get; set; }
        public string internet { get; set; }
        public MongoDBRef agencija { get; set; }

        //ka rezervaciji
    }
}
