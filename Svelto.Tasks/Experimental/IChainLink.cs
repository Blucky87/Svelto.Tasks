using System.Collections.Generic;

namespace Svelto.Tasks.Experimental
{
    public interface IChainLink<Token>: IEnumerator<Token>
    {
        Token Current { set; }
    }
}
