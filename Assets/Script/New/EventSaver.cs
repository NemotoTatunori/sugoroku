using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSaver : MonoBehaviour
{
    List<CellBase> m_cells = new List<CellBase>();
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_cells.Add(transform.GetChild(i).gameObject.GetComponent<CellBase>());
        }
    }
    public bool Callback()
    {
        m_cells[MapManager.Instance.m_player.CellData.m_eventId].Event();
        MapManager.Instance.m_player.CellData.m_eventId++;
        return m_cells.Count <= MapManager.Instance.m_player.CellData.m_eventId;
    }
}
