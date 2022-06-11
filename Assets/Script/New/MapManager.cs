using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    List<PlayerData> players = new List<PlayerData>();
    List<CellBase> cells = new List<CellBase>();
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            cells.Add(transform.GetChild(i).gameObject.GetComponent<CellBase>());
        }
        Debug.Log(cells.Count);
    }

    public void AddPlayer(PlayerController player)
    {
        PlayerData p = new PlayerData(player);
        players.Add(p);
    }
}
