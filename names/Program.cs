using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

namespace names {
    class Program {

        private static Dictionary<string, int> maleNames = new Dictionary<string, int>();
        private static Dictionary<string, int> femaleNames = new Dictionary<string, int>();

        static void Main(string[] args) {
            //ProcessAllYears();
            ProcessYear(1980);
            MostPopularMaleAndFemaleName();
        }

        static void DisplayYear(int year) {
            string fileLocation = "names/yob"+year+".txt";
            StreamReader reader = new StreamReader(fileLocation);
            try {
                while (!reader.EndOfStream) {
                    Console.WriteLine($"{reader.ReadLine()}");
                }
            }
            catch (Exception e) {
                Console.WriteLine($"{e}");
            }
        }

        static void ProcessAllYears() {
            // from 1880 to 2016
            for(int aYear = 1880; aYear <= 2016; aYear++) {
                ProcessYear(aYear);
            }
        }

        static void ProcessYear(int year) {
            string fileLocation = "names/yob"+year+".txt";
            StreamReader reader = new StreamReader(fileLocation);

            try {
                while (!reader.EndOfStream) {
                    string line = reader.ReadLine();
                    string[] data = line.Split(',');

                    if (data.Length != 3) {
                        continue;
                    }

                    AddToDictionary(data[0], data[1], int.Parse(data[2]));
                }
            }
            catch (Exception e) {
                Console.WriteLine($"{e}");
            }
        }

        static void AddToDictionary(string name, string sex, int number) {
            if (sex == "F" && femaleNames.ContainsKey(name)) {
                femaleNames[name] += number;
                return;
            }

            if (sex == "F") {
                femaleNames.Add(name, number);
                return;
            }

            if (maleNames.ContainsKey(name)) {
                maleNames[name] += number;
                return;
            }

            maleNames.Add(name, number);
        }

        // assuming all male and female names are in seperate Dictionary<string, int>
        // assuming only 1 year worth of data is collected
        // assuming no balance between numbers of male vs female in number of people
        // with that name. If the name is the same, the number of people is added.
        static void MostPopularMaleAndFemaleName() {
            Dictionary<string, int> commonNames = new Dictionary<string, int>();

            foreach (string mName in maleNames.Keys) {
                foreach (string fName in femaleNames.Keys) {
                    if (mName == fName) {
                        Console.WriteLine($"{mName} {maleNames[mName]} {femaleNames[fName]}");
                        commonNames.Add(mName, (maleNames[mName] + femaleNames[fName]));
                    }
                }
            }

            var commonResult = 
                from comName in commonNames
                orderby comName.Value descending
                select comName;

            Console.WriteLine($"{commonResult.First().Key} with {commonResult.First().Value} instances.");
        }

        static void MostPopularName() {
            var femaleResult = 
                from fNames in femaleNames
                orderby fNames.Value descending
                select fNames;

            Console.WriteLine($"{femaleResult.First().Key} with {femaleResult.First().Value} instances.");
        }
    }
}
