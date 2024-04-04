using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using ExcelLibrary.Models;
using Path = System.IO.Path;

namespace ExcelCalculator;

public class ExcelService
{
    private readonly ComparingTableModel _comparingTable;
    private readonly string _worksheetName;
    private readonly string _folderPath;

    private const int FirstRow = 2;
    private const int FirstColumn = 2;

    private IXLWorksheet _worksheet;
    private readonly List<PersonModel> _personsToBeCompared;
    private readonly List<int> _personShiftpatternIterators = new();
    private readonly List<List<string>> _listOfShiftpattern = new();

    public ExcelService(ComparingTableModel comparingTable, string worksheetName, string folderPath)
    {
        _comparingTable = comparingTable;
        _worksheetName = worksheetName;
        _folderPath = folderPath;
        _personsToBeCompared = _comparingTable.PersonsToBeCompared;
    }

    public void CreateComparingTableInCsvFile()
    {
        SetShiftpatternIterators();
        using (var workbook = new XLWorkbook())
        {
            CreateWorksheet(workbook);
            AddContentToWorksheet();
            TableStyling();
            SaveWorkbook(workbook);
        }
        Console.WriteLine("Excel file created");
    }
    
    private void SetShiftpatternIterators()
    {
        foreach (var person in _personsToBeCompared)
        {
            var shiftpatternStartDate = person.Shiftsystem.ShiftgroupStartdates[person.Shiftgroup].Startdate;
            var comparingTableYear = new DateTime(_comparingTable.Year, 1, 1);
            var timeSpan = Math.Abs((int)(shiftpatternStartDate - comparingTableYear).TotalDays);
            var personIndex = _personsToBeCompared.IndexOf(person);
            _personShiftpatternIterators.Add(0);
            _listOfShiftpattern.Add(person.Shiftsystem.Shiftpattern);
            
            if (shiftpatternStartDate < comparingTableYear)
            {
                _personShiftpatternIterators[personIndex] = timeSpan % person.Shiftsystem.Shiftpattern.Count;
            } 
            else if (shiftpatternStartDate > comparingTableYear)
            {
                _personShiftpatternIterators[personIndex] = person.Shiftsystem.Shiftpattern.Count - (timeSpan % person.Shiftsystem.Shiftpattern.Count);
                if (person.Shiftsystem.Shiftpattern.Count == _personShiftpatternIterators[personIndex])
                    _personShiftpatternIterators[personIndex] = 0;
            }
        }
    }

    private void CreateWorksheet(XLWorkbook workbook)
    {
        _worksheet = workbook.Worksheets.Add(_worksheetName);
    }

    private void AddContentToWorksheet()
    {
        var firstColumn = FirstColumn;
        for (var month = 1; month <= 12; month++)
        {
            var lastColumn = firstColumn + _personsToBeCompared.Count + 1; // + 1 because of DAY | WEEKDAY | PERSON1 | PERSON2 | ...
            var daysInMonth = DateTime.DaysInMonth(_comparingTable.Year, month);
            
            AddMonthNameHeader(firstColumn, lastColumn, month);
            AddPersonColumnHeader(firstColumn + 2);
            
            for (var day = 1; day <= daysInMonth; day++)
            {
                AddDayOfMonthColumn(firstColumn, day);
                AddWeekdayColumns(firstColumn + 1, month, day);
                AddPersonColumns(firstColumn + 2, day);
            }
            AdjustColumnWidth(firstColumn, lastColumn);
            firstColumn = lastColumn + 1;
        }
    }

    private void AddMonthNameHeader(int firstColumn, int lastColumn, int month)
    {
        _worksheet.Range(FirstRow, firstColumn, FirstRow, lastColumn).Merge();
        var nameOfMonth = new DateTime(1996, month, 1).ToString("MMMM");
        _worksheet.Cell(FirstRow, firstColumn).Value = nameOfMonth;
        //MonthNameHeaderStyling(firstColumn, lastColumn, month);
    }

    private void MonthNameHeaderStyling(int firstColumn, int lastColumn, int month)
    {
        var currentCell = _worksheet.Range(FirstRow, firstColumn, FirstRow, lastColumn); 
        currentCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        currentCell.Style.Border.TopBorder = XLBorderStyleValues.Thick;
        currentCell.Style.Border.RightBorder = XLBorderStyleValues.Medium;
        switch (month)
        {
            case 1:
                currentCell.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
                break;
            case 12:
                currentCell.Style.Border.RightBorder = XLBorderStyleValues.Thick;
                break;
        }
    }
    
    private void AddPersonColumnHeader(int currentColumn)
    {
        foreach (var person in _personsToBeCompared)
        {
            var currentCell = _worksheet.Cell(FirstRow + 1, currentColumn + _personsToBeCompared.IndexOf(person));
            currentCell.Value = person.TableName;
        }
    }
    
