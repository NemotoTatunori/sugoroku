using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    PlayerController m_player;
    cellData m_cellData;
    public cellData CellData => m_cellData;
    int m_myId = 0;
    public int MyId => m_myId;
    int m_cellIndex = 0;
    public int CellIndex { get => m_cellIndex; set { m_cellIndex = value; } }
    public PlayerData(PlayerController player,int id)
    {
        m_player = player;
        m_myId = id;
        m_cellData = new cellData();
    }
    public class cellData
    {
        public bool m_isBranch = false;
        public int m_branchId = 0;
        public int m_eventId = 0;
    }
}
