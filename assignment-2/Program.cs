using System;
using System.IO;
using System.Collections.Generic;

namespace assignment2 {
    class Program {
        private static List<Band> bands = new List<Band>();

        static void Main(string[] args) {
            createBandsFromCSV();
            foreach(Band band in bands) {
                Console.WriteLine($"{band.ToString()}");
            }
        }

        static void createBandsFromCSV() {
            string fileLocation = "./metal_bands_2017-cleaned.csv";
            StreamReader reader = new StreamReader(fileLocation);
            reader.ReadLine();
            try {
                while (!reader.EndOfStream) {
                    string[] data = reader.ReadLine().Split(',');
                    bands.Add(new Band(data[1], int.Parse(data[2]),
                                int.Parse(data[3]), data[4],
                                int.Parse(data[5]), data[6]));
                }
            }
            catch (Exception e) {
                Console.WriteLine($"{e.Message}");
            }
        }
    }
}
