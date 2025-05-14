using UnityEngine;

namespace Nolda.GameManager
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager instance;
        void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}