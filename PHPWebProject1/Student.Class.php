<?php

/**
 * Student short summary.
 *
 * Student description.
 *
 * @version 1.0
 * @author Bert
 */
class Student
{
  public int $Id;

  public ?string $Name;

  public function __toString() : string
  {
    return"$this->Id $this->Name";
  }

  public static string $TableName = "student";
}