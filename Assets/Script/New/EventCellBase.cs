using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventCellBase : CellBase
{
    public override bool Event()
    {
        CallbackEvent();
        return true;
    }
    protected abstract void CallbackEvent();
}
