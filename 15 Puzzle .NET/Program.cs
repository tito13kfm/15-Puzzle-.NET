using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _15_Puzzle.NET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //setup initial board state and winning board for reference
            int[,] board = new int[4, 4] {
            {1, 2, 3, 4},
            {5, 6, 7, 8},
            {9, 10, 11, 12},
            {13, 14, 15, 0}
};
            int[,] winningBoard = new int[4, 4] {
            {1, 2, 3, 4},
            {5, 6, 7, 8},
            {9, 10, 11, 12},
            {13, 14, 15, 0}
};
            int correct;
            bool valid;
            int blankRow = 0, blankCol = 0, moves = 0;
            var startTime = DateTime.Now;
            string time;
            
            //Random number generator initialized
            Random random = new Random();

            Console.Title = "15 Puzzle";
            Console.CursorVisible = false;
            
            InitBoard();
            PrintBoard();
            GetMove();

            //Function to randomize board starting position
            void InitBoard()
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        //Doing 16 swaps to randomize board
                        int k = random.Next(4);
                        int l = random.Next(4);
                        int tmp = board[i, j];
                        board[i, j] = board[k, l];
                        board[k, l] = tmp;
                    }
                }
                //Check current board to see if it's winnable
                if (CheckValid(board, winningBoard)) { return; }
                else { InitBoard(); }
            }

            bool CheckValid(int[,] tryBoard, int[,] checkBoard)
            {
                //Call function to count number of correct positions
                CountCorrect(board, winningBoard);
                // If the number of correct positions, plus the row the blank tile is in, plus 1 is even, then it's a valid board
                valid = false;
                if ((correct + blankRow + 1) % 2 == 0)
                {
                    valid = true;
                    return valid;
                }
                return valid;
            }

            int CountCorrect(int[,] tryBoard, int[,] checkBoard)
            {
                valid = false;
                correct = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (tryBoard[i, j] == checkBoard[i, j])
                        {
                            //count up the number of tiles in correct location
                            correct++;
                        }
                    }
                }
                return correct;
            }

            void GetMove()
            {
                AppendBoard();
                int targetRow = 0;
                int targetCol = 0;
            
                //check which key is pressed and set target piece based on location of blank tile
                ConsoleKeyInfo move = Console.ReadKey();
                switch (move.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        targetRow = blankRow + 1;
                        targetCol = blankCol;
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        targetRow = blankRow;
                        targetCol = blankCol + 1;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        targetRow = blankRow - 1;
                        targetCol = blankCol;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        targetRow = blankRow;
                        targetCol = blankCol - 1;
                        break;
                    default:
                        targetRow = blankRow;
                        targetCol = blankCol;
                        break;
                }

                //temporary location of timer function until I figure out classes
                var aTimer = DateTime.Now - startTime;
                time = aTimer.TotalSeconds.ToString("0.0");
                Console.Title = "15 Puzzle - " + time + " seconds elapsed";
                
                //Check if it's a valid move, and increment move counter
                if (targetRow < 0 || targetCol < 0) { GetMove(); }
                else if (targetRow > 3 || targetCol > 3) { GetMove(); }
                else { moves++; }
                
                //Swap selected tile with blank tile
                int piece = board[targetRow, targetCol];
                board[blankRow, blankCol] = piece;
                board[targetRow, targetCol] = 0;
                CheckWin();

            }

            void AppendBoard()
            {
                //setup new variable using StringBuilder function
                var sb = new StringBuilder();

                //Append all the labels to the 16 spots on the board.
                for (int i = 0; i < 4; i++)
                {
                    sb = new StringBuilder();
                    Console.SetCursorPosition(3, 4 * i + 3);
                    for (int j = 0; j < 4; j++)
                    {
                        sb.Append("║  " + displayValue(board[i, j]) + "   ");
                        if (board[i, j] == 0)
                        {
                            blankRow = i;
                            blankCol = j;
                        }

                    }
                    Console.Write(sb);
                }
                
                //Likely better way of doing this, but for now just move the cursor where I want it and write the move count
                Console.SetCursorPosition(13, 20);
                Console.Write(moves);
                return;
            }

            void CheckWin()
            {
                CountCorrect(board, winningBoard);
                if (correct == 16)
                {
                    Console.Clear();
                    Console.WriteLine("");
                    Console.WriteLine("You Won in " + moves + " moves and  " + time + " seconds!!!!!");
                    Console.WriteLine("");
                    Console.WriteLine("                   Press any key to exit");
                    Console.ReadKey();
                }
                else { GetMove(); }
            }

            void PrintBoard()
            {
                //Function to build and print the empty board.
                Console.Clear();
                Console.WriteLine("\n   ╔═══════╦═══════╦═══════╦═══════╗");
                Console.WriteLine("   ║       ║       ║       ║       ║");

                for (int i = 0; i < 4; i++)
                {
                    if (i > 0) { Console.WriteLine("   ║       ║       ║       ║       ║"); }
                    Console.Write("   ");

                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write("║       ");
                        if (board[i, j] == 0)
                        {
                            blankRow = i;
                            blankCol = j;
                        }
                    }

                    Console.Write("║\n");
                    Console.WriteLine("   ║       ║       ║       ║       ║");

                    if (i < 3) { Console.WriteLine("   ╠═══════╬═══════╬═══════╬═══════╣"); }
                    else { Console.WriteLine("   ╚═══════╩═══════╩═══════╩═══════╝"); }

                }

                Console.WriteLine("\nUse WASD or Arrow Keys to move");
                Console.WriteLine("Move Number: " + moves);
                return;
            }

            string displayValue(int i)
            {
                if (i == 0) { return "  "; }
                if (i < 10) { return " " + i.ToString(); }
                return i.ToString();
            }

        }
    }
}
