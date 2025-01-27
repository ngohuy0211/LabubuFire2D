using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Singleton

    private static SoundManager _instance;

    public static SoundManager GetInstance()
    {
        if (_instance == null) _instance = new SoundManager();

        return _instance;
    }
    
    #endregion

    #region Define

    [SerializeField] private AudioSource mAudioSource;

    #endregion

    #region Properties

    private BackgroundMusicType _backgroundType = BackgroundMusicType.None;
    private AudioClip _audioClipLogin, _audioClipScene;
    private bool _backgroundSoundPlaying = false;

    #endregion

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);
    }
    
    //
    private Hashtable _htbAudio = new Hashtable();


    #region SETTING

    public void SetVolume(float volume)
    {
        mAudioSource.volume = volume;
    }

    public float GetVolume()
    {
        return mAudioSource.volume;
    }

    #endregion

    #region PLAY
    
    public void PlaySound(string soundId)
    {
        if (!GameContext.Instance.SoundOn) return;

        if (mAudioSource == null)
            return;
        if (soundId.Length <= 0)
            return;
        AudioClip clip = (AudioClip) _htbAudio[soundId];
        if (clip == null)
        {
            clip = LoadAudioClip(soundId);
            if (clip != null) _htbAudio.Add(soundId, clip);
        }

        //
        if (clip != null) mAudioSource.PlayOneShot(clip);
        else Debug.Log("------- NULL sound " + soundId);
    }
    
    public void PlayStoryMusic()
    {
        if (!GameContext.Instance.MusicOn) return;

        if (mAudioSource == null) return;

        AudioClip audioClipStory = Resources.Load<AudioClip>("");
        mAudioSource.clip = audioClipStory;
        _backgroundType = BackgroundMusicType.MusicStory;
        //
        mAudioSource.loop = true;
        mAudioSource.Play();
    }
    
    public void PlayBackgroundMusicScene()
    {
        if (_backgroundType == BackgroundMusicType.MusicStory)
            return;

        if (!GameContext.Instance.MusicOn)
            return;
        
        if (mAudioSource == null)
            return;
        
        if (_backgroundSoundPlaying && _backgroundType == BackgroundMusicType.MusicScene)
            return;
        
        if (GameContext.Instance.CurrentScene is LoginScene)
        {
            _audioClipLogin = ResourceHelper.LoadSound("");
            mAudioSource.clip = _audioClipLogin;
            _backgroundType = BackgroundMusicType.MusicLogin;
        }
        else
        {
            if (_audioClipScene == null)
                _audioClipScene = ResourceHelper.LoadSound("");
            mAudioSource.clip = _audioClipScene;
            _backgroundType = BackgroundMusicType.MusicScene;
            //
            if (_audioClipScene != null)
            {
                Resources.UnloadAsset(_audioClipScene);
                _audioClipScene = null;
            }
        }
        //
        _backgroundSoundPlaying = true;
        mAudioSource.loop = true;
        mAudioSource.Play();
    }

    #endregion

    #region STOP

    public void StopSoundWhenPlayOneShot()
    {
        mAudioSource.Stop();
    }

    public void StopBackgroundMusic()
    {
        if (mAudioSource == null) return;
        _backgroundSoundPlaying = false;
        mAudioSource.Stop();
    }

    #endregion
    
    #region LOAD

    public void LoadBackgroundMusic(string pathSound)
    {
        AudioClip clip = ResourceHelper.LoadSound(pathSound);
        mAudioSource.clip = clip;
    }

    private AudioClip LoadAudioClip(string soundId)
    {
        AudioClip clip = null;
        //
        clip = ResourceHelper.LoadSound(soundId);

        //
        if (clip == null) Debug.Log("--- Error loading sound : " + soundId);

        //
        return clip;
    }

    #endregion
}


public enum BackgroundMusicType : int
{
    None = 0,
    MusicScene = 1,
    MusicStory = 2,
    MusicLogin = 3
}
