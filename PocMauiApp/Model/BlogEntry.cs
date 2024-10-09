using SQLite;

namespace PocMauiApp.Model;

public class BlogEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string BlogTopic { get; set; }

    public int AssignedToId { get; set; } // Foreign key to User

    [Ignore]
    public Publisher AssignedTo { get; set; } // Navigation property

    public DateTime AssignmentDate { get; set; }

    public string TeamLeadName { get; set; }

    public DateTime PublishedDate { get; set; }

    public string BlogLink { get; set; }

    public string Notes { get; set; }
    public int StatusId { get; set; } // Foreign key to Status

    [Ignore]
    public Status CurrentStatus { get; set; }
}

