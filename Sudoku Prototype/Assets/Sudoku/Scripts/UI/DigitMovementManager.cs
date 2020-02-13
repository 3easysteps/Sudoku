using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku.UI
{
    using Layout;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class DigitMovementManager : MonoBehaviour
    {
        public Text displayText;
        public Transform mouseFollowerObject;

        private EventSystem eventSystem;
        private int indexHolding = -1;

        private void Start() => eventSystem = EventSystem.current;

        private void Update()
        {
            var highlighting = eventSystem.currentSelectedGameObject;

            mouseFollowerObject.gameObject.SetActive(indexHolding > -1);
            mouseFollowerObject.position = Input.mousePosition;

            // Check to see if the current item is null or not
            if (highlighting == null) return;

            if (indexHolding > -1)
            {
                // We are holding an index.
                // Set the index text
                displayText.text = $"{indexHolding}";

                // Check if there has been a mouse up event
                if (Input.GetButtonUp("Fire1"))
                {
                    var layoutObject = highlighting.GetComponent<LayoutObject>();

                    // Check if the active object we are mousing over exists
                    if (layoutObject != null && !layoutObject.set)
                    {
                        // Set the value and clear the item holding
                        layoutObject.value = indexHolding;
                    }

                    indexHolding = -1;
                }
            }
            else
            {
                // We are not holding an index.
                // Check if there has been a mouse down event
                if (Input.GetButtonDown("Fire1"))
                {
                    var layoutObject = highlighting.GetComponent<LayoutObject>();

                    // Check if the object we are clicking is an index
                    if (layoutObject != null)
                    {
                        indexHolding = layoutObject.value;

                        if (!layoutObject.set)
                        {
                            layoutObject.value = 0;
                        }
                    }

                    var digit = highlighting.GetComponent<Digit>();

                    // Check if we are highlighting one of the digit objects
                    if (digit != null)
                    {
                        indexHolding = digit.value;
                    }
                }
            }
        }
    }
}