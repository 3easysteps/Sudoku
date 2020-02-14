using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sudoku.UI
{
    using Layout;

    public class DigitMovementManager : MonoBehaviour
    {
        public static int indexHolding;

        public Button[] indexes;

        private LayoutManager layoutManager;

        private void Start()
        {
            layoutManager = FindObjectOfType<LayoutManager>();

            SetDigit(indexHolding);
        }

        private void Update()
        {
            layoutManager.HighLightTilesWithDigit(indexHolding);
        }

        public void SetDigit(int index)
        {
            indexHolding = index;

            UpdateSelected(index);
        }

        private void UpdateSelected(int index)
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                if (i == index)
                {
                    indexes[i].interactable = false;
                }
                else
                {
                    indexes[i].interactable = true;
                }
            }
        }
    }
}