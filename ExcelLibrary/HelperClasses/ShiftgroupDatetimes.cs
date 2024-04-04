namespace ExcelLibrary.HelperClasses;

public class ShiftgroupDatetimes
{
    public string Shiftgroup { get; set; }
    public DateTime Startdate { get; set; }

    public ShiftgroupDatetimes(string shiftgroup)
    {
        Shiftgroup = shiftgroup;
        Startdate = DateTime.Now;
    }
}