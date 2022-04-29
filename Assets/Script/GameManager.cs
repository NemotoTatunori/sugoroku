using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>車のプレハブ</summary>
    [SerializeField] GameObject m_carPrefab = null;
    /// <summary>プレイヤーたちの情報</summary>
    [SerializeField] PlayerController[] m_players = null;
    /// <summary>手番を管理する</summary>
    [SerializeField] int m_order = 0;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddName()
    {

    }
    public void RemoveName(GameObject name)
    {

    }
}
