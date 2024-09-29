// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;

using Cfc.DevOps.PlSql.Antlr.Generated;
using Cfc.DevOps.PlSql.Application;

var fname = @"C:\Users\claud\Documents\__Other__\plsql-antlr-tool\001-D111111-LCO-EMG_ENF---EMG-COM_MGR_HO-LCOEMGM.sql";
var sql = File.ReadAllText(fname);
var stream = new AntlrInputStream(sql);
var lexer = new PlSqlLexer(stream);
var tokens = new CommonTokenStream(lexer);
var parser = new PlSqlParser(tokens);

var root = parser.sql_script();

var alterTableVisitor = new AlterSessionVisitor();
var createTableVisitor = new CreateTableVisitor();

alterTableVisitor.Visit(root);
createTableVisitor.Visit(root);

var settings = alterTableVisitor.AlterSessionSettings;
foreach (var setting in settings)
{
    Console.WriteLine($"Location: {setting.StartLocation.Line}:{setting.StartLocation.Column} - {setting.StopLocation.Line}:{setting.StopLocation.Column}");
    Console.WriteLine(sql.Substring(setting.StartLocation.Index, setting.StopLocation.Index - setting.StartLocation.Index + 1));
    foreach (var kvp in setting.Settings)
    {
        Console.WriteLine($"  {kvp.Key} = {kvp.Value}");
    }
}

var createTables = createTableVisitor.CreateTables;
foreach (var createTable in createTables)
{
    Console.WriteLine($"Location: {createTable.StartLocation.Line}:{createTable.StartLocation.Column} - {createTable.StopLocation.Line}:{createTable.StopLocation.Column}");
    Console.WriteLine(sql.Substring(createTable.StartLocation.Index, createTable.StopLocation.Index - createTable.StartLocation.Index + 1));
    Console.WriteLine($"  Table Schema: {createTable.SchemaName ?? "undefined"}");
    Console.WriteLine($"  Table Name: {createTable.TableName}");
}



Console.WriteLine("Hello Antlr!");
