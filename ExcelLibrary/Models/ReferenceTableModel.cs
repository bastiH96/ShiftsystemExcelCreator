using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace ExcelLibrary.Models;

[SQLite.Table("ReferenceTable")]
public class ReferenceTableModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [ForeignKey("ComparingTableId")]
    public int ComparingTableId { get; set; }
    [ForeignKey("PersonId")]
    public int PersonId { get; set; }
}