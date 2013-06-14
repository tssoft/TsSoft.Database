namespace TsSoft.Database.Migrate
{
    using System.Collections.Generic;

    public class MigrationRunner<M> where M : class
    {
        private IMigrationManager<M> migrationManager;

        public MigrationRunner(IMigrationManager<M> migrationManager)
        {
            this.migrationManager = migrationManager;
        }

        public void Migrate(IEnumerable<M> migrationRules)
        {
            var migrationEnumerator = new ApplicableMigrationEnumerator<M>(
                new HashSet<M>(migrationRules), migrationManager);
            while (migrationEnumerator.MoveNext())
            {
                migrationManager.Apply(migrationEnumerator.Current);
            }
        }
    }
}