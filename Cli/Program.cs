using System.Reflection;
using Microsoft.Data.SqlClient;

var connectionString = "Server=172.16.16.40\\SQL2019;Database=bulktest;User Id=bulk;Password=bulk;";
SqlConnectionStringBuilder connectionStringBuilder = new(connectionString);

var actionName = $"{Environment.GetCommandLineArgs()[1]}Action";
Console.WriteLine($"ActionName: {actionName} - Try to run()");

try
{
    var actionType = Assembly.GetExecutingAssembly().GetType(actionName);

    if (actionType == null)
    {
        Console.WriteLine($"Action '{actionName}' not found.");

        return;
    }

    var action = (IAction)Activator.CreateInstance(actionType);
    var result = await action.Run(connectionStringBuilder);

    Console.WriteLine($"Action '{actionName}' executed with result: {result}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error executing action: {ex.Message}");
}
