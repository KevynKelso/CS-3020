using System;

public enum ShipTypes {
    Destroyer = 2,
    Submarine = 3,
    Battleship = 4,
    Carrier = 5
}

namespace assignment1 {
    class Ship {
        private ShipTypes length;
        // if not vertical, it will be horizontal orientation
        private bool verticalOrientation;
        private int headRow;
        private int headCol;
        private Gameboard board;

        // constructs a ship at a valid board location
        public Ship(ShipTypes length, Gameboard board) {
            if (!validLength(length)) {
                Console.WriteLine($"Invalid ship length {length}");
                return;
            }

            this.length = length;
            this.board = board;
            // sets private variables headRow, headCol, and verticalOrientation
            generateValidShip();
        }


        // creates ship that is not running into a ship. Placement is random
        private void generateValidShip() {
            var rand = new Random();
            verticalOrientation = rand.Next(2) == 0;
            bool colision = true;

            while (colision) {
                // if it is vertical, we cant place it length squares away from the bottom
                if (verticalOrientation) { 
                    headRow = rand.Next(0, board.GetRowLength() - (int)length);
                    headCol = rand.Next(0, board.GetColLength());

                    colision = setBoardLocation();
                    continue;
                }

                // otherwise, need to prevent it from hitting the right side
                headRow = rand.Next(0, board.GetRowLength());
                headCol = rand.Next(0, board.GetColLength() - (int)length);

                colision = setBoardLocation();
            }
        }

        // If you haven't noticed, I hate using else clauses. In my opinion,
        // they add clutter to your code.
        // return true = error. writes 'S' characters to the board
        private bool setBoardLocation() {
            // if there's already a ship, we need to error and reverse
            if (checkColision()) {
                return true;
            }
            
            // start at head location and move down
            if (verticalOrientation) {
                for(int i = 0; i < (int)length; i++) {
                    board.SetCell(headRow+i, headCol, 'S');
                }
                return false;
            }

            // otherwise move left to right
            for(int i = 0; i < (int)length; i++) {
                board.SetCell(headRow, headCol+i, 'S');
            }
            return false;
        }

        // checks to make sure ships are not running into each other
        private bool checkColision() {
            if (verticalOrientation) {
                for(int i = 0; i < (int)length; i++) {
                    if (board.GetCell(headRow+i, headCol) == 'S') {
                        return true;
                    }
                }

                return false;
            }

            for(int i = 0; i < (int)length; i++) {
                if (board.GetCell(headRow, headCol+i) == 'S') {
                    return true;
                }
            }

            return false;
        }

        public int getHits() {
            return (int)this.length;
        }

        private bool validLength(ShipTypes length) {
            if (length < ShipTypes.Destroyer || length > ShipTypes.Carrier) {
                Console.WriteLine($"Length {(int)length} is invalid. Must be between 2 and 5");
                return false;
            }
            return true;
        }
    }
}
