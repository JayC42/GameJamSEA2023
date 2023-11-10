using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource BossSFX;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip attack;
    public AudioClip enemyHurt;
    public AudioClip bossAttack;
    public AudioClip bossDead;
    public AudioClip bossHurt;
    public AudioClip itemBreak;
    public AudioClip pickItem;

    public static AudioManager instance;

    private void Awake()
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

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlayerAttack()
    {
        SFXSource.clip = attack;
        SFXSource.Play();
    }

    public void EnemyHurt()
    {
        SFXSource.clip = enemyHurt;
        SFXSource.Play();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void BossAttack()
    {
        BossSFX.clip = bossAttack;
        BossSFX.Play();
    }

    public void BossDie()
    {
        BossSFX.clip = bossDead;
        BossSFX.Play();
    }

    public void BossHurt()
    {
        BossSFX.clip = bossHurt;
        BossSFX.Play();
    }

    public void ItemBreak()
    {
        SFXSource.clip = itemBreak;
        SFXSource.Play();
    }

    public void PickItem()
    {
        SFXSource.clip = pickItem;
        SFXSource.Play();
    }
}
