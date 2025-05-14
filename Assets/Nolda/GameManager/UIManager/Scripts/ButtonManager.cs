using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nolda.GameManager
{
    public class ButtonManager : MonoBehaviour
    {
        void Start()
        {
            Time.timeScale = 1f;
        }
        public void QuitGame()
        {
            Application.Quit();
        }

        public void ResetGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
        }
    }

}