using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapManager : MonoBehaviour
{
    static MapManager m_mapManager;
    public static MapManager Instance => m_mapManager;
    
    int m_playerId = 0;
    List<PlayerData> m_players = new List<PlayerData>();
    List<CellBase> m_cells = new List<CellBase>();
    public PlayerData m_player;
    void Start()
    {
        m_mapManager = this;

        for (int i = 0; i < transform.childCount; i++)
        {
            m_cells.Add(transform.GetChild(i).gameObject.GetComponent<CellBase>());
        }
        Debug.Log(m_cells.Count);
    }

    public void AddPlayer(PlayerController player)
    {
        PlayerData p = new PlayerData(player, m_playerId);
        m_players.Add(p);
        m_playerId++;
    }
    public void SetPlayer(int myId)
    {
        m_player = m_players.First(p => p.MyId == myId);
    }
    void OnEvent()
    {
        if (m_cells[m_player.CellIndex].Event())
        {
            m_player.CellIndex++;
        }
        
    }
}
