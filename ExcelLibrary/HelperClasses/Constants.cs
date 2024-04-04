namespace ExcelLibrary.HelperClasses; 
public static class Constants {
    private const string DbName = "ShiftComparingDb.db";
    // private const string FolderPathMac = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public const string FolderPathWindows = "D:\\SQLite Databases";
    // public static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), dbName);
    public static string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DbName);

    public const int ExcelSheetFirstRow = 2;
    public const int ExcelSheetFirstColumn = 2;
}
