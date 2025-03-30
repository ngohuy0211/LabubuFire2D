using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserModel
{
    public string UserId { get; set; }
    public string UserDisplayName { get; set; }
    public string UserEmail { get; set; }
    public string UserUrlAvatar { get; set; }

    public int AvatarUsingId { get; set; } = -1;
    //1 la avatar mac dinh khi tao nich, gg thi la avat url gg, fb thi la avat url fb, con lai dung avt mac dinh cua he thong
    public List<int> OwnAvatars { get; set; } = new List<int>() {1};
    //Character mac dinh dau game la 1
    public int CharacterUsingId { get; set; } = 1;
    public List<int> OwnCharacters { get; set; } = new List<int>() {1};
    //Bullet mac dinh dau game la 1
    public int BulletUsingId { get; set; } = 1;
    public List<int> OwnBullets { get; set; } = new List<int>() {1};
    //
    public long Coin { get; set; }
    public long Gem { get; set; }
    public int CurrentMapLevel { get; set; } = 1;
}
