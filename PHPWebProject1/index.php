<?php
require_once "Student.Class.php";
require_once "Database.Class.php";

if (isset($_POST) && isset($_POST["button"])) {
  PerformAction($_POST);
}
?>

<html>
<head>
  <title>CRUD in PHP</title>
</head>
<body>
  <form action="index.php" method="post">
    <table border="0">
      <tr>
        <td>Id</td>
        <td><input type="text" readonly name="id" value="<?php $student->Id?>" /></td>
      </tr>
      <tr>
        <td>Naam</td>
        <td><input type="text" name="name" value="<?php $student->Name?>"/></td>
      </tr>
    </table>
    <button type="submit" name="button" value="clear">Nieuw</button>
    <button type="submit" name="button" value="search">Zoek</button>
    <button type="submit" name="button" value="save">Opslaan</button>
    <button type="submit" name="button" value="delete">Verwijder</button>
  </form>
  <hr/>
</body>
</html>

<?php
ShowStudents();

$student = new Student();
function PerformAction(array $data) : void
{
  $action = $data["button"];
  $student = new Student();
  $student->Id = (int) $data["id"];
  $student->Name = $data["name"];

  switch ($action)
  {
    case "clear":
      Clear();
      break;

    case "save":
      Save($student);
      break;
  }
}

function Clear() : void
{
  global $student;
  $student = new Student();
}

function Save(Student $student) : void
{
  $database = new Database("localhost", 3306, "test", "root");
  $database->Save($student);
}

function ShowStudents() : void
{
  $database = new Database("localhost", 3306, "test", "root");
  echo "<table>";
  foreach ($database->GetStudents() as $student)
  {
    echo "<tr>";
    echo "<td>$student->Id</td>";
    echo "<td>$student->Name</td>";
    echo "</tr>";
  }
  echo "</table>";
}

?>
