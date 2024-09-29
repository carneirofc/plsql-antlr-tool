// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;

using Cfc.DevOps.PlSql.Antlr.Generated;
using Cfc.DevOps.PlSql.Application;



var sql = @"SELECT * FROM MINHA_TABELA;";
var stream = new AntlrInputStream(sql);
var lexer = new PlSqlLexer(stream);
var tokens = new CommonTokenStream(lexer);
var parser = new PlSqlParser(tokens);

var root = parser.sql_script();

var visitor = new PlSqlVisitor();

visitor.Visit(root);

foreach (var select in visitor.select_StatementContexts)
{
    Console.WriteLine(select.GetText());
}


Console.WriteLine("Hello Antlr!");
