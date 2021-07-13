using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace assignment2 {
    class Program {
        private static List<Band> bands = new List<Band>();

        static void Main(string[] args) {
            createBandsFromCSV();
            
            //Console.WriteLine($"{MostBandsFormedIn().Item1} was the most popular year for band formation with {MostBandsFormedIn().Item2} bands.");
            //Console.WriteLine($"{CountryWithMostProgressiveBands().Item1} had the most progressive bands: {CountryWithMostProgressiveBands().Item2}");
            //Console.WriteLine($"As of the current time, the longest band duration is {LongestShortestBandDuration().Item1} years, and the shortest is {LongestShortestBandDuration().Item2} years");
            //Console.WriteLine($"");

            //FinnishBlackMetalSince04();
            BandsWithUmlauts();
        }

        static void createBandsFromCSV() {
            DateTime localTime = DateTime.Now;
            Console.WriteLine($"Reading Data file....");

            string fileLocation = "./metal_bands_2017-cleaned.csv";
            StreamReader reader = new StreamReader(fileLocation, System.Text.Encoding.Default);
            string[] data = {};

            reader.ReadLine();
            try {
                while (!reader.EndOfStream) {

                    data = reader.ReadLine().Split(',');

                    if (data.Length != 7) {
                        continue;
                    }

                    // replace the -1 year (haven't split) with current year 
                    // for duration analysis
                    if (int.Parse(data[5]) == -1) {
                        data[5] = localTime.Year.ToString();
                    }

                    bands.Add(new Band(data[1].ToString(), int.Parse(data[2]),
                                int.Parse(data[3]), data[4].ToString(),
                                int.Parse(data[5]), data[6].ToString()));
                }
            }
            catch (Exception e) {
                Console.WriteLine($"{string.Join(",", data)}: {e.Message}");
            }

            Console.WriteLine($"Done.");
        }

        static Tuple<int, int> MostBandsFormedIn() {
            var bandQuery =
                from band in bands
                group band by band.YearFormed into yearGroup
                orderby yearGroup.Count() descending
                select yearGroup;

            return Tuple.Create(bandQuery.First().Key, bandQuery.First().Count());
        }

        static Tuple<string, int> CountryWithMostProgressiveBands() {
            var bandQuery =
                from band in bands
                where band.Style.ToLower().Contains("progressive")
                group band by band.Origin into countryGroup
                orderby countryGroup.Count() descending
                select countryGroup;

            return Tuple.Create(bandQuery.First().Key, bandQuery.First().Count());
        }

        static Tuple<int, int> LongestShortestBandDuration () {
            var bandQuery =
                from band in bands
                where band.YearSplit != -1
                group band by (band.YearSplit - band.YearFormed) into durationGroup
                orderby durationGroup.Count() descending
                select durationGroup;

            return Tuple.Create(bandQuery.Last().Key, bandQuery.First().Key);
            //foreach(var durationGroup in bandQuery) {
                //Console.WriteLine($"{durationGroup.Key}");
                //foreach(var band in durationGroup) {
                    //Console.WriteLine($"\t{band.ToString()}");
                //}
            //}
        }

        static void FinnishBlackMetalSince04() {
            var bandQuery =
                from band in bands
                where band.Origin.ToLower() == "finland" && band.Style.ToLower().Contains("black") && band.YearFormed >= 2004
                orderby band.Fans descending
                select band;

            Console.WriteLine($"Finish Black Metal Since 04");
            foreach(var band in bandQuery) {
                band.PrettyPrint();
            }
        }

        static bool containsUmlaut(string name) {
            Console.WriteLine($"{name}");
            int[] umlauts = new int[] {0196, 0214, 0220, 0223, 0228, 0246, 0252};

            foreach(int umlaut in umlauts) {
                if (name.Contains(Convert.ToChar(umlaut))) {
                    return true;
                }
            }

            return false;
        }

        static void BandsWithUmlauts() {
            var bandQuery = 
                from band in bands 
                where containsUmlaut(band.Name)
                select band;

            foreach(var band in bandQuery) {
                band.PrettyPrint();
            }
        }
    }
}
