using System;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class ResourceHelper : MonoBehaviour
{
    #region Load DB

    public static byte[] LoadDbBinContent(string fileDb)
    {
        return LoadDbBinContentByLanguage(fileDb, Constants.DEFAULT_LANGUAGE);
    }

    private static byte[] LoadDbBinContentByLanguage(string fileDb, Language language)
    {
        string pathDbFile = $"Db/{fileDb}";
        
        TextAsset textAsset = Resources.Load<TextAsset>(pathDbFile);

        return textAsset?.bytes;
    }

    #endregion
    
    public static GameObject LoadPrefab(string path)
    {
        GameObject gameObject = null;
        gameObject = Resources.Load<GameObject>(path);

        if (gameObject == null) Debug.LogError($"Error loading GameObject: {path}");

        return gameObject;
    }

    public static AudioClip LoadSound(string nameSound)
    {
        AudioClip audioClip = null;
        
        string pathRes = "Sounds/" + nameSound;

        audioClip = Resources.Load<AudioClip>(pathRes);
            
        if (audioClip == null) Debug.LogError($"Error loading sound: {pathRes}");

        return audioClip;
    }

    public static ScriptableObject LoadScriptTableObject(string name)
    {
        ScriptableObject scriptTable = null;
        
        string pathRes = "ScriptableObjects/" + name;
        scriptTable = Resources.Load<ScriptableObject>(pathRes);

        return scriptTable;
    }

    public static void LoadSkeletonAnimation(SkeletonAnimation skeletonAnimation, string pathSkel)
    {
        UnityEngine.Object spineSkeleton = null;
        
        spineSkeleton = Resources.Load<UnityEngine.Object>(pathSkel);

        if (spineSkeleton == null) Debug.LogError($"Error loading Skeleton Animation: {pathSkel}");

        skeletonAnimation.skeletonDataAsset = (SkeletonDataAsset)spineSkeleton;
        skeletonAnimation.Initialize(true);
    }

    public static void LoadSkeletonGraphicUI(SkeletonGraphic skeletonGraphic, string pathSkel)
    {
        UnityEngine.Object spineSkeleton = null;

        spineSkeleton = Resources.Load<UnityEngine.Object>(pathSkel);

        if (spineSkeleton == null) Debug.LogError($"Error loading Skeleton Graphic: {pathSkel}");

        skeletonGraphic.skeletonDataAsset = (SkeletonDataAsset)spineSkeleton;
        skeletonGraphic.Initialize(true);
    }

    public static Sprite LoadSprite(string pathSprite, string pathDefault = "_Common/Images/Icon/icon_unknown")
    {
        Sprite sprite = null;
        
        sprite = Resources.Load<Sprite>(pathSprite);

        if (sprite == null)
        {
            sprite = Resources.Load<Sprite>(pathDefault);
            Debug.LogError($"Error loading Sprite: {pathSprite}");
        }

        return sprite;
    }
    
}
