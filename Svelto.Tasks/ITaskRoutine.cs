using System;
using System.Collections;

namespace Svelto.Tasks
{
    public interface ITaskRoutine<T> where T:IEnumerator
    {
        ITaskRoutine<T> SetEnumeratorProvider(Func<T> taskGenerator);
        ITaskRoutine<T> SetEnumerator(T taskGenerator);
        ITaskRoutine<T> SetScheduler(IRunner runner);

        ContinuationWrapper Start(Action<PausableTaskException> onFail = null, Action onStop = null);
        ContinuationWrapper ThreadSafeStart(Action<PausableTaskException> onFail = null, Action onStop = null);
     
        void Pause();
        void Resume();
        void Stop();
    }
}
