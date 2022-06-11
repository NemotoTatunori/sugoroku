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
        PlayerData player = MapManager.Instance.m_player;

        if (!player.CellData.m_isBranch)
        {
            player.CellData.m_isBranch = true;
            player.CellData.m_branchId = BranchEvent(m_eventSavers.Count);
            player.CellData.m_eventId = 0;
            return false;
        }
        else
        {
            if (m_event.Callback())
            {
                player.CellData.m_isBranch = false;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    protected abstract int BranchEvent(int branchCount);
}
