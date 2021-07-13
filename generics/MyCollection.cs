using System;
using System.Collections.Generic;

namespace GenericsExperimentation {
    class MyCollection<T> where T:IComparable  {
        private List<T> content = new List<T>();

        public void Add(T newElem) {
            content.Add(newElem);
        }

        public void Add(T[] inputs) {
            foreach (var e in inputs) {
                content.Add(e);
            }
        }

        public void Display() {
            foreach (var e in content) {
                Console.WriteLine($"{e}");
            }
            Console.WriteLine($"");
        }

        public void Sort() {
            content.Sort();
        }

        public void DescendingSort() {
            content.Sort();
            content.Reverse();
        }

        public T GetMax() {
            T max = content[0];

            foreach(var e in content) {
                if (max.CompareTo(e) < 0) {
                    max = e;
                }
            }

            return max;
        }
    }
}
