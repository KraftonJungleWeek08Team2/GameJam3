using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Tooltip("효과음 사운드 소스")]
    [SerializeField] private AudioSource soundEffectudioSource;

    [Header("리듬 게임 사운드")]
    [Tooltip("퍼펙트 사운드")]
    [SerializeField] private AudioClip perfectSound;
    [Tooltip("퍼펙트 사운드 볼륨")]
    [SerializeField] private float perfectSoundVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlayPerfectSound()
    {
        PlayerSoundEffect(perfectSound, perfectSoundVolume);
    }

    private void PlayerSoundEffect(AudioClip clip, float volume)
    {
        if (clip != null && soundEffectudioSource != null)
        {
            soundEffectudioSource.PlayOneShot(clip, volume);
        }
    }
}
