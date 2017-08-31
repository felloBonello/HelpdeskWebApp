using MongoDB.Driver.Linq;
using MongoDB.Kennedy;
using System.Linq;


namespace HelpdeskDAL
{
    public class DbContext : ConcurrentDataContext
    {
        public DbContext(string databaseName = "HelpdeskDB",
            string serverName = "localhost") :
            base(databaseName, serverName)
        {
        }
        
        public IQueryable<Employee> Employees
        {
            get
            {
                return this.Db.GetCollection<Employee>("employees").AsQueryable();
            }
        }

        public IQueryable<Department> Departments
        {
            get
            {
                return this.Db.GetCollection<Department>("departments").AsQueryable();
            }
        }

        public IQueryable<Problem> Problems
        {
            get
            {
                return this.Db.GetCollection<Problem>("problems").AsQueryable();
            }
        }

        public IQueryable<Call> Calls
        {
            get
            {
                return this.Db.GetCollection<Call>("calls").AsQueryable();
            }
        }
    }
}
