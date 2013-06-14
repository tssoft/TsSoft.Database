namespace TsSoft.Database.SqlServer.Migrate
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class ScriptQueryEnumerator : IEnumerator<string>
    {
        private Regex nonWhiteRegex;
        private string currentQuery;
        private Queue<string> queryQueue;

        public ScriptQueryEnumerator(string script)
        {
            nonWhiteRegex = new Regex(@"\S+",
                RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Multiline);
            Regex goRegex = new Regex(@"^\s*(g|G)(o|O)\s*$",
                RegexOptions.ExplicitCapture | RegexOptions.Compiled | RegexOptions.Multiline);
            var queries = goRegex.Split(script);
            queryQueue = new Queue<string>(queries.ToList());
        }

        public string Current
        {
            get { return currentQuery; }
        }

        public bool MoveNext()
        {
            currentQuery = string.Empty;
            while (!nonWhiteRegex.IsMatch(currentQuery) && queryQueue.Count > 0)
            {
                currentQuery = queryQueue.Dequeue();
            }
            return nonWhiteRegex.IsMatch(currentQuery);
        }

        public void Reset()
        {
        }

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }
    }
}