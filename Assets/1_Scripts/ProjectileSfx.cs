using UnityEngine;

namespace BRAmongUS.Gun
{
    public sealed class ProjectileSfx : MonoBehaviour
    {
        [SerializeField] private AudioSource hitSoundEffect;
        [SerializeField] private float destroyTime = 2.5f;

        private void OnEnable()
        {
            gameObject.transform.SetParent(null);
            Destroy(gameObject, destroyTime);
        }

        public void PlayHitSoundEffect()
        {
            hitSoundEffect.Play();
        }
    }
}