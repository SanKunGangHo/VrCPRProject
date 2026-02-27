using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _02.Scripts.Manager
{
    public class SoundManager_ForStart : MonoBehaviour
    {
        public enum AudioType
        {
            Bgm,
            Na,
            Sfx
        }

        public AudioSource bgmSource;         // 배경음악 재생용 AudioSource
        public AudioSource naSource;          // 나래이션  재생용 AudioSource
        public AudioSource sfxSource;         // 사운드 이펙트 재생용 AudioSource

        public AudioClip[] bgmClips;          // 배경음악 클립 배열
        public AudioClip[] naClips;          // 나래이션 클립 배열
        public AudioClip[] sfxClips;          // 사운드 이펙트 클립 배열

        public string madeInScene;

        // Start is called before the first frame update
        void Start()
        {
            if (SceneManager.GetActiveScene().name == "StartScene_Multi" || SceneManager.GetActiveScene().name == "StartScene")
            {
                PlayBGM(0);
                madeInScene = "StartScene";
            }
            //PlayBGM(0);
            //PlaySFX(0);
        }

        public void PlayBGM(int _idx)
        {
            if (_idx < bgmClips.Length)
            {
                bgmSource.clip = bgmClips[_idx];
                bgmSource.Play();
            }
        }

        public void PlayNa(int _idx)
        {
            if (_idx < naClips.Length)
            {
                naSource.clip = naClips[_idx];
                naSource.Play();
            }
        }

        private IEnumerator PlayNa(AudioType _at ,int _idx, Action onComplete)
        {

            AudioSource selectedSource = null;
            AudioClip[] selectClips = null;

            switch (_at)
            {

                case AudioType.Bgm:
                    selectedSource = bgmSource;
                    selectClips = bgmClips;
                    break;
                case AudioType.Na:
                    selectedSource = naSource;
                    selectClips = naClips;
                    break;
                case AudioType.Sfx:
                    selectedSource = sfxSource;
                    selectClips = sfxClips;
                    break;
                default:
                    break;
            }

            if (_idx < selectClips.Length && selectClips[_idx] != null)
            {
                selectedSource.clip = naClips[_idx];
                selectedSource.Play();

                // 오디오가 끝날 때까지 대기
                while (selectedSource.isPlaying)
                {
                    yield return null; // 다음 프레임까지 대기
                }

                // 오디오가 종료되면 콜백 호출
                onComplete?.Invoke();
            }
            else
            {
                Debug.LogWarning("유효하지 않은 클립 인덱스이거나 클립이 없습니다.");
            }
        }

        public void PlayNaCoroutine(int _audioType, int _idx, Action onComplete)
        {
            StartCoroutine(PlayNa((AudioType)_audioType, _idx, onComplete));
        }

        // 사운드 이펙트 재생
        public void PlaySFX(int _idx)
        {
            Debug.LogWarning("SFX"+_idx);
            if (_idx < sfxClips.Length)
            {
                sfxSource.PlayOneShot(sfxClips[_idx]);
            }
        }

        // 모든 사운드 중지
        public void StopAllSound()
        {
            bgmSource.Stop();
            sfxSource.Stop();
        }
        public void SetVolume(float _bgmVolume, float _sfxVolume)
        {
            bgmSource.volume = _bgmVolume;
            sfxSource.volume = _sfxVolume;
        }

        public IEnumerator FadeOutBGM(float _duration)
        {
            float startVolume = bgmSource.volume;

            while (bgmSource.volume > 0)
            {
                bgmSource.volume -= startVolume * Time.deltaTime / _duration;
                yield return null;
            }

            bgmSource.Stop();
            bgmSource.volume = startVolume;  // 원래 볼륨 복구
        }

        public IEnumerator FadeInBGM(int _idx, float _duration)
        {
            bgmSource.clip = bgmClips[_idx];
            bgmSource.volume = 0;
            bgmSource.Play();

            while (bgmSource.volume < 1)
            {
                bgmSource.volume += Time.deltaTime / _duration;
                yield return null;
            }
        }
    }
}
