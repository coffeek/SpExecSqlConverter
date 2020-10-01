using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace SpExecSqlConverter
{
  internal class FormatSqlException : Exception
  {
    public IList<ParseError> ParseErrors { get; set; }
  }

  internal class SqlFormatter
  {
    private readonly SqlScriptGenerator generator;
    private readonly TSqlParser parser;

    public string Format(string query)
    {
      var parsedQuery = this.parser.Parse(new StringReader(query), out var errors);
      if (errors.Count > 0)
        throw new FormatSqlException { ParseErrors = errors };
      this.generator.GenerateScript(parsedQuery, out var formattedQuery);
      return formattedQuery;
    }

    public SqlFormatter()
    {
      this.parser = new TSql150Parser(false);
      this.generator = new Sql150ScriptGenerator(new SqlScriptGeneratorOptions
      {
        KeywordCasing = KeywordCasing.Lowercase,
        IncludeSemicolons = true,
        NewLineBeforeFromClause = true,
        NewLineBeforeOrderByClause = true,
        NewLineBeforeWhereClause = true,
        AlignClauseBodies = false,
        IndentationSize = 2
      });
    }
  }
}
