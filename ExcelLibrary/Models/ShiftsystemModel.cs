using Newtonsoft.Json;
using ExcelLibrary.HelperClasses;
using SQLite;

namespace ExcelLibrary.Models;

[Table("Shiftsystem")]
public class ShiftsystemModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Column("Shiftpattern")]
    public string ShiftpatternSerialized 
    {
        get => JsonConvert.SerializeObject(Shiftpattern);
        set => Shiftpattern = JsonConvert.DeserializeObject<List<string>>(value); 
    }
    [Column("ShiftgroupStartdates")]
    public string ShiftgroupStartdatesSerialized
    {
        get => JsonConvert.SerializeObject(ShiftgroupStartdates); 
        set => ShiftgroupStartdates = JsonConvert.DeserializeObject<List<ShiftgroupDatetimes>>(value);
    }
    [Ignore] public List<string> Shiftpattern { get; set; }
    [Ignore] public List<ShiftgroupDatetimes> ShiftgroupStartdates { get; set; }

    // public static string ListToString(List<string> list)
    // {
    //     return JsonConvert.SerializeObject(list);
    // }
    //
    // public static List<string> StringToList(string list)
    // {
    //     return JsonConvert.DeserializeObject<List<string>>(list);
    // }
    //
    // public static string ShiftgroupStartdatesToString(List<ShiftgroupDatetimes> shiftgroupStartdates)
    // {
    //     return JsonConvert.SerializeObject(shiftgroupStartdates);
    // }
    //
    // public static List<ShiftgroupDatetimes> ShiftgroupStartdatesToList(string shiftgroupStartdates)
    // {
    //     return JsonConvert.DeserializeObject<List<ShiftgroupDatetimes>>(shiftgroupStartdates);
    // }
}

