using System.Linq;
using System.Text.RegularExpressions;

namespace SpExecSqlConverter
{
  /// <summary>
  /// Конвертер для преобразования параметризированных sql-запросов в простые запросы без параметров.
  /// </summary>
  internal class PlainSqlConverter
  {
    private static readonly Regex ExecSqlPattern = new Regex(
      @"\s*(?:exec|execute)\s+sp_executesql\s+N'((?:''|[^'])*)'\s*,\s*N'((?:''|[^'])*)'\s*,\s*(.*)",
      RegexOptions.Compiled | RegexOptions.Singleline);

    /// <summary>
    /// Преобразовать вызов sp_execsql в простой sql-запрос без параметров.
    /// </summary>
    /// <param name="query">Исходный параметризированный SQL-запрос.</param>
    /// <returns>Преобразованный запрос.</returns>
    public string ConvertFromSpExecSql(string query)
    {
      var m = ExecSqlPattern.Match(query);
      if (!m.Success)
        return query;

      var sqlText = m.Groups[1].Value.Replace("''", "'");
      var paramSpec = m.Groups[2].Value;
      var paramValues = m.Groups[3].Value.Split(',').Select(p => p.Trim().Split('=').ToArray()).ToDictionary(p => p[0], p => p[1]);
      foreach (var pName in paramValues.Keys)
      {
        var pValue = paramValues[pName];
        sqlText = Regex.Replace(sqlText, $@"{pName}\b", pValue);
      }
      return sqlText;
    }
  }
}
