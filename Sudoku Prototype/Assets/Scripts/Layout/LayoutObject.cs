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

        public Text textObject;

        public Color PlayerPlaced = Color.gray,
            Correct = Color.green,
            Wrong = Color.red,
            Placed = Color.black;

        public int value = 0;
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
                textObject.color = Correct;
            }
            else
            {
                textObject.color = Wrong;
            }
        }

        public void IncrementValue()
        {
            if (set) return;

            value++;
            value %= 10;

            textObject.color = PlayerPlaced;

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

            textObject.color = Placed;

            textObject.text = value > 0 ? value.ToString() : "";
        }
    }
}