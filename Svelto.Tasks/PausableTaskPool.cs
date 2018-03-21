using System.Collections;
using Svelto.DataStructures;

namespace Svelto.Tasks.Internal
{
    sealed class PausableTaskPool
    {
        public PausableTask<IEnumerator> RetrieveTaskFromPool()
        {
            PausableTask<IEnumerator> task;

            if (_pool.Dequeue(out task))
            {
                task.Reset();

                return task;
            }

            return CreateEmptyTask();
        }

        public void PushTaskBack(PausableTask<IEnumerator> task)
        {
            task.CleanUpOnRecycle(); //let's avoid leakings

            _pool.Enqueue(task);
        }

        PausableTask<IEnumerator> CreateEmptyTask()
        {
            return new PausableTask<IEnumerator>(this);
        }

        LockFreeQueue<PausableTask<IEnumerator>> _pool = new LockFreeQueue<PausableTask<IEnumerator>>();
    }
}
