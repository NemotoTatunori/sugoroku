using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadController : MonoBehaviour
{
    /// <summary>マス番号</summary>
    [SerializeField] string m_roadNumber;
    /// <summary>マス番号表示</summary>
    [SerializeField] Text m_roadNumberText = null;
    /// <summary>車が止まる場所</summary>
    [SerializeField] Transform m_stopPoint = null;
    /// <summary>次のマスの接続部</summary>
    [SerializeField] Transform[] m_nextConnect = null;
    /// <summary>前のマスの接続部</summary>
    [SerializeField] Transform m_prevConnect = null;
    /// <summary>次のマス</summary>
    [SerializeField] RoadController[] m_nextRoads = null;
    /// <summary>前のマス</summary>
    [SerializeField] RoadController m_prevRoads = null;
    /// <summary>マスのテキスト</summary>
    [SerializeField] string m_eventText = null;

    public Transform PrevConnect
    {
        get => m_prevConnect;
    }

    void Start()
    {
        m_roadNumberText.text = m_roadNumber;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// マスの位置を調節する
    /// </summary>
    public void PositionSetUp()
    {
        if (m_nextRoads == null)
        {
            return;
        }
        for (int i = 0; i < m_nextRoads.Length; i++)
        {
            Vector3 now = m_nextConnect[i].position;
            Vector3 next = m_nextRoads[i].PrevConnect.position;
            Vector3 a = m_nextRoads[i].gameObject.transform.position;
            m_nextRoads[i].gameObject.transform.position = a + (now - next);
            m_nextRoads[i].PositionSetUp();
        }
    }
}
