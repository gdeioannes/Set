using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {


    public Text musicText;
    public Text sfxText;
    public static SoundManager _instance;
    private bool sfx = true;
    private bool music= true;


    AudioSource[] sounds;

	// Use this for initialization
	void Start () {
        
        if (_instance == null)
        {
            _instance = this;
        }
        sounds = this.GetComponents<AudioSource>();

        if (PlayerPrefs.HasKey("music"))
        {
            music = PlayerPrefs.GetInt("music") == 1;
            Debug.Log("Music:"+music);
        }

        if (PlayerPrefs.HasKey("sfx"))
        {
            sfx = PlayerPrefs.GetInt("sfx") == 1;
            Debug.Log("SFX:" + sfx);
        }

        playMusic();
        setMusicText();
        changeSFXText();
    }

    public void toggleMusic() {
        music = toggleBool(music);
        PlayerPrefs.SetInt("music", music ? 1 : 0);
        PlayerPrefs.Save();
        setMusicText();
        playMusic();
    }

    private void setMusicText()
    {
        if (music)
        {
            musicText.text = "Music ON";
        }
        else
        {
            musicText.text = "Music OFF";
        }
    }

    public void toggleSFX() {
        sfx =toggleBool(sfx);
        PlayerPrefs.SetInt("sfx", sfx ? 1 : 0);
        PlayerPrefs.Save();
        changeSFXText();
    }

    private void changeSFXText()
    {
        if (sfx)
        {
            sfxText.text = "SFX ON";
        }
        else
        {
            sfxText.text = "SFX OFF";
        }
    }

    public bool toggleBool(bool val) {
        if (val)
        {
            return false;
        }
        return true;
    }

    public void playMusic()
    {
        if (music) { 
            sounds[0].Play();
        }
        else
        {
            sounds[0].Stop();
        }
    }

    private void playSFX(AudioSource audio)
    {
        if (sfx) { 
         audio.Play();
        }
    }

    public void playCardFlip()
    {
        playSFX(sounds[1]);
    }

    public void playPickCard()
    {
        playSFX(sounds[2]);
    }

    public void playNoSet()
    {
        playSFX(sounds[3]);
    }

    public void playLeaveCard()
    {
        playSFX(sounds[4]);
    }

    public void playSet()
    {
        playSFX(sounds[5]);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
