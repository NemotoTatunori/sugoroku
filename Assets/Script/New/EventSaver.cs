using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSaver : MonoBehaviour
{
    List<CellBase> m_cells = new List<CellBase>();
    int m_id = 0;
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            m_cells.Add(transform.GetChild(i).gameObject.GetComponent<CellBase>());
        }
    }
    public bool Callback()
    {
        m_cells[m_id].Event();
        m_id++;
        Debug.Log($"NextEvent {m_id}");
        return m_cells.Count <= m_id;
    }
}
