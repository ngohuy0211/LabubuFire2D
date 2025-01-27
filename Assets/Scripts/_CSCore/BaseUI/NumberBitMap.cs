using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BitmapFontManager
{
    static BitmapFontManager instance;

    FontBitMap[] lstFont;

    static string imageFont = "JJKShared/Fonts/number_item";

    public static BitmapFontManager getInstance()
    {
        if (instance == null)
        {
            instance = new BitmapFontManager();
            Sprite[] all = Resources.LoadAll<Sprite>(imageFont);
            instance.lstFont = new FontBitMap[all.Length];
            for (int i = 0; i < all.Length; i++)
            {
                instance.lstFont[i] = new FontBitMap();

                instance.lstFont[i].name = all[i].name;

                instance.lstFont[i].image = all[i];
            }
        }

        return instance;
    }

    public Sprite getSprite(char c, string number_prefix)
    {
        Sprite img = null;

        for (int i = 0; i < lstFont.Length; i++)
        {
            string tmp = "" + c;
            if (c == '/')
            {
                tmp = "slash";
            }

            if (c == '.' || c == ',')
            {
                tmp = "dot";
            }

            if (lstFont[i].name.Equals(number_prefix + tmp))
            {
                return lstFont[i].image;
            }
        }


        return img;
    }
}

[Serializable]
public class FontBitMap
{
    public string name;
    public Sprite image;
}

public class NumberBitMap : MonoBehaviour
{
    public Image[] lstNumber;
    public string number_prefix = "white_";
    string currentString = "!@#$";

    public void setColor(Color numberColor)
    {
        for (int i = 0; i < lstNumber.Length; i++)
        {
            lstNumber[i].color = numberColor;
        }
    }
    public void setNumber(string value)
    {
        if (currentString == value)
            return;
        currentString = value;


        for (int i = value.Length; i < lstNumber.Length; i++)
        {
            if (lstNumber[i].gameObject.activeSelf)
                lstNumber[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < lstNumber.Length; i++)
        {
            if (i < value.Length)
            {
                if (!lstNumber[i].gameObject.activeSelf)
                    lstNumber[i].gameObject.SetActive(true);

                lstNumber[i].sprite = BitmapFontManager.getInstance().getSprite(value[i], number_prefix);
                lstNumber[i].SetNativeSize();
            }
        }
    }
}