using Microsoft.Data.SqlClient;

internal class SqlBrokerQueueListenerAction : IAction
{
    public async Task<bool> Run(SqlConnectionStringBuilder sqlStringBuilder)
    {
        sqlStringBuilder.TrustServerCertificate = true;
        sqlStringBuilder.MultipleActiveResultSets = true; // Enable MARS to allow multiple readers.
        var connectionString = sqlStringBuilder.ToString();

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        while (true)
        {
            Console.WriteLine("Waiting...");

            using (var command = new SqlCommand("WAITFOR (RECEIVE cast(message_body as nvarchar(max)) as payload, * from [OrderQueue])", connection))
            {
                command.CommandTimeout = 0;

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var payload = reader["payload"];
                        Console.WriteLine("Received payload: " + payload);
                    }
                }
            }
        }
    }
}
