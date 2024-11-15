using System.Data;
using Microsoft.Data.SqlClient;

internal class BulkUpsertAction : IAction
{
    public async Task<bool> Run(SqlConnectionStringBuilder sqlStringBuilder)
    {
        sqlStringBuilder.TrustServerCertificate = true;

        var dataTable = new DataTable();
        dataTable.Columns.Add("id", typeof(int));
        dataTable.Columns.Add("number", typeof(int));
        var rnd = new Random();

        for (var i = 0; i < 100; i++)
        {
            var dataRow = dataTable.NewRow();
            dataRow["id"] = i;
            dataRow["number"] = i * rnd.Next(minValue: 0, i);
            dataTable.Rows.Add(dataRow);
        }

        using var bulkCopy = new SqlBulkCopy(sqlStringBuilder.ToString(), SqlBulkCopyOptions.FireTriggers);
        bulkCopy.SqlRowsCopied += (sender, args) => Console.WriteLine($"Row: {args.RowsCopied}");
        bulkCopy.NotifyAfter = 1000;
        bulkCopy.BatchSize = 1000;
        bulkCopy.DestinationTableName = "bulktest";
        bulkCopy.WriteToServer(dataTable);

        return await Task.FromResult(true);
    }
}
