using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sudoku.Layout
{
    public class LayoutManager : MonoBehaviour
    {
        [Tooltip("The seed for this layout.")]
        public int seed = 0;

        private System.Random random;

        [Space]
        [Tooltip("The objects the sudoku grid will be tied to.")]
        public Transform[] layoutObjects;

        [Space]
        [Tooltip("The sudoku clickable object.")]
        public GameObject prefab;

        public LayoutObject[,] grid;

        internal Vector2Int size = Vector2Int.one * 9;

        public int hintsPerBlock = 2;

        public bool LevelReady { get; internal set; }

        private void Start()
        {
            random = new System.Random(seed);

            StartCoroutine(GeneratePuzzle());
        }

        private IEnumerator GeneratePuzzle()
        {
            LevelReady = false;

            PopulateObjects();

            while (grid.GetLength(0) < size.x)
                yield return null;

            PopulatePuzzle();

            LevelReady = true;
        }

        /// <summary>
        /// Create the grid.
        /// </summary>
        private void PopulateObjects()
        {
            // Create an empty grid with the length we desire.
            grid = new LayoutObject[size.x, size.y];

            // Create some temporary veriables so we can loop around the grid
            int startX = 0;
            int startY = 0;

            // Loop through the layout objects
            foreach (var layoutObject in layoutObjects)
            {
                // Create a button for each point on this layout object
                for (int x = startX; x < startX + (size.x / 3); x++)
                {
                    for (int y = startY; y < startY + (size.y / 3); y++)
                    {
                        // instantiate the button
                        var go = Instantiate(prefab, layoutObject);
                        go.name = $"GridPoint {x}, {y}"; // lable the button for debugging

                        // Set the object in our grid
                        grid[x, y] = go.GetComponent<LayoutObject>();
                        grid[x, y].index = new Vector2Int(x, y); // Tell the object which grid position it is in.
                    }
                }

                // Loop around the grid
                startX += size.x / 3;
                if (startX >= size.x)
                {
                    startX = 0;
                    startY += size.y / 3;
                }
            }
        }

        /// <summary>
        /// Create the puzzle
        /// </summary>
        private void PopulatePuzzle()
        {
            // Create some temporary veriables so we can loop around the grid
            int startX = 0;
            int startY = 0;

            // Loop through the layout objects
            foreach (var layoutObject in layoutObjects)
            {
                int hintsThisBlock = 0;

                var validDigits = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                // Create a button for each point on this layout object
                for (int x = startX; x < startX + (size.x / 3); x++)
                {
                    for (int y = startY; y < startY + (size.y / 3); y++)
                    {
                        // Generate a number here
                        var number = validDigits[random.Next(0, validDigits.Count)]; // Get one from the valid digits.
                        var numbersTakenHorizontally = ScanHorizontal(y); // Get the horizontal number that have already been placed
                        var numbersTakenVertically = ScanVertical(x); // Get the vertical number that have already been placed
                        var cellNumbers = CheckCell(layoutObject);

                        // Check if we have already tried placing this number in this column/row/cell
                        bool takenX = numbersTakenHorizontally.Contains(number);
                        bool takenY = numbersTakenHorizontally.Contains(number);
                        bool takenCell = cellNumbers.Contains(number);

                        if (takenX || takenY || takenCell)
                        {
                            var possibleDigits = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                            // Adjust accordingly
                            int val = -1;

                            foreach (var item in numbersTakenHorizontally)
                                possibleDigits.Remove(item);

                            foreach (var item in numbersTakenVertically)
                                possibleDigits.Remove(item);

                            foreach (var item in cellNumbers)
                                possibleDigits.Remove(item);

                            val = possibleDigits.FirstOrDefault();

                            number = val;
                        }

                        bool hide = random.Next(0, 100) > 50 / (hintsThisBlock + 1);

                        grid[x, y].SetValue(number, hide);

                        if (hintsThisBlock <= hintsPerBlock && hide == false)
                            hintsThisBlock++;

                        validDigits.Remove(number);
                    }
                }

                // Loop around the grid
                startX += size.x / 3;
                if (startX >= size.x)
                {
                    startX = 0;
                    startY += size.y / 3;
                }
            }
        }

        public bool Valid(int value, int x, int y)
        {
            int containingCount = 0;

            var vals = CheckCell(grid[x, y].transform.parent);

            for (int i = 0; i < vals.Length; i++)
            {
                if (vals[i] == value) containingCount++;
            }

            return CheckLine(value, x, y) == false && containingCount <= 1;
        }

        /// <summary>
        /// Checks a specified row and column, and if either the row or column already contains that digit, it will return true.
        /// </summary>
        /// <param name="number">The digit to check for.</param>
        /// <param name="row">The row to check in.</param>
        /// <param name="column">The column to check in.</param>
        /// <returns>If row/column already contains this value.</returns>
        public bool CheckLine(int number, int row, int column)
        {
            var sender = grid[row, column];

            // Check row
            for (int x = 0; x < size.x; x++)
            {
                var currentIndex = grid[x, column];

                if (currentIndex.value == number && currentIndex != sender)
                {
                    // There are two numbers of the same type in this row
                    return true;
                }
            }

            // Check column
            for (int y = 0; y < size.y; y++)
            {
                var currentIndex = grid[row, y];

                if (currentIndex.value == number && currentIndex != sender)
                {
                    // There are two numbers of the same type in this column
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the values from a cell.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        public int[] CheckCell(Transform cell)
        {
            var numbers = new List<int>();

            foreach (var item in cell.GetComponentsInChildren<LayoutObject>())
                numbers.Add(item.value);

            return numbers.ToArray();
        }

        /// <summary>
        /// Grabs all the number from a horizontal slice.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public int[] ScanHorizontal(int column)
        {
            var numbersInRow = new List<int>();

            // Check row
            for (int x = 0; x < size.x; x++)
            {
                var number = grid[x, column].value;

                numbersInRow.Add(number);
            }

            return numbersInRow.ToArray();
        }

        /// <summary>
        /// Grabs all the number from a vertical slice.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public int[] ScanVertical(int row)
        {
            var numbersInColumn = new List<int>();

            // Check column
            for (int y = 0; y < size.x; y++)
            {
                var number = grid[row, y].value;

                numbersInColumn.Add(number);
            }

            return numbersInColumn.ToArray();
        }

        /// <summary>
        /// Checks the grid to ensure all digits are in the correct placement.
        /// </summary>
        /// <returns></returns>
        public bool CheckForSolved()
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var value = grid[x, y].value;

                    if (CheckLine(value, x, y) || value == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}