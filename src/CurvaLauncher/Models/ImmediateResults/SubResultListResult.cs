using System.Collections.Generic;

namespace CurvaLauncher.Models.ImmediateResults
{
    public class SubResultListResult : ImmediateResult
    {
        public IReadOnlyList<IQueryResult> Results { get; }

        public SubResultListResult(IReadOnlyList<IQueryResult> results)
        {
            Results = results;
        }
    }
}
