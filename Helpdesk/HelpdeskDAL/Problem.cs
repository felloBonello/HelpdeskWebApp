using MongoDB.Bson;
using MongoDB.Kennedy;

namespace HelpdeskDAL
{
    [System.Serializable]
    public class Problem : IMongoEntity
    {
        public ObjectId _id { get; set; }
        public string _accessId { get; set; }
        public string Description { get; set; }
        public string Entity64 { get; set; }
    }
}

