using Microsoft.Data.SqlClient;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

internal class OnChangeAction : IAction
{
    public async Task<bool> Run(SqlConnectionStringBuilder sqlStringBuilder)
    {
        var connectionString = sqlStringBuilder.ToString();

        using (var tableDependency = new SqlTableDependency<Customers>(connectionString))
        {
            tableDependency.OnChanged += TableDependency_Changed;
            tableDependency.Start();

            Console.WriteLine("Waiting for receiving notifications...");
            Console.WriteLine("Press a key to stop");
            Console.ReadKey();
        }

        void TableDependency_Changed(object sender, RecordChangedEventArgs<Customers> e)
        {
            if (e.ChangeType != ChangeType.None)
            {
                var changedEntity = e.Entity;
                Console.WriteLine("DML operation: " + e.ChangeType);
                Console.WriteLine("ID: " + changedEntity.Id);
                Console.WriteLine("Name: " + changedEntity.Name);
                Console.WriteLine("Surname: " + changedEntity.Surname);
            }
        }

        return await Task.FromResult(true);
    }
}
