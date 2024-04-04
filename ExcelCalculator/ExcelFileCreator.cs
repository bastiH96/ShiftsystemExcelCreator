using ClosedXML.Excel;
using ExcelLibrary.Models;

namespace ExcelCalculator; 
public class ExcelFileCreator {

    private readonly string _worksheetName;
    private string _folderPath;
    private readonly ComparingTableModel _comparingTable;

    public ExcelFileCreator(string worksheetName, string folderPath, ComparingTableModel comparingTable)
    {
        this._worksheetName = worksheetName;
        this._folderPath = folderPath;
        this._comparingTable = comparingTable;
    }

    public void CreateExcelFile() {
        using (var workbook = new XLWorkbook()) {
            var worksheet = workbook.Worksheets.Add(_worksheetName);
        }
    }

    private void CreateTableContent(IXLWorksheet worksheet) {
        int startcolumn = 1;
        int displacementFactor = _comparingTable.PersonsToBeCompared.Count + 1;
        int lastcolumn = startcolumn + displacementFactor;

        for(var month = 1; month <= 12; month++) {

        }
    }

    private void ConfigureMonth(int monthStartColumn, int monthLastColumn, int currentMonth) {

    }
}
