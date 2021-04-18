using System;
using System.Collections;
using System.Collections.Generic;

namespace Tools
{
    namespace Collections
    {
        class CustomCollection<T> : IEnumerable<T>
        {
            public IEnumerator<T> GetEnumerator()
            {
                return new CustomElement<T>();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new CustomElement<T>();
            }
        }

        class CustomElement<T> : IEnumerator<T>
        {
            public T Current => default(T);

            int count;
            public int Count { get { return count; } }

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                count++;

                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }
    }

}
