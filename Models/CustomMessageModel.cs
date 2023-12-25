public class CustomMessageModel
{
    public int Age { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Subject { get; set; }
}

public class MoviesMessageModel
{
    public int Year { get; set; }
    public string Name { get; set; }
    public string Director { get; set; }
}

public class StandartMessageModel
{
    public string MessageType { get; set; }
    public string MessageBody { get; set; }
}