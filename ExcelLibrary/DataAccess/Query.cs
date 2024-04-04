namespace ExcelLibrary.DataAccess;

public static class Query
{
    public const string CreateShiftsystemTable = @"CREATE TABLE IF NOT EXISTS Shiftsystem (
            Id INTEGER NOT NULL,
            Name TEXT NOT NULL,
            Description TEXT NOT NULL,
            Shiftpattern TEXT NOT NULL,
            ShiftgroupStartdates TEXT NOT NULL,
            PRIMARY KEY(Id AUTOINCREMENT))";
    
    public const string CreatePersonTable = @"CREATE TABLE IF NOT EXISTS Person (
            Id INTEGER NOT NULL,
            Name TEXT NOT NULL,
            TableName TEXT NOT NULL,
            ShiftsystemId INTEGER,
            Shiftgroup INTEGER,
            FOREIGN KEY(ShiftsystemId) REFERENCES Shiftsystem(Id),
            PRIMARY KEY(Id AUTOINCREMENT))";

    public const string CreateComparingTableTable = @"CREATE TABLE IF NOT EXISTS ComparingTable (
            Id INTEGER NOT NULL,
            Name TEXT NOT NULL,
            Year INTEGER NOT NULL,
            PRIMARY KEY(Id AUTOINCREMENT))";

    public const string CreateReferenceTableTable = @"CREATE TABLE IF NOT EXISTS ReferenceTable(
            Id INTEGER NOT NULL,
            ComparingTableId INTEGER NOT NULL,
            PersonId INTEGER NOT NULL,
            FOREIGN KEY(ComparingTableId) REFERENCES ComparingTable(Id),
            FOREIGN KEY(PersonId) REFERENCES Person(Id),
            PRIMARY KEY(Id AUTOINCREMENT))";
}