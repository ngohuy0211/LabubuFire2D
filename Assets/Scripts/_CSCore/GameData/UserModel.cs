using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class UserModel
{
    public string userId;
    public string userDisplayName;
    public string userEmail;
    public string userUrlAvatar;
    public int avatarUsingId = -1;
    //1 la avatar mac dinh khi tao nich, gg thi la avat url gg, fb thi la avat url fb, con lai dung avt mac dinh cua he thong
    public List<int> ownAvatars = new List<int>() {1};
    //Character mac dinh dau game la 1
    public int characterUsingId = 1;
    public List<int> ownCharacters = new List<int>() {1};
    //Bullet mac dinh dau game la 1
    public int bulletUsingId = 1;
    public List<int> ownBullets = new List<int>() {1};
    public long coin;
    public long gem;
    public int currentMapLevel = 1;
}
