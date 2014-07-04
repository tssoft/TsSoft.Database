namespace TsSoft.Database.SqlServer.Migrate
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using Database.Migrate;

    public abstract class MsSqlMigrationManager : IMigrationManager<IMigrationRule>
    {
        private readonly SqlConnection connection;

        protected MsSqlMigrationManager(SqlConnection connection)
        {
            this.connection = connection;
        }

        public abstract bool IsApplicable(IMigrationRule migrationRule);

        public void Apply(IMigrationRule migrationRule)
        {
            using (var command = connection.CreateCommand())
            {
                IEnumerator<string> queryEnumerator = new ScriptQueryEnumerator(migrationRule.GetScript());
                while (queryEnumerator.MoveNext())
                {
                    command.CommandText = queryEnumerator.Current;
                    command.CommandTimeout = connection.ConnectionTimeout;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}