namespace TsSoft.Database.Migrate
{
    public interface IMigrationManager<M>
    {
        bool IsApplicable(M migrationRule);

        void Apply(M migrationRule);
    }
}