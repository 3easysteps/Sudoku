using UnityEngine;

namespace Sudoku
{
    public class GameStateManager : MonoBehaviour
    {
        public Layout.LayoutManager LayoutManager;

        [HideInInspector]
        public bool success;

        [Space]
        public UnityEngine.Events.UnityEvent onSuccessEvent;


        private void Update()
        {
            success = LayoutManager.CheckForSolved();

            // If the game has been launched for a second or two, and the user has completed the puzzle.
            // Send out an on completion event.
            if (Time.timeSinceLevelLoad > 2 && success)
            {
                onSuccessEvent?.Invoke();
                onSuccessEvent = null;
            }
        }
    }
}