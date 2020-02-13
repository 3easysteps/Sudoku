using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sudoku
{
    public class GameStateManager : MonoBehaviour
    {
        public Layout.LayoutManager LayoutManager;

        [HideInInspector]
        public bool success;

        [Space]
        public UnityEngine.Events.UnityEvent onSuccessEvent;

        private float lastInvoke;

        private void Update()
        {
            success = LayoutManager.CheckForSolved();

            // If the game has been launched for a second or two, and the user has completed the puzzle.
            // Send out an on completion event.
            if (Time.timeSinceLevelLoad > 2 && success && Time.time + 10 > lastInvoke)
            {
                onSuccessEvent?.Invoke();
                lastInvoke = Time.time;
            }
        }

        public void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}