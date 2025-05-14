using TMPro;
using UnityEngine;

namespace Nolda.Collectibles
{
    public class FigureCollection : MonoBehaviour
    {
        public int collectibleCount = 0;
        private int collectibleRemaining = 15;

        public int totalCollectibles;
        public GameObject winScreen;

        public TextMeshProUGUI collectibleText;
        public TextMeshProUGUI collectibleRemainText;

        void Start()
        {
            winScreen.gameObject.SetActive(false);
        }
        void Update()
        {
            if(collectibleCount == totalCollectibles) 
            {
                winScreen.SetActive(true);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "Collectible")
            {
                collectibleCount++;
                collectibleRemaining--;
                collectibleText.text = "Toys: " + collectibleCount.ToString();

                collectibleRemainText.text = "Toys Remaining: " + collectibleRemaining.ToString();
                Destroy(other.gameObject);
            }
        }

        
    }
}
