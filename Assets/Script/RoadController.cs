using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>イベントの種類</summary>
public enum RoadEvents
{
    /// <summary>進む</summary>
    Go,
    /// <summary>戻る</summary>
    Return,
    /// <summary>休み</summary>
    Rest,
    /// <summary>お金をもらう</summary>
    GetMoney,
    /// <summary>お金を払う</summary>
    PayMoney,
    /// <summary>就職</summary>
    FindWork,
    /// <summary>給料日</summary>
    Payday,
    /// <summary>結婚</summary>
    Marriage,
    /// <summary>出産</summary>
    Childbirth,
    /// <summary>分岐</summary>
    RoadBranch,
    /// <summary>昇給</summary>
    PayRaise,
    /// <summary>降給</summary>
    ReductionInPay,
    /// <summary>お宝</summary>
    Treasure,
    /// <summary>ゴール</summary>
    Goal,
}
public class RoadController : MonoBehaviour
{
    /// <summary>イベントのステート</summary>
    [SerializeField] RoadEvents m_event = RoadEvents.GetMoney;
    /// <summary>イベントのパラメーター</summary>
    [SerializeField] int m_eventParameter = 1000;
    /// <summary>マス番号</summary>
    [SerializeField] string m_roadNumber;
    /// <summary>マス番号表示</summary>
    [SerializeField] TextMesh m_roadNumberText = null;
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
    [SerializeField] Text m_eventText = null;
    /// <summary>ストップマスのフラグ</summary>
    [SerializeField] bool m_stopFlag = false;
    /// <summary>マスの色を表示する</summary>
    [SerializeField] GameObject m_roadColorDisplay = null;
    /// <summary>マスの色候補</summary>
    [SerializeField] Material[] m_roadColors = null;
    /// <summary>設定済のフラグ</summary>
    bool m_positionCorrection = false;
    /// <summary>ゲームマネージャー</summary>
    protected Action<RoadController> m_getRoad;
    /// <summary>前のマスの接続部のプロパティ</summary>
    public Transform[] PrevConnect => m_prevConnect;
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
    /// <summary>設定済フラグのプロパティ</summary>
    public bool PositionCorrection => m_positionCorrection;
    /// <summary>ストップマスのプロパティ</summary>
    public bool StopFlag => m_stopFlag;
    /// <summary>車が止まる場所のプロパティ</summary>
    public Transform StopPint => m_stopPoint;
    /// <summary>一つ前のマスのプロパティ</summary>
    public RoadController PrevRoad => m_prevRoads[0];
    /// <summary>次のマスのプロパティ</summary>
    public RoadController[] NextRoads => m_nextRoads;
    /// <summary>イベントのプロパティ</summary>
    public RoadEvents Event => m_event;
    /// <summary>イベントパラメーターのプロパティ</summary>
    public int EventParameter => m_eventParameter;

    void Start()
    {
        if (m_roadNumber != "")
        {
            m_roadNumberText.text = m_roadNumber;
        }
        if (m_stopFlag)
        {
            m_roadColorDisplay.GetComponent<Renderer>().material = m_roadColors[1];
        }
    }
    void OnValidate()
    {

    }
    /// <summary>
    /// マス情報を設定する
    /// </summary>
    public virtual void RoadSetUp(RoadController road, string rn, Action<RoadController> gameManager)
    {
        PrevRoadSet(road);
        if (m_positionCorrection)
        {
            return;
        }
        m_getRoad = gameManager;
        if (RoadNumber == "")
        {
            RoadNumber = rn;
        }
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
        m_getRoad(this);
        m_nextRoads[0].RoadSetUp(this, NextNumber(RoadNumber, 0), gameManager);
        m_positionCorrection = true;
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
        this.transform.position = p;
    }
    /// <summary>
    /// 次のマスを返す
    /// </summary>
    /// <returns>次のマス</returns>
    public virtual RoadController NextRoad(int branch)
    {
        if (m_nextRoads.Length == 0)
        {
            return null;
        }
        return m_nextRoads[0];
    }
    /// <summary>
    /// マスのイベントテキストを返す
    /// </summary>
    /// <returns>イベントテキスト</returns>
    public string EventText()
    {
        string BText = m_eventText.text;
        string AText = "";
        foreach (var item in BText)
        {
            if (item == char.Parse("#"))
            {
                AText += m_eventParameter;
            }
            else
            {
                AText += item;
            }
        }
        return AText;
    }
}
