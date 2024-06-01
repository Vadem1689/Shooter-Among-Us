using UnityEngine;

namespace BRAmongUS.Utils.Singleton
{
    public abstract class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;
        
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}