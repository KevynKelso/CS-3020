using System;

namespace assignment1 {
    class Ship {
        private int length;
        // if not vertical, it will be horizontal orientation
        private bool verticalOrientation;
        private int headRow;
        private int headCol;
        // can bowX bowY be calculated in gameboard, do we really need coord info?
        //private int bowX;
        //private int bowY;
        //private int sternX;
        //private int sternY;

        public Ship(int length, bool verticalOrientation, int headRow, int headCol) {
            if (!validLength(length)) {
                return;
            }

            if (!validLoc(headRow, headCol)) {
                return;
            }

            this.length = length;
            this.verticalOrientation = verticalOrientation;
            //this.bowX = bowX;
            //this.bowY = bowY;
            //this.sternX = sternX; 
            //this.sternY = sternY;
        }

        private bool validLoc(int row, int col) {
            // create new gameboard only for checking lengths
            Gameboard board = new Gameboard();
            if (row < 0 || row > board.GetRowLength() || col < 0 || col > board.GetColLength()) {
                return false;
            }
            return true;
        }

        private bool validLength(int length) {
            if (length < 2 || length > 5) {
                Console.WriteLine($"Length {length} is invalid. Must be between 2 and 5");
                return false;
            }
            return true;
        }

    }
}
