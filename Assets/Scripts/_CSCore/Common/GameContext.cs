using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spine.Unity;
using UnityEngine;

public class GameContext : Singleton<GameContext>
{
    // data
    public string Udid = "";
    public string Session = "";
    public string Username = "";
    public BaseScene CurrentScene = null;
    public List<BasePopup> PopupActives = new List<BasePopup>();
    public bool MusicOn = true;
    public bool SoundOn = true;
    
    //
    //chi dung cho thanh toan SDK Voi Doi Tac thoi,
    //ko dung o dau khac
    public string SDKUserId = "";
    public string SDKUsername = "";

    public void ClearData()
    {
        Udid = "";
        Session = "";
        Username = "";
        StaticData.Reset();
    }
}