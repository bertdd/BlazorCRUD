namespace BlazorApp1;

public class Student
{
  public int Id { get; set; }

  public string? Name { get; set; }

  public override string ToString() => $"{Id} {Name}";

  public static string TableName { get; } = "student";
}
