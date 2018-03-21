using System.Collections;

namespace Svelto.Tasks.Experimental
{
    public class SerialTaskCollectionWithToken<Token> : SerialTaskCollectionWithToken<IEnumerator, Token>
    {
        public SerialTaskCollectionWithToken(Token token) : base(token)
        {}
    }
    
    public class SerialTaskCollectionWithToken<T, Token> : SerialTaskCollection<T> where T:IEnumerator
    {
        public SerialTaskCollectionWithToken(Token token) { _token = token; }

        protected override void CheckForToken(object current)
        {
            var task = current as IChainLink<Token>;
            if (task != null)
                task.Current = _token;
        }

        readonly Token _token;
    }
}
