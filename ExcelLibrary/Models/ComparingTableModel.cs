using SQLite;

namespace ExcelLibrary.Models;
[Table("ComparingTable")]

public class ComparingTableModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Year { get; set; }
    [Ignore]
    public List<PersonModel> PersonsToBeCompared { get; set; }
}