using System;
using System.Collections.Generic;
using System.Text;

namespace assignment1 {
    class Gameboard {
        private char[,] board = new char[10, 10]; 
        private bool hacks = false;

        public Gameboard() {
            ResetBoard();
        }

        public void ToggleHacks() {
            this.hacks = !this.hacks;
        }

        // Displays the gameboard on the screen with rows as cap letters and
        // cols as numbers
        public void Display() {
            Console.WriteLine("  1  2  3  4  5  6  7  8  9 10 ");

            for(int row = 0; row < board.GetLength(0); row++) {
                Console.Write($"{(char)(row+65)}");

                for(int col = 0; col < board.GetLength(0); col++) {
                    char position = board[row, col];
                    // hide ships if hacks are not enabled
                    if (position == 'S' && !hacks) {
                        position = ' ';
                    }

                    Console.Write($"[{position}]");
                }

                Console.WriteLine();
            }
        }

        // Sets every board position to contain newChar
        public void FillBoard(char newChar) {
            for(int row = 0; row < board.GetLength(0); row++) {
                for(int col = 0; col < board.GetLength(0); col++) {
                    board[row,col] = newChar;
                }
            }
        }

        // resets the board with all water
        public void ResetBoard() {
            FillBoard(' ');
        }

        // returns character at specified location, human readable
        public char GetCellUser(char row, int col) {
            int rowNumber = (int)(row-65);
            if (rowNumber > board.GetLength(0) || rowNumber < 0 || col < 0 || col > board.GetLength(1)) {
                return '@';
            }

            return board[rowNumber,col-1];
        }

        // returns character at specified location, zero indexed
        public char GetCell(int row, int col) {
            if (row > board.GetLength(0)-1 || row < 0 || col < 0 || col > board.GetLength(1)-1) {
                return '@';
            }

            return board[row,col];
        }

        // sets board location to desired cell, zero indexed
        public bool SetCell(int row, int col, char newChar) {
            if (row > board.GetLength(0)-1 || row < 0 || col < 0 || col > board.GetLength(1)-1) {
                Console.WriteLine($"Invalid board location {row}, {col}.");
                return false;
            }

            board[row,col] = newChar;
            return true;
        }

        // sets board location to desired cell, human readable
        public bool SetCellUser(char row, int col, char newChar) {
            int rowNumber = (int)(row-65);
            int colNumber = col - 1;

            if (rowNumber > board.GetLength(0)-1 || rowNumber < 0 || colNumber < 0 || colNumber > board.GetLength(1)-1) {
                Console.WriteLine($"Invalid board location {rowNumber}, {colNumber}.");
                return false;
            }

            board[rowNumber,colNumber] = newChar;
            return true;
        }

        // wrapper for GetLength(0); makes code outside this file more readable
        // and less confusing
        public int GetRowLength() {
            return board.GetLength(0);
        }

        // wrapper for GetLength(1); makes code outside this file more readable
        // and less confusing
        public int GetColLength() {
            return board.GetLength(1);
        }

    } 
}
