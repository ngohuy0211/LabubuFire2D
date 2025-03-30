using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager
{
    #region Key

    private const string KEY_EMAIL_LOGIN = "key_username";
    private const string KEY_PASSWORD = "key_password";
    private const string KEY_ACCOUNT_ID = "key_account_id";
    private const string KEY_LAST_LOGIN_SERVER = "key_last_login";
    private const string KEY_CP = "key_cp";
    private const string KEY_SOUND = "key_sound";
    private const string KEY_MUSIC = "key_music";
    private const string KEY_CACHE_CLIENT_LANGUAGE = "key_cache_client_language";

    #endregion

    #region SET
    
    public static void SaveEmailLogin(string username)
    {
        CacheManager.SetPrefString(KEY_EMAIL_LOGIN, username);
    }

    public static void SavePassword(string pwd)
    {
        CacheManager.SetPrefString(KEY_PASSWORD, pwd);
    }

    public static void SaveAccountID(string accountId)
    {
        CacheManager.SetPrefString(KEY_ACCOUNT_ID, accountId);
    }
    
    public static void SaveSoundSetting(bool soundOn)
    {
        PlayerPrefs.SetInt(KEY_SOUND, soundOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    public static void SaveMusicSetting(bool musicOn)
    {
        PlayerPrefs.SetInt(KEY_MUSIC, musicOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    #endregion

    #region GET

    public static string GetUsername()
    {
        return CacheManager.LoadPrefString(KEY_EMAIL_LOGIN);
    }

    public static string GetPassword()
    {
        return CacheManager.LoadPrefString(KEY_PASSWORD);
    }

    public static string GetAccountID()
    {
        return CacheManager.LoadPrefString(KEY_ACCOUNT_ID);
    }
    
    public static bool GetMusicSetting()
    {
        int iOn = PlayerPrefs.GetInt(KEY_MUSIC, 1);
        return iOn == 1;
    }
    
    public static bool GetSoundSetting()
    {
        int iOn = PlayerPrefs.GetInt(KEY_SOUND, 1);
        return iOn == 1;
    }

    #endregion
}