// See https://aka.ms/new-console-template for more information
using ExcelCalculator;
using ExcelLibrary.DataAccess;


var comparingtable = ComparingTableDataAccess.GetAllComparingTables()[0];
comparingtable.Year = 2024;
var folderPathMac = "/Users/sebastianheyde/Documents/privat/programming/testData";
var folderPathWindows = "D:\\TestData";

ExcelService excelCreator = new ExcelService(comparingtable, "TestWorksheet", folderPathMac);
excelCreator.CreateComparingTableInCsvFile();
