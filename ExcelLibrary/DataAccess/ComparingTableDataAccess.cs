using ExcelLibrary.Models;
using ExcelLibrary.HelperClasses;
using SQLite;

namespace ExcelLibrary.DataAccess;

public static class ComparingTableDataAccess
{
    private static readonly string DbPath = Constants.dbPath;

    public static void CreateComparingTableTable()
    {
        var connection = new SQLiteConnection(DbPath);
        connection.Execute(Query.CreateComparingTableTable);
        connection.Close();
    }

    public static void CreateReferenceTableTable()
    {
        var connection = new SQLiteConnection(DbPath);
        connection.Execute(Query.CreateReferenceTableTable);
        connection.Close();
    }

    public static List<ComparingTableModel> GetAllComparingTables()
    {
        var connection = new SQLiteConnection(DbPath);
        var comparingTables = connection.Query<ComparingTableModel>("SELECT * FROM ComparingTable");
        foreach (var ct in comparingTables)
        {
            var personsToBeCompared = GetPersonsOfComparingTable(ct.Id);
            ct.PersonsToBeCompared = personsToBeCompared;
        }
        return comparingTables;
    }
    
    public static void InsertComparingTable(ComparingTableModel comparingTable)
    {
        var connection = new SQLiteConnection(DbPath);
        if (connection.Insert(comparingTable) > 0)
        {
            var referenceTableEntry = new ReferenceTableModel
            {
                ComparingTableId = GetLastInsertedItemId()
            };
            foreach (var person in comparingTable.PersonsToBeCompared)
            {
                referenceTableEntry.PersonId = person.Id;
                connection.Insert(referenceTableEntry);
            }
        }
        connection.Close();
    }

    public static void DeleteComparingTable(int id)
    {
        string query = $"DELETE FROM ReferenceTable WHERE ComparingTableId = {id}";
        var connection = new SQLiteConnection(DbPath);
        connection.RunInTransaction(() =>
        {
            Console.WriteLine("executed ones");
            connection.Execute(query);
            Console.WriteLine("executed twice");
            connection.Delete<ComparingTableModel>(id.ToString());
            Console.WriteLine("executed third");
        });
    }

    public static void UpdateComparingTable(ComparingTableModel comparingTable, List<PersonModel> oldEntries, List<PersonModel> newEntries)
    {
        var referenceTable = new ReferenceTableModel()
        {
            ComparingTableId = comparingTable.Id
        };
        
        var connection = new SQLiteConnection(DbPath);
        connection.RunInTransaction(() =>
        {
            Console.WriteLine("executed ones");
            connection.Update(comparingTable);
            Console.WriteLine("executed twice");
            foreach (var person in oldEntries)
            {
                connection.Execute(@$"DELETE FROM ReferenceTable 
                                    WHERE PersonId = {person.Id} 
                                    AND ComparingTableId = {comparingTable.Id}");
            }
            Console.WriteLine("executed third");
            foreach (var person in newEntries)
            {
                referenceTable.PersonId = person.Id;
                connection.Insert(referenceTable);
            }
            Console.WriteLine("executed fourth");
        });
    }

    public static int GetLastInsertedItemId()
    {
        var connection = new SQLiteConnection(DbPath);
        var lastItemId = connection.Query<ComparingTableModel>("SELECT * FROM ComparingTable").Last().Id;
        return lastItemId;
    }

    private static List<PersonModel> GetPersonsOfComparingTable(int comparingTableId)
    {
        string query = @$"SELECT p.* FROM Person p
            INNER JOIN ReferenceTable rt ON rt.PersonId = p.Id
            INNER JOIN ComparingTable ct ON rt.ComparingTableId = ct.Id
            WHERE ct.Id = {comparingTableId}";
        var connection = new SQLiteConnection(DbPath);
        var persons = connection.Query<PersonModel>(query);
        foreach (var person in persons) {
            person.Shiftsystem = ShiftsystemDataAccess.GetOneShiftsystem(person.ShiftsystemId);
        }
        connection.Close();
        return persons;
    }
}