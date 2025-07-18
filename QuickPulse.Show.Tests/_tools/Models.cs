namespace QuickPulse.Show.Tests._tools;

public class Models
{
    public record Person(string Name, int Age);

    public class Node(string name)
    {
        public string Name { get; } = name;
        public Node? Next { get; set; }
    }

    public class Coach
    {
        public string Name { get; }
        public string Email { get; }
        public HashSet<string> Skills { get; }

        public Coach(string name, string email)
        {
            Name = name;
            Email = email;
            Skills = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }
    }
}




