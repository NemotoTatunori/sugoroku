using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CellBase : MonoBehaviour
{
    List<CellBase> m_cells = new List<CellBase>();
    
    public abstract bool Event();
    
    private void Start()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            m_cells.Add(gameObject.transform.GetChild(i).GetComponent<CellBase>());
        }
    }
}
