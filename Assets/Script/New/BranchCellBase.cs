using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BranchCellBase : CellBase
{
    List<EventSaver> m_eventSavers = new List<EventSaver>();
    EventSaver m_event = null;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_eventSavers.Add(transform.GetChild(i).gameObject.AddComponent<EventSaver>());
        }
    }
    public override bool Event()
    {
        if (m_event == null)
        {
            m_event = m_eventSavers[BranchEvent(m_eventSavers.Count)];
            Debug.Log($"EventSaverName :{m_event.gameObject.name}");
            return false;
        }
        else
        {
            return m_event.Callback();
        }
    }
    protected abstract int BranchEvent(int branchCount);
}
