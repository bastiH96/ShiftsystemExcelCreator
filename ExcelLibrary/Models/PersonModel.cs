using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace ExcelLibrary.Models;

[SQLite.Table("Person")]
public class PersonModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string TableName { get; set; }
    [ForeignKey("ShiftsystemId")]
    public int ShiftsystemId { get; set; }
    public int Shiftgroup { get; set; }
    [Ignore]
    public ShiftsystemModel Shiftsystem { get; set; }
}