using PocMauiApp.Model;
using SQLite;

namespace PocMauiApp.Database
{
    public class DatabaseService
    {
        private readonly SQLiteConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteConnection(dbPath);
            _database.CreateTable<BlogEntry>();
            _database.CreateTable<Publisher>();
            _database.CreateTable<Status>();
        }
        // BlogEntry methods
        public IEnumerable<BlogEntry> GetAllBlogEntries()
        {
            var blogEntries = _database.Table<BlogEntry>().ToList();

            foreach (var entry in blogEntries)
            {
                // Load the related Publisher based on AssignedToId
                entry.CurrentStatus=_database.Find<Status>(entry.StatusId);
                entry.AssignedTo = _database.Find<Publisher>(entry.AssignedToId);
            }

            return blogEntries;
        }
        public BlogEntry GetBlogEntryById(int id) => _database.Find<BlogEntry>(id);
        public void AddBlogEntry(BlogEntry entry) => _database.Insert(entry);
        public void UpdateBlogEntry(BlogEntry entry)  => _database.Update(entry);
        public void DeleteBlogEntry(int id) => _database.Delete<BlogEntry>(id);

        // User methods
        public IEnumerable<Publisher> GetAllUsers() => _database.Table<Publisher>().ToList();
        public Publisher GetUserById(int id) => _database.Find<Publisher>(id);
        public void AddUser(Publisher user) => _database.Insert(user);
        public void UpdateUser(Publisher user) => _database.Update(user);
        public void DeleteUser(int id) => _database.Delete<Publisher>(id);

        // Status methods
        public IEnumerable<Status> GetAllStatuses() => _database.Table<Status>().ToList();
        public Status GetStatusById(int id) => _database.Find<Status>(id);
        public void AddStatus(Status status) => _database.Insert(status);
        public void UpdateStatus(Status status) => _database.Update(status);
        public void DeleteStatus(int id) => _database.Delete<Status>(id);
    }
}
