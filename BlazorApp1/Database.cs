using MySql.Data.MySqlClient;

namespace BlazorApp1;

public class Database : IDisposable
{
  public Database(string server, uint port, string database, string userID, string password = "")
  {
    var connectionString = new MySqlConnectionStringBuilder
    {
      Server = server,
      Port = port,
      UserID = userID,
      Password = password,
      Database = database,
    }.ToString();

    connection = new MySqlConnection(connectionString);
    connection.Open();
  }

  public void Dispose()
  {
    GC.SuppressFinalize(this);
    connection?.Dispose();
  }

  public async Task Save(Student student)
  {
    using var command = connection!.CreateCommand();
    command.CommandText = student.Id == 0 ?
      "insert into student (name, id) values (@name, @id)" :
      "update student set name = @name where id = @id";
    command.Parameters.AddWithValue("@name", student.Name);
    command.Parameters.AddWithValue("@id", student.Id);
    await command.ExecuteNonQueryAsync();
  }

  public async Task<List<Student>> GetStudents()
  {
    var result = new List<Student>();
    using var command = connection!.CreateCommand();
    command.CommandText = "select * from student";
    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
      result.Add(
          new Student
          {
            Id = (int)reader["Id"],
            Name = (string)reader["Name"]
          });
    }
    return result;
  }

  public async Task Delete(Student student)
  {
    if (student.Id != 0)
    {
      using var command = connection!.CreateCommand();
      command.CommandText = "delete from student where id = @id";
      command.Parameters.AddWithValue("@id", student.Id);
      await command.ExecuteNonQueryAsync();
    }
  }

  public async Task<Student> GetStudent(int id)
  {
    Student result = new();
    if (id > 0)
    {
      using var command = connection!.CreateCommand();
      command.CommandText = "select * from student where id = @id";
      command.Parameters.AddWithValue("@id", id);
      using var reader = await command.ExecuteReaderAsync();
      if (await reader.ReadAsync())
      {
        result = new Student
        {
          Id = (int)reader["Id"],
          Name = (string)reader["Name"]
        };
      }
    }
    return result;
  }

  readonly MySqlConnection? connection;
}
