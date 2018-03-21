using System;
using System.Collections;
using System.Collections.Generic;
using Svelto.DataStructures;

namespace Svelto.Tasks
{
    public abstract class TaskCollection<T>: IEnumerator where T:IEnumerator
    {
        public bool            isRunning { protected set; get; }
        public abstract object Current   { get; }

        public abstract bool MoveNext();
        public abstract void Reset();

        public void Clear()
        {
            _listOfStacks.Clear();
        }

        public TaskCollection<T> Add(T enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException();

            CheckForToken(enumerator);

            Stack<T> stack;
            if (_listOfStacks.Reuse(_listOfStacks.Count, out stack) == false)
                stack = new Stack<T>(_INITIAL_STACK_SIZE);
            else
                stack.Clear();

            stack.Push(enumerator);
            _listOfStacks.Add(stack);

            return this;
        }

        /// <summary>
        /// Restore the list of stacks to their original state
        /// </summary>
        public void SafeReset()
        {
            var count = _listOfStacks.Count;
            for (int index = 0; index < count; ++index)
            {
                Stack<T> stack = _listOfStacks[index];
                while (stack.Count > 1) stack.Pop();
            }
        }

        protected TaskCollection()
                    : this(_INITIAL_STACK_COUNT)
        { }

        protected TaskCollection(int initialSize)
        {
            _listOfStacks = FasterList<Stack<T>>.PreFill<Stack<T>>(initialSize);
        }

        protected IEnumerator StandardEnumeratorCheck(object current)
        {
            if (current is IEnumerator)
            {
                CheckForToken(current);

                return (IEnumerator) current;
            }
#if DEBUG && !PROFILER
            if (current is IAbstractTask)
                throw new TaskYieldsIEnumerableException("yielding a task as been deprecated for performance issues, use new TaskEnumerator explicitly");
         
            if (current is IEnumerator[])
                throw new TaskYieldsIEnumerableException("yielding an array as been deprecated for performance issues, use paralleltask explicitly");

            if (current is IEnumerable)
                throw new TaskYieldsIEnumerableException("Yield an IEnumerable is not supported " + current.GetType());
#endif
            return null;
        }

        protected virtual void CheckForToken(object current)
        {}       

        protected FasterList<Stack<T>> _listOfStacks;

        const int _INITIAL_STACK_COUNT = 3;
        const int _INITIAL_STACK_SIZE = 3;
    }
}

