using System;
using System.Text.RegularExpressions;

namespace assignment1 {
    class Game {
        private bool isRunning;
        private Gameboard board = new Gameboard(true);

        public Game() {
            this.isRunning = true;
        }

        // entry point into the game
        public void Start() {
            board.ResetBoard();
            GenerateAllTheShips();
            PlayerRun();
            board.Display();
            PlayerTurn();
        }

        private void PlayerRun() {
            bool won = false;
            int turns = 0;

            while (!won) {
                board.Display();
                PlayerTurn();
                won = checkWin();
                turns++;
            }

            Console.WriteLine($"You won in {turns} turns!");
        }

        private bool checkWin() {
            for(int i = 0; i < board.GetRowLength(); i++) {
                for(int j = 0; j < board.GetColLength(); j++) {
                    if (board.GetCell(i, j) == 'S')
                        return false;
                }
            }

            return true;
        }

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

        public void GenerateAllTheShips() {
            Ship destroyer1 = GenerateValidShip(2);
            Ship destroyer2 = GenerateValidShip(2);
            Ship sub1 = GenerateValidShip(3);
            Ship sub2 = GenerateValidShip(3);
            Ship battleship1 = GenerateValidShip(4);
            Ship carier1 = GenerateValidShip(5);
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
            if (input.Length < 2 || input.Length > 3 || 
                (int)(input[0]-65) < 0 || (int)(input[0]-65) > board.GetRowLength() || 
                int.Parse(input.Substring(1)) < 0 || int.Parse(input.Substring(1)) > board.GetColLength()) {
                return false;
            }
            char targetRow = userInput[0];
            char targetCol = int.Parse(userInput.Substring(1));
            char cell = board.GetCellUser(targetRow, targetCol);

            if (cell == 'X' || cell == 'O') {
                return false;
            }

            return true;
        }

        // basic player logic
        private void PlayerTurn() {
            char targetRow;
            int targetCol;
            char replaceChar = 'X';
            string message = "Miss :(";

            string userInput = PromptUser();
            if (!valid(userInput)) {
                Console.WriteLine($"The input you entered: {userInput} is invalid. Please try again.");
                return;
            }
            
            targetRow = userInput[0];
            targetCol = int.Parse(userInput.Substring(1));
            Console.WriteLine($"Targeted Cell: {targetRow},{targetCol}");

            if (board.GetCellUser(targetRow, targetCol) == 'S') {
                message = "Hit!";
                replaceChar = 'O';
            }

            Console.WriteLine($"{message}");
            board.SetCellUser(targetRow, targetCol, replaceChar);
        }


    }
}
