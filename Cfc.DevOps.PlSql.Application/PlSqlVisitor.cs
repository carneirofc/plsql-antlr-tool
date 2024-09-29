using Antlr4.Runtime.Misc;
using Cfc.DevOps.PlSql.Antlr.Generated;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfc.DevOps.PlSql.Application
{
    public enum Type
    {
        Unknown,
        STRING,
        DICTIONARY
    }
    public class PlSqlVisit
    {
        public required Type Type { get; set; }
        public object? RawValue { get; set; }
        public T? Value<T>()
        {
            if (RawValue == null)
            {
                return default;
            }
            return (T)RawValue;
        }
    }

    public class Location
    {
        public required int Index { get; set; }
        public required int Line { get; set; }
        public required int Column { get; set; }
    }
    public class AlterSessionSettings
    {
        public required Location StartLocation { get; set; }
        public required Location StopLocation { get; set; }
        public required Dictionary<string, string> Settings { get; set; }
    }

    public class AlterSessionVisitor : PlSqlParserBaseVisitor<PlSqlVisit>
    {
        public List<AlterSessionSettings> AlterSessionSettings = new List<AlterSessionSettings>();

        public override PlSqlVisit VisitAlter_session([NotNull] PlSqlParser.Alter_sessionContext context)
        {
            var set_clause = context.alter_session_set_clause();
            if (set_clause != null)
            {
                var alterSessionValues = Visit(set_clause);
                if (alterSessionValues.Type == Type.DICTIONARY)
                {
                    var dict = alterSessionValues.Value<Dictionary<string, string>>();
                    if (dict == null)
                    {
                        throw new Exception("Alter session dictionary is null");
                    }

                    AlterSessionSettings.Add(new AlterSessionSettings()
                    {
                        StartLocation = new Location()
                        {
                            Index = context.Start.StartIndex,
                            Line = context.Start.Line,
                            Column = context.Start.Column
                        },
                        StopLocation = new Location()
                        {
                            Index = context.Stop.StopIndex,
                            Line = context.Stop.Line,
                            Column = context.Stop.Column
                        },
                        Settings = dict
                    });
                }
            }
            return base.VisitAlter_session(context);
        }

        public override PlSqlVisit VisitAlter_session_set_clause([NotNull] PlSqlParser.Alter_session_set_clauseContext context)
        {
            var dict = new Dictionary<string, string>();
            var p_names = context.parameter_name();
            var p_values = context.parameter_value();

            for (int i = 0; i < p_names.Length; i++)
            {
                var name = Visit(p_names[i])?.Value<string>();
                var value = Visit(p_values[i])?.Value<string>();
                if (name == null || value == null)
                {
                    continue;
                }
                dict.Add(name, value);
            }
            return new PlSqlVisit { Type = Type.DICTIONARY, RawValue = dict };
        }

        public override PlSqlVisit VisitParameter_name([NotNull] PlSqlParser.Parameter_nameContext context)
        {
            return new PlSqlVisit { Type = Type.STRING, RawValue = context.GetText() };
        }

        public override PlSqlVisit VisitParameter_value([NotNull] PlSqlParser.Parameter_valueContext context)
        {
            return new PlSqlVisit { Type = Type.STRING, RawValue = context.GetText() };
        }
    }

    public sealed class CreateTable
    {
        public string? SchemaName { get; set; }
        public string TableName { get; set; }
        public required Location StartLocation { get; set; }
        public required Location StopLocation { get; set; }
    }

    public class CreateTableVisitor : PlSqlParserBaseVisitor<PlSqlVisit>
    {
        public List<CreateTable> CreateTables = new();
        public override PlSqlVisit VisitCreate_table([NotNull] PlSqlParser.Create_tableContext context)
        {
            var createTable = new CreateTable()
            {
                StartLocation = new Location()
                {
                    Index = context.Start.StartIndex,
                    Line = context.Start.Line,
                    Column = context.Start.Column
                },
                StopLocation = new Location()
                {
                    Index = context.Stop.StopIndex,
                    Line = context.Stop.Line,
                    Column = context.Stop.Column
                }
            };

            Console.WriteLine("Create table found");
            if (context.schema_name() != null)
            {
                var schemaName = Visit(context.schema_name());
                createTable.SchemaName = schemaName.Value<string>();
            }
            if (context.table_name() != null)
            {
                var tableName = Visit(context.table_name());
                createTable.TableName = tableName.Value<string>() ?? throw new Exception("Table name is null");
            }
            CreateTables.Add(createTable);

            return base.VisitCreate_table(context);
        }

        public override PlSqlVisit VisitTable_name([NotNull] PlSqlParser.Table_nameContext context)
        {
            return new PlSqlVisit { Type = Type.STRING, RawValue = context.GetText() };
        }
        public override PlSqlVisit VisitSchema_name([NotNull] PlSqlParser.Schema_nameContext context)
        {
            return new PlSqlVisit { Type = Type.STRING, RawValue = context.GetText() };
        }
    }
}
