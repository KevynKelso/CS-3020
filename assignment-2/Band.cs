using System;

namespace assignment2 {
    class Band {
        public string Name { get; set; }
        public int Fans { get; set; }
        public int YearFormed { get; set; }
        public string Origin { get; set; }
        public int YearSplit { get; set; }
        public string Style { get; set; }

        public Band(string name, int fans, int yearFormed, string origin, 
                int yearSplit, string style) {
            this.Name = name;
            this.Fans = fans;
            this.YearFormed = yearFormed;
            this.Origin = origin;
            this.YearSplit = yearSplit;
            this.Style = style;
        }

        public override string ToString() {
            return $"{Name} {Fans} {YearFormed} {Origin} {YearSplit} {Style}";
        }

        public void PrettyPrint() {
            Console.WriteLine($"{Name}\n\t{Fans} fans,\n\tFormed in {YearFormed},\n\tFrom {Origin},\n\t{(YearSplit == DateTime.Now.Year ? "Still together" : $"Split {YearSplit.ToString()}")},\n\tStyle: {Style}");
        }
    }
}
