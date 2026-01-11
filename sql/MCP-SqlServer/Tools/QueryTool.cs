using System.Data;
using Server.Utils;
using System.Text.Json;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using ModelContextProtocol.Server;

namespace Server.Tools;

/// <summary>
/// Provides static methods for performing diagnostic and query operations on a Microsoft SQL Server database,
/// including health checks, schema inspection, and executing SELECT queries with JSON-formatted results.
/// </summary>
/// <remarks>The QueryTool class is intended for use in server-side tools and utilities that require
/// direct interaction with a SQL Server database. All methods are static and operate independently, opening new
/// database connections for each call. The class is designed for diagnostic, monitoring, and ad-hoc querying
/// scenarios, and is not optimized for high-frequency or concurrent use. Error information is returned in the
/// output of each method rather than through exceptions.</remarks>
[McpServerToolType]
public static class QueryTool
{
    /// <summary>
    /// Checks the status of the Microsoft SQL Server database connection and returns a message indicating whether
    /// the connection is healthy.
    /// </summary>
    /// <remarks>This method attempts to open a connection to the configured SQL Server database. It
    /// does not throw exceptions; any connection errors are reported in the returned message. This method is
    /// intended for diagnostic or monitoring purposes.</remarks>
    /// <returns>A string message indicating the result of the health check. Returns "Connection is OK" if the connection is
    /// successful; otherwise, returns "Connection failed: <error message>" describing the failure.</returns>
    [McpServerTool, Description("Tests if the Microsoft SQL Server Database connection is good and alive.")]
    public static string HealthCheck()
    {
        FileLogger.Log("Called HealthCheck()");

        try
        {
            using var connection = new SqlConnection(Configuration.GetConnectionString());
            connection.Open();

            return "Connection is OK";
        }
        catch (Exception e)
        {
            return $"Connection failed: {e.Message}";
        }
    }

    /// <summary>
    /// Retrieves a list of all tables in the SQL Server database, including their schemas, columns, and data types,
    /// formatted as a JSON string.
    /// </summary>
    /// <remarks>The returned JSON object maps each table name to a list of its columns, where each
    /// column entry includes the schema, column name, and data type. This method is intended for use with SQL
    /// Server databases and does not include views or other non-table objects.</remarks>
    /// <returns>A JSON string containing the schema, table names, column names, and data types for all base tables in the
    /// database. If an error occurs, the JSON string will contain an "error" property with the error message.</returns>
    [McpServerTool(), Description("Get a list of all tables with their respective schema, columns and types (SQL SERVER).")]
    public static string GetSchema()
    {
        FileLogger.Log("Called GetSchema()");

        try
        {
            using var connection = new SqlConnection(Configuration.GetConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            using var command = new SqlCommand(@"
                    SELECT 
                        t.table_schema,
                        t.table_name, 
                        c.column_name, 
                        c.data_type 
                    FROM 
                        information_schema.tables t
                    JOIN 
                        information_schema.columns c 
                        ON t.table_name = c.table_name
                        AND t.table_schema = c.table_schema
                    WHERE 
                        t.table_type = 'BASE TABLE';",
                connection, 
                transaction
            );

            using var reader = command.ExecuteReader();

            var tables = new Dictionary<string, List<Dictionary<string, string>>>();

            while (reader.Read())
            {
                var schemaName = reader.GetString(0);
                var tableName = reader.GetString(1);
                var columnName = reader.GetString(2);
                var columnType = reader.GetString(3);

                if (!tables.ContainsKey(tableName))
                    tables[tableName] = new List<Dictionary<string, string>>();

                tables[tableName].Add(new Dictionary<string, string>
                {
                    { "schema", schemaName },
                    { "name", columnName },
                    { "type", columnType }
                });
            }

            return JsonSerializer.Serialize(tables, new JsonSerializerOptions { WriteIndented = false });
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Executes the specified SQL query against the Microsoft SQL Server database and returns the result as a
    /// JSON-formatted string.
    /// </summary>
    /// <remarks>The method opens a new database connection for each call and executes the query
    /// within a transaction using the ReadCommitted isolation level. Only SELECT queries are supported; other types
    /// of queries may result in errors. The output JSON will contain null values for database NULLs. This method is
    /// not thread-safe and should not be used for high-frequency or long-running queries without additional error
    /// handling and resource management.</remarks>
    /// <param name="query">The SQL query to execute. This should be a valid SELECT statement. If the query is invalid or causes an
    /// error, an error message will be returned in the JSON output.</param>
    /// <returns>A JSON-formatted string representing the result set of the query. If the query executes successfully, the
    /// result is an array of objects, each corresponding to a row in the result set. If an error occurs, the JSON
    /// contains an 'error' property with the error message.</returns>
    [McpServerTool, Description("Execute a query into the Microsoft SQL Server database and return the result as a JSON")]
    public static string Query(string query)
    {
        FileLogger.Log($"Called Query({query})");

        try
        {
            using var connection = new SqlConnection(Configuration.GetConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            using var command = new SqlCommand(query, connection, transaction);
            using var reader = command.ExecuteReader();

            var dataTable = new DataTable();
            dataTable.Load(reader);

            var rows = new List<Dictionary<string, object>>();
            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column] == DBNull.Value ? null : row[column];
                }
                rows.Add(dict);
            }

            transaction.Commit();

            return JsonSerializer.Serialize(rows, new JsonSerializerOptions { WriteIndented = false });

        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { error = ex.Message });
        }
    }
}
