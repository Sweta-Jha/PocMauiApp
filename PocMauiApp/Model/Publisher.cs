using SQLite;


namespace PocMauiApp.Model
{
    public class Publisher
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
