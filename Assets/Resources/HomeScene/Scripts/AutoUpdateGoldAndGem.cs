using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemKey
{
    COIN = 1,
    GEM = 2
}

public class AutoUpdateGoldAndGem : MonoBehaviour
{
    #region Define

    [SerializeField] private ItemKey itemKey;
    [SerializeField] private Text textNumItem;
    
    #endregion

    #region Core MonoBehavior

    void Update()
    {
        textNumItem.text = itemKey switch
        {
            ItemKey.COIN => GameContext.Instance.UserModel.coin.ToString(),
            ItemKey.GEM => GameContext.Instance.UserModel.gem.ToString(),
            _ => "0"
        };
    }

    #endregion
}
