using UnityEngine;

namespace Nolda.GameManager
{
    public class PauseMenu : MonoBehaviour
    {

        public GameObject pauseMenu;
        public bool isPaused;


        void Start()
        {
            pauseMenu.SetActive(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ContinueGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            isPaused = true;
        }

        public void ContinueGame()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
        }
    }
}
