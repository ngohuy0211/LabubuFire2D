using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public static bool IsInitLanguage = false;

#if UNITY_EDITOR
    public const string OS = "UNITY";

#elif UNITY_IOS
	public const string OS = "IOS";


#elif UNITY_ANDROID
	public const string OS = "ANDROID";

#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE
	public const string OS = "PC";
#endif

    public const string VERSION_VN = "1.0.0";
    public static string VERSION = VERSION_VN;

    //secods
    public const float CONNECTION_TIMEOUT = 15f;

    public static Language DEFAULT_LANGUAGE = Language.VI;
    
}

public enum Language : int
{
    VI = 1,
    EN = 2,
    TH = 3,
    ZH = 4
}