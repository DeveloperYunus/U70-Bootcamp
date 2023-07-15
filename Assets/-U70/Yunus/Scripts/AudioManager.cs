using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager ins;
    [HideInInspector] public float currentVolume;       //bu her ses ayar�nda g�ncellensin

    public static bool inCave;                          //magaran�n i�indemiyiz d���ndam�y�z
    [HideInInspector] public float inCaveTimer;         //magaraya girince hesaplamalar� ks�tl� s�reli�ine yapmas� i�in s�re sayac�
    float bgTimer;                                      //background m�ziklerinin timer k�sm� sayac� 
    float inCaveSound;
    string currentBGMelody;


    void Awake()
    {
        /*if (instance == null)                         
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);      */

        ins = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * currentVolume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        currentVolume = PlayerPrefs.GetFloat("soundVolume", 0.5f);

        inCaveSound = 0;
        inCaveTimer = 0;
    }
    private void Update()
    {
        if (bgTimer < 0)
        {
            int a = UnityEngine.Random.Range(0,2);
            if (a == 0)
            {
                bgTimer = GetClip("Forest1").length;
                PlaySound("Forest1");
               currentBGMelody = "Forest1";
            }
            else
            {
                bgTimer = GetClip("Forest2").length;
                PlaySound("Forest2");
                currentBGMelody = "Forest2";
            }
        }
        else 
            bgTimer -= Time.deltaTime;

        if (inCaveTimer > 0)
        {
            inCaveTimer -= Time.deltaTime;

            if (inCave)
            {
                inCaveSound = Mathf.Lerp(inCaveSound, 1, 0.06f);
                PlaySoundOne("Cave");

                SetSound(currentBGMelody, (1 - inCaveSound) * currentVolume);             //mevcut bg yi s�f�ra do�ru g�t�r
                SetSound("Cave", inCaveSound * currentVolume);
            }
            else
            {
                inCaveSound = Mathf.Lerp(inCaveSound, 0, 0.06f);

                SetSound(currentBGMelody, (1 - inCaveSound) * currentVolume);
                SetSound("Cave", inCaveSound * currentVolume);
            }
        }
    }
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //Oyun arka planda �al���yor.
            AudioListener.pause = true;
        }
        else
        {
            //Oyun �n planda �al���yor
            AudioListener.pause = false;
        }
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);       
        if (s == null) return;                                                      //e�er o isimde bir ses dosyas� yoksa return et
        SetSound(name, currentVolume);
        s.source.Play();
    }
    /// <summary>   Hali haz�rda �almakta olan seslei cald�rmaz.   </summary>
    public void PlaySoundOne(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;

        if (!s.source.isPlaying)                                                    //e�er bu ses �alm�yorsa bu sesi �al
        {   
            SetSound(name, currentVolume);
            s.source.Play();
        }
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);                  
        if (s == null) return;                                                      
        s.source.Stop();
    }
    public void SetSound(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.volume = volume * s.volume;
    }
    public AudioClip GetClip(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return null;
        return s.clip;
    }

    public void SetGV(float gv)
    {
        currentVolume = gv;                                                       //   0 < gv < 1
        PlayerPrefs.SetFloat("soundLevel", currentVolume);

        SetSound(currentBGMelody, (1 - inCaveSound) * currentVolume);             //mevcut bg yi s�f�ra do�ru g�t�r
        SetSound("Cave", inCaveSound * currentVolume);
    }
}



[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}