using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing_Singleton : Singleton<Testing_Singleton>
{
    protected override void Awake()
    {
        base.Awake();
    }
}
