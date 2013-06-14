namespace TsSoft.Database.SqlServer.Migrate
{
    public interface IMigrationRule
    {
        string GetScript();
    }
}