namespace assignment2 {
    class Band {
        private string name;
        private int fans;
        private int yearFormed;
        private string origin;
        private int yearSplit;
        private string style;

        public Band(string name, int fans, int yearFormed, string origin, 
                int yearSplit, string style) {
            this.name = name;
            this.fans = fans;
            this.yearFormed = yearFormed;
            this.origin = origin;
            this.yearSplit = yearSplit;
            this.style = style;
        }

        public override string ToString() {
            return $"{name} {fans} {yearFormed} {origin} {yearSplit} {style}";
        }
    }
}
