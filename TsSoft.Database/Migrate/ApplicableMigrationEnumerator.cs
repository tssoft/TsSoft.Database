namespace TsSoft.Database.Migrate
{
    using System.Collections;
    using System.Collections.Generic;

    internal class ApplicableMigrationEnumerator<M> : IEnumerator<M> where M : class
    {
        private ISet<M> migrationRules;
        private IMigrationManager<M> migrationManager;
        private M currentRule;

        public ApplicableMigrationEnumerator(ISet<M> migrationRules, IMigrationManager<M> migrationManager)
        {
            this.migrationRules = migrationRules;
            this.migrationManager = migrationManager;
        }

        public bool MoveNext()
        {
            currentRule = null;
            var enumerator = migrationRules.GetEnumerator();
            while (currentRule == null && enumerator.MoveNext())
            {
                var migrationRule = enumerator.Current;
                if (migrationManager.IsApplicable(migrationRule))
                {
                    currentRule = migrationRule;
                    migrationRules.Remove(migrationRule);
                }
            }
            return currentRule != null;
        }

        public M Current
        {
            get { return currentRule; }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public void Dispose()
        {
        }

        public void Reset()
        {
        }
    }
}