    private void AddDayOfMonthColumn(int firstColumn, int day)
    {
        _worksheet.Cell(FirstRow + 1 + day, firstColumn).Value = day;
        _worksheet.Cell(FirstRow + 1 + day, firstColumn).Style.Alignment.Horizontal =
            XLAlignmentHorizontalValues.Center;
    }

    private void AddWeekdayColumns(int secondColumn, int month, int day)
    {
        var dayOfWeek = new DateTime(_comparingTable.Year, month, day).DayOfWeek;
        _worksheet.Cell(FirstRow + 1 + day, secondColumn).Value = dayOfWeek.ToString()[..3];
        
    }
    
    private void AddPersonColumns(int thirdColumn, int day)
    {
        for (var personIndex = 0; personIndex < _comparingTable.PersonsToBeCompared.Count; personIndex++)
        {
            var currentColumn = thirdColumn + personIndex;
            var shiftpattern = _listOfShiftpattern[personIndex];
            _worksheet.Cell(FirstRow + 1 + day, currentColumn).Value = shiftpattern[_personShiftpatternIterators[personIndex]];
            _worksheet.Cell(FirstRow + 1 + day, currentColumn).Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;
            _personShiftpatternIterators[personIndex]++;
            if (_personShiftpatternIterators[personIndex] == shiftpattern.Count) 
                _personShiftpatternIterators[personIndex] = 0;
        }
    }

    private void AdjustColumnWidth(int firstColumn, int lastColumn)
    {
        _worksheet.Columns(firstColumn, lastColumn).AdjustToContents();
    }
    
    private void SaveWorkbook(XLWorkbook workbook)
    {
        workbook.SaveAs(Path.Combine(_folderPath, $"{_worksheetName}.xlsx"));
    }

    private void TableStyling()
    {
        var lastColumn = (2 + _personsToBeCompared.Count) * 12 + FirstColumn - 1; // -1 because excel column count starts with 1 instead of 0
        var lastRow = FirstRow + 32;
        InsideBorderStyling(lastRow);
        OutsideBorderStyling(lastRow, lastColumn);
        
    }
    
    private void OutsideBorderStyling(int lastRow, int lastColumn)
    {
        var firstRowLength = _worksheet.Range(FirstRow,
            FirstColumn,
            FirstRow,
            lastColumn);
        var firstColumnLength = _worksheet.Range(FirstRow,
            FirstColumn,
            lastRow,
            FirstColumn);
        var lastRowLength = _worksheet.Range(lastRow,
            FirstColumn,
            lastRow,
            lastColumn);
        var lastColumnLength = _worksheet.Range(FirstRow,
            lastColumn,
            lastRow,
            lastColumn);
        
        firstRowLength.Style.Border.TopBorder = XLBorderStyleValues.Thick;
        firstRowLength.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
        firstColumnLength.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
        lastRowLength.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
        lastColumnLength.Style.Border.RightBorder = XLBorderStyleValues.Thick;
    }

    private void InsideBorderStyling(int lastRow)
    {
        var displaceFactor = 2 + _personsToBeCompared.Count;
        MonthColumnStyling(lastRow, displaceFactor);
    }

    private void MonthColumnStyling(int lastRow, int displaceFactor)
    {
        var currentColumn = FirstColumn;
        for (var month = 1; month <= 12; month++)
        {
            for (var row = FirstRow; row <= lastRow; row++)
            {
                _worksheet.Cell(row, currentColumn + displaceFactor).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                _worksheet.Cell(row, currentColumn + displaceFactor - _personsToBeCompared.Count).Style.Border.LeftBorder =
                    XLBorderStyleValues.Thin;
                MonthDayBottomBorder(row, currentColumn);
                
            }
            currentColumn += displaceFactor;
        }
    }
    
    private void MonthDayBottomBorder(int row, int secondColumn)
    {
        var sunday = new DateTime(2024, 2, 25).DayOfWeek.ToString()[..3];
        if (row > FirstRow + 2)
        {
            if (_worksheet.Cell(row, secondColumn).Value.ToString() == sunday)
            {
                _worksheet.Range(row,
                        secondColumn - 1,
                        row,
                        secondColumn + _personsToBeCompared.Count)
                    .Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            }
            else
            {
                _worksheet.Range(row,
                        secondColumn - 1,
                        row,
                        secondColumn + _personsToBeCompared.Count)
                    .Style.Border.BottomBorder = XLBorderStyleValues.DashDot;
            }
            
        }
    }
    
    private void FillColoring(int row, int displaceFactor)
    {
        
    }
    

}