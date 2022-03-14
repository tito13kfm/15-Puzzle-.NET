﻿using System;
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

            int blankRow = 0, blankCol = 0, moves = 0;
            var startTime = DateTime.Now;
            Random random = new Random();
            Console.Title = "15 Puzzle";
            InitBoard();
            GetMove();

            void CheckWin()
            {
                int correct = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (board[i, j] == winningBoard[i, j])
                        {
                            correct++;
                        }
                    }
                }
                if (correct == 16)
                {
                    Console.WriteLine("You Won in " + moves + " moves!!!!!");
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                }
                else { GetMove(); }
            }

            void InitBoard()
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int k = random.Next(4);
                        int l = random.Next(4);
                        int tmp = board[i, j];
                        board[i, j] = board[k, l];
                        board[k, l] = tmp;
                    }
                }
            }

            void PrintBoard()
            {
                Console.Clear();
                Console.WriteLine("\n   ╔═══════╦═══════╦═══════╦═══════╗");
                Console.WriteLine("   ║       ║       ║       ║       ║");

                for (int i = 0; i < 4; i++)
                {
                    if (i > 0) { Console.WriteLine("   ║       ║       ║       ║       ║"); }
                    Console.Write("   ");

                    for (int j = 0; j < 4; j++)
                    {
                        Console.Write("║  " + displayValue(board[i, j]) + "   ");
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

            void GetMove()
            {
                PrintBoard();
                int targetRow = 0;
                int targetCol = 0;
                ConsoleKeyInfo move = Console.ReadKey();

                switch (move.Key)
                {
                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        targetRow = blankRow + 1;
                        targetCol = blankCol;
                        moves++;
                        break;
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        targetRow = blankRow;
                        targetCol = blankCol + 1;
                        moves++;
                        break;
                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        targetRow = blankRow - 1;
                        targetCol = blankCol;
                        moves++;
                        break;
                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        targetRow = blankRow;
                        targetCol = blankCol - 1;
                        moves++;
                        break;
                    default:
                        targetRow = blankRow;
                        targetCol = blankCol;
                        break;
                }

                var aTimer = DateTime.Now - startTime;
                string time = aTimer.TotalSeconds.ToString("0.0");
                Console.Title = "15 Puzzle - " + time + " seconds elapsed";
                if (targetRow < 0 || targetCol < 0) { GetMove(); }
                if (targetRow > 3 || targetCol > 3) { GetMove(); }
                int piece = board[targetRow, targetCol];
                board[blankRow, blankCol] = piece;
                board[targetRow, targetCol] = 0;
                CheckWin();

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
