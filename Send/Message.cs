public class Message
{
    public int Id { get; set; }
    public string Comment { get; set; }
    public bool Seen { get; set; }
}

public class MessageWithError{
    public int Id { get; set; }
    public string Comment { get; set; }
    public int ErrorCount { get; set; }
}