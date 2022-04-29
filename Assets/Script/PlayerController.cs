using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>プレイヤーの名前</summary>
    [SerializeField] string m_name = null;
    /// <summary>所持金</summary>
    [SerializeField] int m_money = 0;
    /// <summary>現在位置</summary>
    [SerializeField] string m_location = null;
    /// <summary>座席の位置</summary>
    [SerializeField] Transform[] m_Seats = null;
    /// <summary>座席に人がいるかのフラグ</summary>
    [SerializeField] bool[] m_sittings = null;
    /// <summary>人のプレハブ</summary>
    [SerializeField] GameObject m_humanPrefab = null;
    void Start()
    {
        m_sittings = new bool[m_Seats.Length];
        for (int i = 1; i < m_sittings.Length; i++)
        {
            m_sittings[i] = false;
        }
        m_sittings[0] = true;
        Instantiate(m_humanPrefab, m_Seats[0]);
    }
    void Update()
    {

    }
}
