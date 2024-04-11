<?php

/**
 * Database short summary.
 *
 * Database description.
 *
 * @version 1.0
 * @author Bert
 */
class Database
{
  public function __construct(string $host, int $port, string $database, string $user, string $password = "")
  {
    $connectionString = "mysql:host=$host;port=$port;dbname=$database";
    $this->connection = new PDO($connectionString, $user, $password);
  }

  public function Save(Student $student) : void
  {
    $statement = $this->connection->prepare(
      $student->Id == 0 ?
       "insert into " . Student::$TableName . " (name, id) values (:name, :id)" :
       "update " . Student::$TableName . " set name = :name where id = :id"
      );
    $statement->execute([ "name" => $student->Name, "id" => $student->Id]);
  }

  public function GetStudents() : array
  {
    $result = array();

    $sql = "select * from " . Student::$TableName;
    $statement = $this->connection->prepare($sql);
    $statement->setFetchMode(PDO::FETCH_CLASS, "Student");
    $statement->execute();
    while ($student = $statement->fetch())
    {
      $result[] = $student;
    }

    return $result;
  }

  private PDO $connection;
}