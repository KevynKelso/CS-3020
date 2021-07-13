using System;

namespace GenericsExperimentation {
    class Program {
        static void Main(string[] args) {
            int[] iNums = new int[] {1,2,3,4,5,6,7,8,9,10};
            float[] fNums = new float[] {1f, 2f, 6f, 20f, 456.3f, 902875.7890f};
            char[] chars = new char[] {'a', 'g', '#'};

            //Display(iNums);

            MyCollection<string> myCollection = new MyCollection<string>();
            myCollection.Add("kevyn");
            myCollection.Add(new string[]{"hello", "yo", "the", "quick", "brown", "fox"});
            myCollection.Display();
            myCollection.Sort();
            myCollection.Display();

            Console.WriteLine($"{GetMax(iNums)}");
            Console.WriteLine($"{myCollection.GetMax()}");
        }

        static void Display<T>(T[] input) {
            foreach (var e in input) {
                Console.WriteLine($"{e}");
            }
            Console.WriteLine($"");
        }

        static T GetMax<T> (T[] input) where T:IComparable {
            T max = input[0];
            foreach(var e in input) {
                if (max.CompareTo(e) < 0) {
                    max = e;
                }
            }
            
            return max;
        }
    }
}
