using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource BaseMusicIntro;
    [SerializeField] private AudioSource BaseMusic;
    [SerializeField] private AudioSource DangerMusicIntro;
    [SerializeField] private AudioSource DangerMusic;


    [SerializeField] private float MinDistForDangerMusic;

    private EnemySpawner Spawner;

    [SerializeField] private float VolumeChangePerSecond;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayMusic(BaseMusicIntro, BaseMusic, 1f);
        PlayMusic(DangerMusicIntro, DangerMusic, 0f);
        Spawner = EnemySpawner.instance;
    }

    // Update is called once per frame
    void Update()
    {
        ManageMusicRatio();
    }


    private void ManageMusicRatio()
    {
        // the goal here is to blend the two musics together if an enemy is close eneough

        bool EnemyClose = false;
        if (Spawner.DistanceToClosestEnemy > 0 && Spawner.DistanceToClosestEnemy < MinDistForDangerMusic)
        {
            EnemyClose = true;
        }

        if (EnemyClose)
        {
            if (DangerMusic.volume < 1f)
            {
                SetVolume(DangerMusicIntro, DangerMusic, DangerMusic.volume + Time.deltaTime * VolumeChangePerSecond);
            }
            if (DangerMusic.volume > 1f)
            {
                SetVolume(DangerMusicIntro, DangerMusic, 1f);
            }
            if (BaseMusic.volume > 0f)
            {
                SetVolume(BaseMusicIntro, BaseMusic, BaseMusic.volume - Time.deltaTime * VolumeChangePerSecond);
            }
            if (BaseMusic.volume < 0f)
            {
                SetVolume(BaseMusicIntro, BaseMusic, 0f);
            }
        }
        else
        {
            if (BaseMusic.volume < 1f)
            {
                SetVolume(BaseMusicIntro, BaseMusic, BaseMusic.volume + Time.deltaTime * VolumeChangePerSecond);
            }
            if (BaseMusic.volume > 1f)
            {
                SetVolume(BaseMusicIntro, BaseMusic, 1f);
            }
            if (DangerMusic.volume > 0f)
            {
                SetVolume(DangerMusicIntro, DangerMusic, DangerMusic.volume - Time.deltaTime * VolumeChangePerSecond);
            }
            if (BaseMusic.volume < 0f)
            {
                SetVolume(DangerMusicIntro, DangerMusic, 0f);
            }
        }

    }


    private void SetVolume(AudioSource Intro, AudioSource Main, float Volume)
    {
        Intro.volume = Volume;
        Main.volume = Volume;
    }

    // We play the music intro and then schedule the main looping part to play as the intro finishes
    private void PlayMusic(AudioSource Intro, AudioSource Main, float Volume)
    {
        SetVolume(Intro, Main, Volume);
        double dsptime = AudioSettings.dspTime;
        double introduration = (double)Intro.clip.samples / Intro.clip.frequency;

        Intro.PlayScheduled(dsptime);
        Main.PlayScheduled(dsptime + introduration);
    }
}
