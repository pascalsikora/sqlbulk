// See https://aka.ms/new-console-template for more information
using System.Data;
using Microsoft.Data.SqlClient;

Console.WriteLine("Hello, World!");
var connectionString = "Server=.;Database=test1;User Id=sa;Password=sa;Trusted_Connection=True;";
SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);
connectionStringBuilder.TrustServerCertificate = true;

var dataTable = new DataTable();
dataTable.Columns.Add("id",typeof(int));
dataTable.Columns.Add("number",typeof(int));
Random rnd = new Random();
for (int i = 0; i < 100000; i++)
{
    var dataRow = dataTable.NewRow();
    dataRow["id"] = i;
    dataRow["number"] = i*rnd.Next(0,i);
    dataTable.Rows.Add(dataRow);
}

using SqlBulkCopy bulkCopy = new SqlBulkCopy(connectionStringBuilder.ToString(),SqlBulkCopyOptions.FireTriggers);
bulkCopy.SqlRowsCopied+= (sender,args) => Console.WriteLine($"Row: {args.RowsCopied}");
bulkCopy.NotifyAfter = 1000;
bulkCopy.BatchSize = 1000;
bulkCopy.DestinationTableName = "bulktest";
bulkCopy.WriteToServer(dataTable);
