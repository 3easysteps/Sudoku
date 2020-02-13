using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sudoku.UI
{
    public class Digit : MonoBehaviour
    {
        public Text text;

        public int value
        {
            get
            {
                return int.Parse(text.text);
            }
            set
            {
                text.text = $"{value}";
            }
        }
    }
}