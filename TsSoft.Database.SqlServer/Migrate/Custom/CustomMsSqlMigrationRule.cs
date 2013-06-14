namespace TsSoft.Database.SqlServer.Migrate.Custom
{
    using System.IO;
    using System.Text;

    public class CustomMsSqlMigrationRule : IMigrationRule
    {
        private string scriptFile;

        public CustomMsSqlMigrationRule(string scriptFile)
        {
            this.scriptFile = scriptFile;
        }

        public string Id
        {
            get { return Path.GetFileNameWithoutExtension(scriptFile); }
        }

        /// <summary>
        /// Version of database that would be set after migration been applied
        /// </summary>
        public int DatabaseVersion
        {
            get { return -1; }
        }

        public string GetScript()
        {
            string script;
            using (StreamReader streamReader = new StreamReader(scriptFile, Encoding.UTF8))
            {
                script = streamReader.ReadToEnd();
            }
            return script;
        }
    }
}