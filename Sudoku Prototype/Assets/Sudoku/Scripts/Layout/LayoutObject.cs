using Sudoku.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Sudoku.Layout
{
    public class LayoutObject : MonoBehaviour
    {
        internal bool notConflicting;
        internal bool set;

        [Tooltip("This object will display the color.")]
        public Graphic ColorDisplay;

        // These are separate so you can use either the text or the background graphic.

        [Tooltip("This object will display the current digit.")]
        public Text textObject;

        public Color PlayerPlaced = Color.gray,
            Correct = Color.green,
            Wrong = Color.red,
            HighLight = Color.cyan;

        internal bool highLighting = false;

        public int value = 0;

        // You could use this to give the user a hint if need be
        [HideInInspector]
        public int trueValue = 0;

        [HideInInspector]
        public Vector2Int index;

        private LayoutManager layoutManager;

        private void Start()
        {
            layoutManager = FindObjectOfType<LayoutManager>();
        }

        /// <summary>
        /// Update the text
        /// </summary>
        private void Update()
        {
            if (layoutManager.LevelReady == false) return;

            notConflicting = layoutManager.Valid(value, index.x, index.y);

            if (notConflicting)
            {
                ColorDisplay.color = highLighting ? HighLight : (set ? Correct : PlayerPlaced);
            }
            else
            {
                ColorDisplay.color = Wrong;
            }

            if (set) return;

            value %= 10;

            textObject.text = value > 0 ? value.ToString() : "";
        }

        public void SetValue(int val, bool hide)
        {
            trueValue = val;

            set = !hide;

            if (hide == false)
            {
                value = trueValue;
            }

            ColorDisplay.color = Correct;

            textObject.text = value > 0 ? value.ToString() : "";
        }

        public void GetDigit()
        {
            if (!set)
                value = DigitMovementManager.indexHolding;
        }
    }
}