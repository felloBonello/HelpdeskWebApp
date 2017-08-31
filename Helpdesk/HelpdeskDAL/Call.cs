using MongoDB.Bson;
using MongoDB.Kennedy;

namespace HelpdeskDAL
{
    [System.Serializable]
    public class Call : IMongoEntity
    {
        public ObjectId _id { get; set;  }
        public string _accessId { get; set; }
        public ObjectId EmployeeId { get; set; }
        public ObjectId ProblemId { get; set; }
        public ObjectId TechId { get; set; }
        public System.DateTime DateOpened { get; set; }
        public System.DateTime? DateClosed { get; set; }
        public bool? OpenStatus { get; set; }
        public string Notes { get; set;  }
        public string Entity64 { get; set; }
    }
}
