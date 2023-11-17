using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip BGM;
    public AudioClip[] sfxClips;  

    public static AudioManager instance;

    private void Start()
    {
        musicSource.clip = BGM;
        musicSource.Play();
    }

    // Method to play a single SFX normally
    public void PlaySingleSFX(AudioClip sfxClip)
    {
        SFXSource.PlayOneShot(sfxClip);
    }

    // Method to play a random SFX from a list
    public void PlayRandomSFX()
    {
        if (sfxClips.Length > 0)
        {
            int randomIndex = Random.Range(0, sfxClips.Length);
            SFXSource.PlayOneShot(sfxClips[randomIndex]);
        }
        else
        {
            Debug.LogWarning("No SFX clips assigned to the AudioManager.");
        }
    }
}
