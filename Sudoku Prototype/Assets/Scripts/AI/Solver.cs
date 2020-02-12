using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.AI
{
    using Layout;
    using System.Linq;

    public class Solver : MonoBehaviour
    {
        public LayoutManager LayoutManager;

        private void Start()
        {
            StartSolving();
        }

        private void StartSolving()
        {
            StartCoroutine(Solve());
        }

        private IEnumerator Solve()
        {
            yield return null;

            while (FindObjectOfType<GameStateManager>().success == false)
            {
                for (int g = 0; g < 10; g++)
                {
                    foreach (var gridPoint in LayoutManager.grid)
                    {
                        yield return new WaitForEndOfFrame();

                        var numbersTakenHorizontally = LayoutManager.ScanHorizontal(gridPoint.index.y);
                        var numbersTakenVertically = LayoutManager.ScanVertical(gridPoint.index.x);
                        var cellNumbers = LayoutManager.CheckCell(gridPoint.transform.parent);

                        var possibleDigits = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                        foreach (var item in numbersTakenHorizontally)
                            possibleDigits.Remove(item);

                        foreach (var item in numbersTakenVertically)
                            possibleDigits.Remove(item);

                        foreach (var item in cellNumbers)
                            possibleDigits.Remove(item);

                        var val = 0;

                        if (gridPoint.notConflicting == false || gridPoint.value == 0)
                        {
                            if (possibleDigits.Count > 0)
                            {
                                val = possibleDigits[Random.Range(0, possibleDigits.Count - 1)];
                            }
                            else
                            {
                                val += Random.Range(0, 1);
                            }
                        }

                        for (int i = 0; i < val; i++)
                            gridPoint.IncrementValue();

                        yield return new WaitForEndOfFrame();
                    }

                    foreach (var gridPoint in LayoutManager.grid)
                    {
                        var val = 0;

                        if (gridPoint.notConflicting == false)
                        {
                            val += 10 - gridPoint.value;
                        }

                        for (int i = 0; i < val; i++)
                            gridPoint.IncrementValue();
                    }

                    foreach (var gridPoint in LayoutManager.grid)
                    {
                        yield return new WaitForEndOfFrame();

                        var numbersTakenHorizontally = LayoutManager.ScanHorizontal(gridPoint.index.y);
                        var numbersTakenVertically = LayoutManager.ScanVertical(gridPoint.index.x);
                        var cellNumbers = LayoutManager.CheckCell(gridPoint.transform.parent);

                        var possibleDigits = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                        foreach (var item in numbersTakenHorizontally)
                            possibleDigits.Remove(item);

                        foreach (var item in numbersTakenVertically)
                            possibleDigits.Remove(item);

                        foreach (var item in cellNumbers)
                            possibleDigits.Remove(item);

                        var val = 0;

                        if (gridPoint.notConflicting == false || gridPoint.value == 0)
                        {
                            if (possibleDigits.Count > 0)
                            {
                                val = possibleDigits[Random.Range(0, possibleDigits.Count - 1)];
                            }
                            else
                            {
                                val += Random.Range(0, 1);
                            }
                        }

                        for (int i = 0; i < val; i++)
                            gridPoint.IncrementValue();

                        yield return new WaitForEndOfFrame();
                    }

                    yield return null;
                }

                yield return null;
            }
        }
    }
}