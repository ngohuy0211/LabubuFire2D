using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable", menuName = "ScripTableObject/ListSprite")]
public class ListSprite : ScriptableObject
{
    public List<Sprite> listSprite = new List<Sprite>();
    // gray - yellow - blue - red - green
    public Sprite GetSprite(int index)
    {
        Sprite ret = null;
        if (index < listSprite.Count)
        {
            ret = listSprite[index];
        }
        else
        {
            Debug.Log(" Out of rank list sprite");
        }
        return ret;
    }
}
