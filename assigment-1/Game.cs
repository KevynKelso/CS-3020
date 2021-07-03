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

            Console.WriteLine($"Starting game....\nWhatever you do, don't press Z0.");
            while (hits < hitsToWin && isRunning) {
                board.Display();
                hits += PlayerTurn() ? 1 : 0;
                turns++;
            }

            Console.WriteLine($"You won in {turns} turns!\nAccuracy: {((double)hitsToWin/turns) * 100}%");
        }

        // generates a specific type of ship, and calculates total number of hits
        // on the board for win condition
        private int GenerateShips(int numShips, ShipTypes length) {
            int hits = 0;

            for(int i = 0; i < numShips; i++) {
                Ship ship = new Ship(length, board);
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
            maxHits += GenerateShips(numCarriers, ShipTypes.Carrier);
            maxHits += GenerateShips(numBattleships, ShipTypes.Battleship);
            maxHits += GenerateShips(numSubs, ShipTypes.Submarine);
            maxHits += GenerateShips(numDestroyers, ShipTypes.Destroyer);

            return maxHits;
        }

        // gets coord info from user
        private string PromptUser() {
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
                Console.WriteLine($"|-|@><$ enabled.");
                return false;
            }

            // within board params
            if ((int)(input[0]-65) < 0 || (int)(input[0]-65) >= board.GetRowLength() || 
                int.Parse(input.Substring(1)) < 0 || int.Parse(input.Substring(1)) > board.GetColLength()) {
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

        // basic player logic, validates input, sets cells on board returns hits
        private bool PlayerTurn() {
            char targetRow;
            int targetCol;
            char replaceChar = 'X';
            string message = "Miss, u suk";
            bool hit = false;

            string userInput = PromptUser();
            if (!valid(userInput)) {
                Console.WriteLine($"The input you entered: {userInput} is invalid. Please try again.");
                return false;
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
            
            return hit;
        }
    }
}
