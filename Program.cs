using System;
using System.Reflection.Metadata.Ecma335;

namespace Sudoku_Solver_and_Creator
{
    class Program
    {
        /*
         r = row
         c = column
         n = newNum
        
         */
        static Random random = new Random();

        //Validity Checker
        static bool IsValid(int r, int c, int newNum, int[,] sudokuBoard)
        {
            return !InRow(r, newNum, sudokuBoard) &&
                   !InColumn(c, newNum, sudokuBoard) &&
                   !InGrid(r -r % 3, c -c % 3 , newNum, sudokuBoard);  //Returns the 3 states 
        }
        static bool InRow(int r, int newNum, int[,] sudokuBoard)
        {
            for (int i = 0; i < sudokuBoard.GetLength(0); i++)
            {
                if (newNum == sudokuBoard[r, i])
                {
                    return true;
                }
            }
            return false;
        }
        static bool InColumn(int c, int newNum, int[,] sudokuBoard)
        {
            for (int i = 0; i < sudokuBoard.GetLength(1); i++)
            {
                if (newNum == sudokuBoard[i, c])
                {
                    return true;
                }

            }
            return false;
        }
        static bool InGrid(int r, int c, int newNum, int[,] sudokuBoard)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (sudokuBoard[row + r, col + c] == newNum)
                    {
                        return true;
                    }
                }
            }
            return false;

        }
        //End of Validity Checker

        //Sudoku Solver Using Backtracking
        static bool SolveSudoku(int[,] sudokuBoard)
        {          
            for (int r = 0; r < sudokuBoard.GetLength(0); r++)
            {
                for (int c = 0; c < sudokuBoard.GetLength(1); c++)
                    if (sudokuBoard[r, c] == 0)
                    {
                        for (int n = 1; n <= 9; n++)
                        {
                            if (IsValid(r, c, n, sudokuBoard))
                            {
                                sudokuBoard[r, c] = n;
                                if (SolveSudoku(sudokuBoard))
                                {
                                    return true;

                                }
                                sudokuBoard[r, c] = 0; // backtracking
                            }

                        }
                        return false; // returns false if the Puzzle is not Solvable
                    }
            }
            PrintBoard(sudokuBoard);
            return true; //Will return True when all spaces have been filled 
        }


        //Functions for Generating a completed SudokuBoard and shuffled to increase the randomness
        static bool GenerateShuffledBoard(int[,] sudokuBoard) 
        {
            int randomRow; 
            int randomCol;
            for (int r = 0; r < sudokuBoard.GetLength(0); r++)
            {
                for (int c = 0; c < sudokuBoard.GetLength(1); c++)
                    if (sudokuBoard[r, c] == 0)
                    {
                        for (int n = 1; n <= 9; n++)
                        {
                            if (IsValid(r, c, n, sudokuBoard))
                            {
                                sudokuBoard[r, c] = n;
                                if (GenerateShuffledBoard(sudokuBoard))
                                {
                                    return true;

                                }
                                sudokuBoard[r, c] = 0; // backtracking
                            }

                        }
                        return false; // returns false if the Puzzle is not Solvable
                    }
            }
            for (int s = 0; s < 150; s++) 
            {
                randomRow = random.Next(0, sudokuBoard.GetLength(0));
                randomCol = random.Next(0, sudokuBoard.GetLength(1));
                sudokuBoard = ShuffleRows(sudokuBoard, randomRow);
                sudokuBoard = ShuffleColumns(sudokuBoard, randomCol);
            
            }
            PrintBoard(sudokuBoard);
            Console.WriteLine("");
            RemoveNumbers(sudokuBoard);
            Console.WriteLine("");
            SolveSudoku(sudokuBoard);
            return true; //Will return True when all spaces have been filled 

        }

        //Shuffling Algorithms for more unique Puzzles
        static int[,]ShuffleRows(int[,] sudokuBoard, int rowOne)
        {
            rowOne -= rowOne % 3;
            int newRow;
            do
            {
                newRow = rowOne + random.Next(0, 2);


            }
            while (newRow == rowOne);
            
            for (int col = 0; col < sudokuBoard.GetLength(0); col++)
            {
                int temp = sudokuBoard[rowOne, col];
                sudokuBoard[rowOne, col] = sudokuBoard[newRow, col];
                sudokuBoard[newRow, col] = temp;

            }
            return sudokuBoard;
        }

        static int[,]ShuffleColumns(int[,] sudokuBoard, int colOne)
        {
            int gridPos;
            gridPos = colOne-= colOne % 3;  //converts position into a multiple of 3 to determine position of grid within the array
            int newCol;
            do
            {
                newCol = colOne + random.Next(0, 2);

            }
            while (newCol == colOne);
             //swaps pos with new pos within grid
            for (int row = 0; row < sudokuBoard.GetLength(1); row++)  //loops and replaces
            {
                int temp = sudokuBoard[row, colOne];
                sudokuBoard[row, colOne] = sudokuBoard[row, newCol];
                sudokuBoard[row, newCol] = temp;
            }
            return sudokuBoard;
            
            
        }



        //Function that removes Positions from the Sudoku Board randomly and ensures theres still only one unique solution

        static int solutionAmount = 0;
        static void RemoveNumbers(int[,] sudokuBoard) 
        {
            int randomRow;
            int randomCol;
            int removedNumbers = 40;
            while (removedNumbers != 0) 
            {
                randomRow = random.Next(0, sudokuBoard.GetLength(0));
                randomCol = random.Next(0, sudokuBoard.GetLength(1));
                if (sudokuBoard[randomRow, randomCol] != 0)
                {
                    sudokuBoard[randomRow, randomCol] = 0;

                    removedNumbers--;


                }


            }
            PrintBoard(sudokuBoard);
        }
        //Print Board function that uses a nested loop to individually print out each vaue of the 2d array
        static void PrintBoard(int[,] sudokuBoard)
        {
            for (int r = 0; r < sudokuBoard.GetLength(0); r++)
            {
                for (int c = 0; c < sudokuBoard.GetLength(1); c++)
                {
                    Console.Write(sudokuBoard[r, c] + " ");

                }
                Console.WriteLine();

            }
        }

        static void Main(string[] args)
        {
            int[,] sudokuBoardTest = {
                {8, 3, 0,  0, 0, 0,  1, 0, 0 },
                {0, 0, 0,  0, 0, 2,  3, 0, 0 },
                {1, 0, 0,  0, 5, 0,  0, 0, 4 }
                ,
                {9, 8, 0,  1, 0, 5,  0, 7, 2 },
                {2, 5, 7,  9, 0, 0,  0, 3, 1 },
                {6, 1, 3,  7, 2, 8,  0, 4, 0 }
                ,
                {4, 2, 0,  5, 0, 1,  0, 0, 3 },
                {0, 7, 8,  0, 0, 9,  0, 0, 5 },
                {0, 6, 0,  4, 0, 0,  0, 0, 0},

            };
            int[,] emptyBoard = new int[9, 9];
            GenerateShuffledBoard(emptyBoard);

        }

    }
}




