using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.Layout
{
    public class LayoutGenerator : MonoBehaviour
    {
        public int[,] grid;

        private System.Random random;

        internal bool done = false;

        public void Generate(System.Random random)
        {
            // Start a new grid
            grid = GetBaseGrid();

            var digits = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Shuffle grid based on random Seed
            for (int i = digits.Count - 1; i > 0; i--)
            {
                // decide wether or not to swap this number
                bool swap = random.Next(0, 50) > 25;

                if (swap == false || i >= digits.Count) continue;

                int toSwapA = digits[i];
                int toSwapB = digits[random.Next(0, digits.Count)];

                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.GetLength(1); y++)
                    {
                        if (grid[x, y] == toSwapA)
                        {
                            grid[x, y] = toSwapB;
                            continue;
                        }

                        if (grid[x, y] == toSwapB)
                            grid[x, y] = toSwapA;
                    }
                }

                digits.Remove(toSwapA);
                digits.Remove(toSwapB);
            }

            // Swap around some of the rows
            for (int i = 0; i < 3; i++)
            {
                // decide wether or not to swap this number
                bool swapX = random.Next(0, 50) > 25;
                bool swapY = random.Next(0, 50) < 25;

                if (swapX)
                {
                    var xValA = i * 3;
                    var xValB = xValA + 1;

                    SwapRows(xValA, xValB);
                }
                if (swapY)
                {
                    var yValA = i * 3;
                    var yValB = yValA + 1;

                    SwapColumns(yValA, yValB);
                }
            }

            done = true;
        }

        private void SwapRows(int rowA, int rowB)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var left = grid[rowA, y];

                grid[rowA, y] = grid[rowB, y];
                grid[rowB, y] = left;
            }
        }

        private void SwapColumns(int columnA, int columnB)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var left = grid[columnA, y];

                grid[columnA, y] = grid[columnB, y];
                grid[columnB, y] = left;
            }
        }

        /// <summary>
        /// Returns a solved grid
        /// </summary>
        /// <returns></returns>
        private int[,] GetBaseGrid()
        {
            var t = new int[,]
            {
                { 7, 3, 5,   6, 1, 4,   8, 9, 2 },
                { 8, 4, 2,   9, 7, 3,   5, 6, 1 },
                { 9, 6, 1,   2, 8, 5,   3, 7, 4 },

                { 2, 8, 6,   3, 4, 9,   1, 5, 7 },
                { 4, 1, 3,   8, 5, 7,   9, 2, 6 },
                { 5, 7, 9,   1, 2, 6,   4, 3, 8 },

                { 1, 5, 7,   4, 9, 2,   6, 8, 3 },
                { 6, 9, 4,   7, 3, 8,   2, 1, 5 },
                { 3, 2, 8,   5, 6, 1,   7, 4, 9 },
            };

            return t;
        }
    }
}