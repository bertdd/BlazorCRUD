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
      $"insert into {Student.TableName} (name, id) values (@name, @id)" :
      $"update {Student.TableName} set name = @name where id = @id";
    command.Parameters.AddWithValue("name", student.Name);
    command.Parameters.AddWithValue("id", student.Id);
    await command.ExecuteNonQueryAsync();
  }

  public async Task<List<Student>> GetStudents(string? name = null)
  {
    var result = new List<Student>();
    using var command = connection!.CreateCommand();
    command.CommandText = $"select * from {Student.TableName}";
    if (name != null)
    {
      command.CommandText += " where name like @name";
      command.Parameters.AddWithValue("name", name + '%');
    }
    using var reader = await command.ExecuteReaderAsync();
    while (await reader.ReadAsync())
    {
      result.Add(
          new Student
          {
            Id = (int)reader["id"],
            Name = (string)reader["name"]
          });
    }
    return result;
  }

  public async Task Delete(Student student)
  {
    if (student.Id != 0)
    {
      using var command = connection!.CreateCommand();
      command.CommandText = $"delete from {Student.TableName} where id = @id";
      command.Parameters.AddWithValue("id", student.Id);
      await command.ExecuteNonQueryAsync();
    }
  }

  public async Task<Student> GetStudent(int id)
  {
    Student result = new();
    if (id > 0)
    {
      using var command = connection!.CreateCommand();
      command.CommandText = $"select * from {Student.TableName} where id = @id";
      command.Parameters.AddWithValue("id", id);
      using var reader = await command.ExecuteReaderAsync();
      if (await reader.ReadAsync())
      {
        result = new Student
        {
          Id = (int)reader["id"],
          Name = (string)reader["name"]
        };
      }
    }
    return result;
  }

  readonly MySqlConnection? connection;
}
