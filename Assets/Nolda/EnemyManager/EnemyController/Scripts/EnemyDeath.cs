using UnityEngine;

namespace Nolda.EnemyManager
{
    public class EnemyDeath : MonoBehaviour
    {
        public GameObject enemy;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "PlayerBottom")
            {
                Destroy(enemy);
            }
        }
    }
}