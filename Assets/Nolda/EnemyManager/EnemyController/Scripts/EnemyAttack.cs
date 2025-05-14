using UnityEngine;
using Nolda.GameManager;

namespace Nolda.EnemyManager
{
    
    public class EnemyAttack : MonoBehaviour
    {
        public PlayerHealth playerHealth;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                playerHealth.TakeDamage(1);
            }
        }
    }
}
