using Antlr4.Runtime.Misc;
using Cfc.DevOps.PlSql.Antlr.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cfc.DevOps.PlSql.Application
{

    public class PlSqlVisitor : PlSqlParserBaseVisitor<object>
    {
        public List<PlSqlParser.Select_statementContext> select_StatementContexts = new List<PlSqlParser.Select_statementContext>();
        public override object VisitSelect_statement([NotNull] PlSqlParser.Select_statementContext context)
        {
            select_StatementContexts.Add(context);
            return base.VisitSelect_statement(context);
        }
    }
}
