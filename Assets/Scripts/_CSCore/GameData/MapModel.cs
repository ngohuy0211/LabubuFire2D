using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModel
{
    public int ID { get; set; }
    public int MapID { get; set; }
    public int Level { get; set; }
    public Dictionary<int, int> DictItemDropRate = new Dictionary<int, int>();
    public int RequirePoint { get; set; }
    public int WinCoin { get; set; }
    public int Time { get; set; }
}
