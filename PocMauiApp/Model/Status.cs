using SQLite;

namespace PocMauiApp.Model
{
    public class Status
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string StatusName { get; set; }
    }
}
