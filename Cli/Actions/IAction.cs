using Microsoft.Data.SqlClient;

public interface IAction
{
    Task<bool> Run(SqlConnectionStringBuilder sqlStringBuilder);
}
