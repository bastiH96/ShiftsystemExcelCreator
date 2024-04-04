using ExcelLibrary.HelperClasses;
using ExcelLibrary.Models;
using SQLite;

namespace ExcelLibrary.DataAccess;

public static class PersonDataAccess
{
    private static readonly string DbPath = Constants.dbPath;

    public static void CreatePersonTable()
    {
        var connection = new SQLiteConnection(DbPath);
        connection.Execute(Query.CreatePersonTable);
        connection.Close();
    }

    public static bool InsertPerson(PersonModel person)
    {
        var connection = new SQLiteConnection(DbPath);
        var executedLines = connection.Insert(person);
        connection.Close();
        return executedLines > 0;
    }

    public static bool DeletePerson(int id)
    {
        var connection = new SQLiteConnection(DbPath);
        var executedLines = connection.Delete<PersonModel>(id.ToString());
        connection.Close();
        return executedLines > 0;
    }

    public static bool UpdatePerson(PersonModel person)
    {
        var connection = new SQLiteConnection(DbPath);
        var executedLines = connection.Update(person);
        connection.Close();
        return executedLines > 0;
    }
    
    public static List<PersonModel> GetAllPersons()
    {
        var connection = new SQLiteConnection(DbPath);
        var persons = connection.Query<PersonModel>("SELECT * FROM Person");
        foreach(var person in persons) {
            person.Shiftsystem = ShiftsystemDataAccess.GetOneShiftsystem(person.ShiftsystemId);
        }
        connection.Close();
        return persons;
    }

    public static PersonModel GetOnePerson(int id) {
        var connection = new SQLiteConnection(DbPath);
        var person = connection.Query<PersonModel>($"SELECT * FROM Person WHERE Id = {id}").Single();
        person.Shiftsystem = ShiftsystemDataAccess.GetOneShiftsystem(person.ShiftsystemId);
        connection.Close();
        return person;
    }
    
}