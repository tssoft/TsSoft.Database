namespace TsSoft.Database.SqlServer.Migrate.Custom
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class CustomMigrationRuleFactory
    {
        public IEnumerable<IMigrationRule> FindMigrationRules(string scriptDirectory)
        {
            var scriptFiles = Directory.GetFiles(scriptDirectory, "*.sql");
            Array.Sort(scriptFiles, StringComparer.InvariantCulture);
            ICollection<IMigrationRule> migrationRules = new List<IMigrationRule>();
            foreach (var scriptFile in scriptFiles)
            {
                migrationRules.Add(new CustomMsSqlMigrationRule(scriptFile));
            }
            return migrationRules;
        }
    }
}