using System;
using System.Collections;

namespace Svelto.Tasks.Experimental
{
    public class ParallelTaskCollectionWithToken<Token> : ParallelTaskCollection
    {
        public ParallelTaskCollectionWithToken(Token token) { _token = token; }

        protected override void CheckForToken(object current)
        {
            var task = current as IChainLink<Token>;
            if (task != null)
                task.Current = _token;
        }

        readonly Token _token;
    }
}
