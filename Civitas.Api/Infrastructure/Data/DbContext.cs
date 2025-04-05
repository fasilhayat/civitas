using Npgsql;

namespace Civitas.Api.Infrastructure.Data;

using System.Data;

/// <summary>
/// A simple database context class to handle database connections and queries.
/// </summary>
public class DbContext : IDisposable
{
    /// <summary>
    /// The connection string used to connect to the database.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// The active database connection. Can be null if no connection is currently open.
    /// </summary>
    private NpgsqlConnection? _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbContext"/> class with the specified connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <exception cref="ArgumentNullException">Thrown if the connection string is null.</exception>
    public DbContext(string? connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    /// Executes a database function and returns the results as a <see cref="DataTable"/> asynchronously.
    /// </summary>
    /// <param name="functionName">The name of the database function to execute.</param>
    /// <param name="parameters">The parameters to pass to the function.</param>
    /// <returns>A <see cref="DataTable"/> containing the results of the function.</returns>
    /// <exception cref="ArgumentException">Thrown if the function name is null or empty.</exception>
    public async Task<DataTable> ExecuteFunctionAsync(string functionName, params NpgsqlParameter[] parameters)
    {
        if (string.IsNullOrWhiteSpace(functionName))
            throw new ArgumentException("Function name cannot be null or empty.", nameof(functionName));

        // Construct the SQL to call the function
        var sql = $"SELECT * FROM {functionName}";

        // Add parameters to the SQL query if provided
        if (parameters is { Length: > 0 })
        {
            var paramList = string.Join(", ", parameters.Select(p =>
            {
                if (p.Value is int or long or double or decimal or float)
                    return $"{p.ParameterName} => {p.Value}";

                var value = p.Value?.ToString()?.Replace("'", "''");
                return $"{p.ParameterName} => '{value}'";
            }));
            sql += $"({paramList})";
        }

        await OpenConnectionAsync();
        await using var command = new NpgsqlCommand(sql, _connection)
        {
            CommandType = CommandType.Text
        };

        if (parameters is { Length: > 0 }) command.Parameters.AddRange(parameters);

        using var adapter = new NpgsqlDataAdapter(command);
        var result = new DataTable();
        await Task.Run(() => adapter.Fill(result));
        CloseConnection();

        return result;
    }

    /// <summary>
    /// Executes a stored procedure and returns the results as a <see cref="DataTable"/> asynchronously.
    /// </summary>
    /// <param name="procedureName">The name of the stored procedure to execute.</param>
    /// <param name="parameters">The parameters to pass to the stored procedure.</param>
    /// <returns>A <see cref="DataTable"/> containing the results of the stored procedure.</returns>
    /// <exception cref="ArgumentException">Thrown if the procedure name is null or empty.</exception>
    public async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, params NpgsqlParameter[] parameters)
    {
        if (string.IsNullOrWhiteSpace(procedureName))
            throw new ArgumentException("Procedure name cannot be null or empty.", nameof(procedureName));

        await OpenConnectionAsync();
        await using var command = new NpgsqlCommand(procedureName, _connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        if (parameters is { Length: > 0 })
            command.Parameters.AddRange(parameters);

        using var adapter = new NpgsqlDataAdapter(command);
        var result = new DataTable();
        await Task.Run(() => adapter.Fill(result));

        CloseConnection();
        return result;
    }

    /// <summary>
    /// Executes a scalar database query and returns the result as a specified value type asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the scalar result.</typeparam>
    /// <param name="query">The query to execute.</param>
    /// <param name="parameters">The parameters to pass to the query.</param>
    /// <returns>The scalar result of the query.</returns>
    /// <exception cref="ArgumentException">Thrown if the query is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the query result is null.</exception>
    public async Task<T> ExecuteScalarFunctionAsync<T>(string query, params NpgsqlParameter[]? parameters) where T : struct
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Query cannot be null or empty.", nameof(query));

        await OpenConnectionAsync();

        await using var command = new NpgsqlCommand(query, _connection);
        if (parameters != null)
            command.Parameters.AddRange(parameters);

        var result = await command.ExecuteScalarAsync();

        CloseConnection();
        return (T)(result ?? throw new InvalidOperationException());
    }

    /// <summary>
    /// Releases the resources used by this instance of <see cref="DbContext"/>.
    /// </summary>
    public void Dispose()
    {
        CloseConnection();
        _connection?.Dispose();
    }

    /// <summary>
    /// Opens the database connection asynchronously if it is not already open.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    private async Task OpenConnectionAsync()
    {
        _connection ??= new NpgsqlConnection(_connectionString);

        if (_connection.State != ConnectionState.Open)
            await _connection.OpenAsync();
    }

    /// <summary>
    /// Closes the database connection if it is open.
    /// </summary>
    private void CloseConnection()
    {
        if (_connection?.State == ConnectionState.Open)
            _connection.Close();
    }
}
