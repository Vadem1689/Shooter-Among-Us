using System.Collections;
using BRAmongUS.Utils;
using BRAmongUS.Utils.Singleton;
using UnityEngine;
using YG;

namespace BRAmongUS.Audio
{
    public enum ESoundType
    {
        ButtonClick,
        EnemyDeath,
        PlayerDeath,
        PickupHeal,
        PickupShield,
        PickupCoin,
        BackgroundMusic,
    }

    public sealed class AudioController : SingletonDontDestroy<AudioController>
    {
        [SerializeField] private VInspector.SerializedDictionary<ESoundType, AudioSource> sounds;
        [SerializeField] private AudioClip[] backgroundMusicClips;
        
        [SerializeField] private float timeBeforeInit = 0.3f;
        [SerializeField] private float timeBetweenBackgroundMusic = 0.3f;
        [SerializeField, Range(0, 1)] private float backgroundMusicVolume = 0.3f;

        private int currentBackgroundMusicIndex = -1;
        
        private AudioSource backgroundMusic;
        private Coroutine backgroundMusicCoroutine;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(timeBeforeInit);
            MuteAudioListener(YandexGame.savesData.isAudioMuted);
            backgroundMusicCoroutine = StartCoroutine(PlayBackgroundMusicCoroutine());
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if(backgroundMusic == null) return;
            if(hasFocus)
            {
                StopCoroutine(backgroundMusicCoroutine);
                backgroundMusicCoroutine = StartCoroutine(PlayBackgroundMusicCoroutine());
            }
            else
                backgroundMusic.Pause();
        }

        private void OnMuteAudio(bool isMuted)
        {
            MuteAudioListener(isMuted);
            SavingUtils.SetBool(ESaveType.IsAudioMuted, isMuted);
        }

        private void MuteAudioListener(bool isMuted)
        {
            AudioListener.volume = isMuted ? 0 : 1;
        }

        public void MuteAudio(bool isMuted)
        {
            OnMuteAudio(isMuted);
        }
        
        public void PlaySound(in ESoundType soundType)
        {
            if (YandexGame.savesData.isAudioMuted) return;
            Play(sounds[soundType]);
        }

        private void Play(in AudioSource audioSource)
        {
            var instance = Instantiate(audioSource);
            Destroy(instance.gameObject, audioSource.clip.length);
        }

        private IEnumerator PlayBackgroundMusicCoroutine()
        {
            float musicLength;
            float time;
            
            while (true)
            {
                currentBackgroundMusicIndex = currentBackgroundMusicIndex == 0 ? 1 : 0;
                backgroundMusic = Instantiate(sounds[ESoundType.BackgroundMusic], transform);
                backgroundMusic.clip = backgroundMusicClips[currentBackgroundMusicIndex];
                backgroundMusic.volume = backgroundMusicVolume;
                musicLength = backgroundMusic.clip.length;
                time = 0;
                
                backgroundMusic.Play();
                while (time < musicLength)
                {
                    time += Time.deltaTime;
                    yield return null;
                }
                
                Destroy(backgroundMusic.gameObject);
                yield return new WaitForSeconds(timeBetweenBackgroundMusic);
            }
        }
    }
}