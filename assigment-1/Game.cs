using System;
using System.Text.RegularExpressions;

namespace assignment1 {
    class Game {
        private bool isRunning;
        private Gameboard board = new Gameboard();
        private Ship[] ships;
        private int numDestroyers;
        private int numSubs;
        private int numBattleships;
        private int numCarriers;

        public Game(int numDestroyers, int numSubs, int numBattleships, int numCarriers) {
            this.isRunning = true;
            this.numDestroyers = numDestroyers;
            this.numSubs = numSubs;
            this.numBattleships = numBattleships;
            this.numCarriers = numCarriers;
            this.ships = new Ship[numCarriers + numSubs + numBattleships + numDestroyers];
        }

        // entry point into the game
        public void Start() {
            board.ResetBoard();
            PlayerStart(GenerateAllTheShips());
        }

        private void PlayerStart(int hitsToWin) {
            int turns = 0;
            int hits = 0;

            while (hits < hitsToWin && isRunning) {
                board.Display();
                hits += PlayerTurn();
                turns++;
            }

            Console.WriteLine($"You won in {turns} turns!\nAccuracy: {hitsToWin / hits * 100}%");
        }

        // creates ship that is not running into a ship. Placement is random
        public Ship GenerateValidShip(int length) {
            var rand = new Random();
            bool vertical = rand.Next(2) == 0;
            bool colision = true;
            int colLoc = 0;
            int rowLoc = 0;

            while (colision) {
                // if it is vertical, we cant place it length squares away from the bottom
                if (vertical) { 
                    rowLoc = rand.Next(0, board.GetRowLength() - length);
                    colLoc = rand.Next(0, board.GetColLength());

                    colision = setBoardLocation(length, vertical, rowLoc, colLoc);
                    continue;
                }

                // otherwise, need to prevent it from hitting the right side
                rowLoc = rand.Next(0, board.GetRowLength());
                colLoc = rand.Next(0, board.GetColLength() - length);

                colision = setBoardLocation(length, vertical, rowLoc, colLoc);
            }

            return new Ship(length, vertical, rowLoc, colLoc);
        }

        private bool checkColision(int length, bool vertical, int rowLoc, int colLoc) {
            if (vertical) {
                for(int i = 0; i < length; i++) {
                    if (board.GetCell(rowLoc+i, colLoc) == 'S') {
                        return true;
                    }
                }

                return false;
            }

            for(int i = 0; i < length; i++) {
                if (board.GetCell(rowLoc, colLoc+i) == 'S') {
                    return true;
                }

            }

            return false;
        }

        // If you haven't noticed, I hate using else clauses. In my opinion,
        // they add clutter to your code.
        // true = error
        private bool setBoardLocation(int length, bool vertical, int rowLoc, int colLoc) {
            // if there's already a ship, we need to error and reverse
            if (checkColision(length, vertical, rowLoc, colLoc)) {
                return true;
            }
            
            // start at head location and move down
            if (vertical) {
                for(int i = 0; i < length; i++) {
                    board.SetCell(rowLoc+i, colLoc, 'S');
                }

                return false;
            }
            // otherwise move left to right
            for(int i = 0; i < length; i++) {
                board.SetCell(rowLoc, colLoc+i, 'S');
            }

            return false;
        }

        private int GenerateShips(int numShips, int length) {
            int hits = 0;

            for(int i = 0; i < numShips; i++) {
                Ship ship = GenerateValidShip(length);
                hits += length;
            }

            return hits;
        }

        public int GenerateAllTheShips() {
            int maxHits = 0;
            int carrierLength = 5;
            int battleshipLength = 4;
            int subLength = 3;
            int destroyerLength = 2;
            // starting with the bigger ships allows more placement options for
            // the smaller ships and less chance of colision having to generate
            // new random numbers
            maxHits += GenerateShips(numCarriers, carrierLength);
            maxHits += GenerateShips(numBattleships, battleshipLength);
            maxHits += GenerateShips(numSubs, subLength);
            maxHits += GenerateShips(numDestroyers, destroyerLength);

            return maxHits;
        }

        // I might get rid of this method and put the loop in Start()
        public void Update() {
            while (isRunning) {
                board.Display();
                Console.ReadLine();
            }
        }

        // gets coord info from user
        private string PromptUser() {
            Console.Write("\nPlease enter target coordinates (row then col no spaces)");
            return Regex.Replace(Console.ReadLine().ToUpper().Trim(), @"\s+", "");
        }

        // ensures user input is within the parameters of the gameboard
        private bool valid(string input) {
            if (input.Length < 2 || input.Length > 3) {
                return false;
            }

            if ((int)(input[0]) == 90 && int.Parse(input.Substring(1)) == 0) {
                board.ToggleHacks();
                Console.WriteLine($"|-|@><$ enabled.");
                return false;
            }

            if ((int)(input[0]-65) < 0 || (int)(input[0]-65) > board.GetRowLength() || 
                int.Parse(input.Substring(1)) < 0 || int.Parse(input.Substring(1)) > board.GetColLength()) {
                return false;
            }

            char targetRow = input[0];
            int targetCol = int.Parse(input.Substring(1));
            char cell = board.GetCellUser(targetRow, targetCol);

            if (cell == 'X' || cell == 'O') {
                Console.WriteLine($"Wow, isn't that horse already dead?");
                return false;
            }

            return true;
        }

        // basic player logic
        private int PlayerTurn() {
            char targetRow;
            int targetCol;
            char replaceChar = 'X';
            string message = "Miss, u suk";
            bool hit = false;

            string userInput = PromptUser();
            if (!valid(userInput)) {
                Console.WriteLine($"The input you entered: {userInput} is invalid. Please try again.");
                return 0;
            }
            
            targetRow = userInput[0];
            targetCol = int.Parse(userInput.Substring(1));
            Console.WriteLine($"Targeted Cell: {targetRow},{targetCol}");

            if (board.GetCellUser(targetRow, targetCol) == 'S') {
                message = "Hit!!!!!!!!!!!!!!! AAHHHHHHHHHHHHHH! BOOOOOOOOOOMMMMMMMMM!\n SPLASHHHHH!";
                replaceChar = 'O';
                hit = true;
            }

            Console.WriteLine($"{message}");
            board.SetCellUser(targetRow, targetCol, replaceChar);
            
            return hit ? 1 : 0;
        }
    }
}
