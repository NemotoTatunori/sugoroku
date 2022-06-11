using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTestCell : EventCellBase
{
    protected override void CallbackEvent()
    {
        Debug.Log($"Name{name}");
    }
}
