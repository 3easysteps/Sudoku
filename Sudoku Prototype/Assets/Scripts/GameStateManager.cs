using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku
{
    public class GameStateManager : MonoBehaviour
    {
        public bool success;

        public Layout.LayoutManager LayoutManager;

        private void Update()
        {
            success = LayoutManager.CheckForSolved();
        }
    }
}