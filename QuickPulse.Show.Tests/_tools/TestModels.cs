namespace QuickPulse.Show.Tests._tools;

public record Person(string Name, int Age);

public class Node(string name)
{
    public string Name { get; } = name;
    public Node? Next { get; set; }
}
