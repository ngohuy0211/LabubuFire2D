using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingInitDataAction
{
    public string Name { get; set; } = "";
    public Action Action { get; set; }

    public LoadingInitDataAction(string name, Action action)
    {
        this.Name = name;
        this.Action = action;
    }
}