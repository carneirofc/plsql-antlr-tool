# Development

Install pip package and download parser and lexer

```bash
cd .\tools
python -m venv .venv
.\.venv\Scripts\Activate.ps1
pip install antlr4-tools
```

Generate the the parser

```bash
antlr4 -encoding utf8 -Werror -package Cfc.DevOps.PlSql.Antlr.Generated -o ..\Cfc.DevOps.PlSql.Antlr\Generated\ -Dlanguage=CSharp -visitor .\PlSqlParser.g4 .\PlSqlLexer.g4
```

# References

- https://www.antlr.org/
- https://github.com/antlr/grammars-v4/tree/master/sql/plsql
- https://tomassetti.me/antlr-mega-tutorial/#chapter11
- https://tomassetti.me/getting-started-with-antlr-in-csharp/
- https://tomassetti.me/best-practices-for-antlr-parsers/
