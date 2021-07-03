using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace assignment1 {
    class Game {
        private bool isRunning;
        private Gameboard board = new Gameboard();
        private List<Ship> ships = new List<Ship>();
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
        }

        // entry point into the game
        public void Start() {
            board.ResetBoard();
            playerStart(GenerateAllTheShips());
        }

        // initializes and runs game
        private void playerStart(int hitsToWin) {
            int turns = 0;
            int hits = 0;

            Console.WriteLine($"\n\nStarting game....\nWhatever you do, don't press Z0.");
            while (hits < hitsToWin && isRunning) {
                board.Display();
                hits += playerTurn() ? 1 : 0;
                turns++;
            }

            Console.WriteLine($"You won in {turns} turns!\nAccuracy: {((double)hitsToWin/turns) * 100}%");
            promptPlayAgain();
        }

        // single character prompt to play again
        private void promptPlayAgain() {
            Console.Write($"Play again? [Y/n]:");
            char input = Convert.ToChar(Console.Read());

            if (input == 'y' || input == 'Y' || input == '\n') {
                Start();
                return;
            }
            
            Console.WriteLine($"\nGoodbye!\n\nWritten by Kevyn Kelso.");
        }

        // generates a specific type of ship, and calculates total number of hits
        // on the board for win condition
        private int generateShips(int numShips, ShipTypes length) {
            int hits = 0;

            for(int i = 0; i < numShips; i++) {
                ships.Add(new Ship(length, board));
                hits += (int)length;
            }

            return hits;
        }

        // places all ships on the board. returns number of 'S' locations for 
        // win condition
        public int GenerateAllTheShips() {
            int maxHits = 0;
            // starting with the bigger ships allows more placement options for
            // the smaller ships and less chance of colision having to generate
            // new random numbers slowing the game down
            maxHits += generateShips(numCarriers, ShipTypes.Carrier);
            maxHits += generateShips(numBattleships, ShipTypes.Battleship);
            maxHits += generateShips(numSubs, ShipTypes.Submarine);
            maxHits += generateShips(numDestroyers, ShipTypes.Destroyer);

            return maxHits;
        }

        // gets coord info from user
        private string promptUser() {
            Console.Write("\nPlease enter target coordinates (row then col no spaces)");
            return Regex.Replace(Console.ReadLine().ToUpper().Trim(), @"\s+", "");
        }

        // ensures user input is within the parameters of the gameboard
        private bool valid(string input) {
            // basic length check
            if (input.Length < 2 || input.Length > 3) {
                return false;
            }

            // hacks
            if ((int)(input[0]) == 90 && int.Parse(input.Substring(1)) == 0) {
                board.ToggleHacks();
                Console.WriteLine($"|-|@><$ toggled... This will apply to all other games.");
                return false;
            }

            // within board params
            if ((int)(input[0]-65) < 0 || (int)(input[0]-65) >= board.GetRowLength() || 
                int.Parse(input.Substring(1)) <= 0 || int.Parse(input.Substring(1)) > board.GetColLength()) {
                return false;
            }

            char targetRow = input[0];
            int targetCol = int.Parse(input.Substring(1));
            char cell = board.GetCellUser(targetRow, targetCol);

            // has the user already chosen that location
            if (cell == 'X' || cell == 'O') {
                Console.WriteLine($"Wow, isn't that horse already dead?");
                return false;
            }

            return true;
        }

        // just your average helper method 
        private void updateBoard(char replaceChar, String message, char targetRow, int targetCol) {
            Console.WriteLine($"{message}");
            board.SetCellUser(targetRow, targetCol, replaceChar);
        }

        // basic player logic, validates input, sets cells on board returns hits
        private bool playerTurn() {
            char targetRow;
            int targetCol;

            string userInput = promptUser();
            if (!valid(userInput)) {
                Console.WriteLine($"The input you entered: {userInput} is invalid. Please try again.");
                return false;
            }
            
            targetRow = userInput[0];
            targetCol = int.Parse(userInput.Substring(1));
            Console.WriteLine($"Targeted Cell: {targetRow},{targetCol}");

            foreach(Ship ship in ships) {
                if (ship.IsHit(targetRow, targetCol)) {
                    updateBoard('O', "Sir, we have a hit.", targetRow, targetCol);
                    return true;
                }
            }

            updateBoard('X', "Miss, u suk.", targetRow, targetCol);
            return false;
        }
    }
}
