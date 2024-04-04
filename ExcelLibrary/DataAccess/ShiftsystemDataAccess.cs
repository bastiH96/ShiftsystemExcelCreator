using ExcelLibrary.HelperClasses;
using ExcelLibrary.Models;
using SQLite;

namespace ExcelLibrary.DataAccess;

public static class ShiftsystemDataAccess
{
    private static readonly string DbPath = Constants.dbPath;

    public static void CreateShiftsystemTable()
    {
        var connection = new SQLiteConnection(DbPath);
        connection.Execute(Query.CreateShiftsystemTable);
        connection.Close();
    }

    public static int AddNewShiftsystem(ShiftsystemModel shiftsystem)
    {
        var connection = new SQLiteConnection(DbPath);
        connection.Insert(shiftsystem);
        var id = connection.Query<ShiftsystemModel>("SELECT * FROM Shiftsystem").ToList().Last().Id;
        connection.Close();
        return id;
    }

    public static bool DeleteShiftsystem(int id)
    {
        var connection = new SQLiteConnection(DbPath);
        var executedLines = connection.Delete<ShiftsystemModel>(id.ToString());
        connection.Close();
        return executedLines > 0;
    }

    public static bool UpdateShiftsystem(ShiftsystemModel shiftsystem)
    {
        var connection = new SQLiteConnection(DbPath);
        var executedLines = connection.Update(shiftsystem);
        connection.Close();
        return executedLines > 0;
    }

    public static List<ShiftsystemModel> GetAllShiftsystems()
    {
        var connection = new SQLiteConnection(DbPath);
        var shiftsystems = connection.Query<ShiftsystemModel>("SELECT * FROM Shiftsystem").ToList();
        connection.Close();
        return shiftsystems;
    }

    public static ShiftsystemModel GetOneShiftsystem(int id)
    {
        var connection = new SQLiteConnection(DbPath);
        var shiftsystem = connection.Query<ShiftsystemModel>($"SELECT * FROM Shiftsystem WHERE Id = {id}");
        connection.Close();
        return shiftsystem.Single();
    }

    
}