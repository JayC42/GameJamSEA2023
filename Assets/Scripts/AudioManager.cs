using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip BGM;

    public static AudioManager instance;

    private void Start()
    {
        musicSource.clip = BGM;
        musicSource.Play();
    }
}
