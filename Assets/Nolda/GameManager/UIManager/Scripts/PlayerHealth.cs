using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Nolda.GameManager
{
    
    public class PlayerHealth : MonoBehaviour
    {
        public GameObject gameOverScreen;
        public GameObject player;
        public int health;
        public int numOfHearts;

        public UnityEngine.UI.Image[] hearts;
        public Sprite fullHeart;
        public Sprite emptyHeart;

        void Start()
        {
            gameOverScreen.gameObject.SetActive(false);
        }


        void Update()
        {
            if(health <= 0)
            {
                gameOverScreen.gameObject.SetActive(true);   
            }
            if(health > numOfHearts) health = numOfHearts;
            
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health) hearts[i].sprite = fullHeart;
                else hearts[i].sprite = emptyHeart;

                if (i < numOfHearts) hearts[i].enabled = true;
                else hearts[i].enabled = false;
            }
        }

        public void TakeDamage(int amount){
            health -= amount;
        }
    }

}