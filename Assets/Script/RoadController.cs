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
    [SerializeField] protected Transform[] m_nextConnect = null;
    /// <summary>前のマスの接続部</summary>
    [SerializeField] Transform[] m_prevConnect = null;
    /// <summary>次のマス</summary>
    [SerializeField] protected RoadController[] m_nextRoads = null;
    /// <summary>前のマス</summary>
    [SerializeField] protected RoadController[] m_prevRoads = null;
    /// <summary>マスのテキスト</summary>
    [SerializeField] string m_eventText = null;
    /// <summary>ストップマスのフラグ</summary>
    [SerializeField] bool m_stopFlag = false;
    /// <summary>位置補正のフラグ</summary>
    bool m_positionCorrection = false;
    /// <summary>ゲームマネージャー</summary>
    protected GameManager m_gameManager;
    /// <summary>前のマスの接続部のプロパティ</summary>
    public Transform[] PrevConnect
    {
        get => m_prevConnect;
    }
    /// <summary>マス番号のプロパティ</summary>
    public string RoadNumber
    {
        get => m_roadNumber;
        set
        {
            m_roadNumber = value;
            m_roadNumberText.text = m_roadNumber;
        }
    }
    /// <summary>位置補正のプロパティ</summary>
    public bool PositionCorrection
    {
        get => m_positionCorrection;
    }
    /// <summary>ストップマスのプロパティ</summary>
    public bool StopFlag
    {
        get => m_stopFlag;
    }
    /// <summary>車が止まる場所のプロパティ</summary>
    public Transform StopPint
    {
        get => m_stopPoint;
    }
    /// <summary>一つ前のマスのプロパティ</summary>
    public RoadController PrevRoad
    {
        get => m_prevRoads[0];
    }

    void Start()
    {
        if (m_roadNumber != "")
        {
            m_roadNumberText.text = m_roadNumber;
        }
    }
    /// <summary>
    /// マス情報を設定する
    /// </summary>
    public virtual void RoadSetUp(RoadController road, string rn)
    {
        m_gameManager = FindObjectOfType<GameManager>();
        if (RoadNumber == "")
        {
            RoadNumber = rn;
        }

        PrevRoadSet(road);
        //次のマスに情報を送る
        if (m_nextRoads.Length == 0)
        {
            return;
        }
        if (!m_nextRoads[0].PositionCorrection)
        {
            Vector3 now = m_nextConnect[0].position;
            Vector3 next = m_nextRoads[0].PrevConnect[0].position;
            Vector3 a = m_nextRoads[0].gameObject.transform.position;
            m_nextRoads[0].PositionSetUp(a + (now - next));
        }
        m_gameManager.GetRoads(this);
        m_nextRoads[0].RoadSetUp(this, NextNumber(RoadNumber,0));
    }
    /// <summary>
    /// 一つ前のマスを登録する
    /// </summary>
    /// <param name="road">一つ前のマス</param>
    protected void PrevRoadSet(RoadController road)
    {
        if (m_prevRoads.Length == 0)
        {
            m_prevRoads = new RoadController[1];
            m_prevRoads[0] = road;
        }
        else
        {
            RoadController[] prevRoads = m_prevRoads;
            m_prevRoads = new RoadController[prevRoads.Length + 1];
            int i = 0;
            while (i < m_prevRoads.Length - 1)
            {
                m_prevRoads[i] = prevRoads[i];
                i++;
            }
            m_prevRoads[i] = road;
        }
    }
    /// <summary>
    /// マス番号を設定する
    /// </summary>
    /// <param name="rn">前のマスの番号</param>
    protected virtual string NextNumber(string rn, int bn)
    {
        string[] sn = rn.Split(char.Parse("-"));
        int[] n = new int[sn.Length];
        for (int i = 0; i < n.Length; i++)
        {
            n[i] = int.Parse(sn[i]);
        }
        n[2]++;
        string an = "";
        for (int i = 0; i < n.Length; i++)
        {
            an += n[i].ToString();
            if (i < n.Length - 1)
            {
                an += char.Parse("-");
            }
        }
        return an;
    }
    /// <summary>
    /// マスの位置を調節する
    /// </summary>
    public void PositionSetUp(Vector3 p)
    {
        m_positionCorrection = true;
        this.transform.position = p;
    }
    /// <summary>
    /// 次のマスを返す
    /// </summary>
    /// <returns>次のマス</returns>
    public virtual RoadController NextRoad()
    {
        if (m_nextRoads.Length == 0)
        {
            return null;
        }
        return m_nextRoads[0];
    }
}
