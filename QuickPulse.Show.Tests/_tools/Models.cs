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

    public class Enumy { public DayOfWeek Day { get; set; } }

    public static class Forest
    {
        public abstract class Tree { }

        public class Leaf : Tree
        {
            public int Value { get; set; }
        }

        public class Branch : Tree
        {
            public Tree? Left { get; set; }
            public Tree? Right { get; set; }
        }
    }

    public class Horses
    {
        public enum WeekDays
        {
            Monday = 1,
            Tuesday,
            Wednesday,
            Thursday,
            Friday
        }

        public class TimeSlotJasonDTO
        {
            public WeekDays Day { get; set; }

            public int Start { get; set; }
            public int End { get; set; }
        }

        public class GetByIdResponse
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public List<string> Skills { get; set; } = new();
            public List<TimeSlotJasonDTO> TimeSlots { get; set; } = new();
            //public CoachBasicDTO? Coach { get; set; }                           // Genest Coach DTO, nullable
        }
    }
}




