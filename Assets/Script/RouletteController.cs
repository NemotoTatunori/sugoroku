using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteController : MonoBehaviour
{
    /// <summary>数字を表示するテキスト</summary>
    [SerializeField] Text m_numberDisplay = null;
    /// <summary>スタートボタン</summary>
    [SerializeField] GameObject m_StartButton = null;
    /// <summary>ストップボタン</summary>
    [SerializeField] GameObject m_StopButton = null;
    /// <summary>数字が切り替わる時間</summary>
    [SerializeField] float m_speed = 0.1f;
    /// <summary>数字を隠す蓋</summary>
    [SerializeField] GameObject m_lid = null;
    /// <summary>選ばれる数字の配列</summary>
    int[] m_lineup = { 1, 2, 3, 4, 5 };
    /// <summary>選ばれる文字の配列</summary>
    string[] m_branchRoadLineup = { "左", "右" };
    /// <summary>ルーレット始動のフラグ</summary>
    bool m_start = false;
    /// <summary>出た数字</summary>
    int m_number = 0;

    GameManager m_gameManager;
    /// <summary>出た数字のプロパティ</summary>
    public int Number => m_number;
    /// <summary>
    /// ゲームマネージャーを受け取る
    /// </summary>
    /// <param name="gm">ゲームマネージャー</param>
    public void GetGameManager(GameManager gm)
    {
        m_gameManager = gm;
    }
    /// <summary>
    /// ルーレットをスタートさせる
    /// </summary>
    public Coroutine RouletteStart(bool f)
    {
        m_StartButton.SetActive(false);
        m_StopButton.SetActive(true);
        m_start = true;
        return StartCoroutine(Roulette(f));
    }
    /// <summary>
    /// 選ばれる数字の候補を受け取る
    /// </summary>
    /// <param name="lineup">候補</param>
    public void GetLineup(int[] lineup)
    {
        m_lineup = lineup;
    }
    /// <summary>
    /// 選ばれる分岐道の候補を受け取る
    /// </summary>
    /// <param name="lineup">候補</param>
    public void GetBranchRoadLineup(string[] lineup)
    {
        m_branchRoadLineup = lineup;
    }
    /// <summary>
    /// ルーレットを止める
    /// </summary>
    public void RouletteStop()
    {
        m_start = false;
        m_StopButton.SetActive(false);
    }
    /// <summary>
    /// ルーレットを動かす
    /// </summary>
    /// <param name="EventFlag">イベントのフラグ</param>
    /// <returns></returns>
    IEnumerator Roulette(bool EventFlag)
    {
        int loop = EventFlag ? m_lineup.Length - 1 : m_branchRoadLineup.Length - 1;
        m_lid.SetActive(true);
        int n = 0;
        float interval = 0;
        while (true)
        {
            if (interval > m_speed)
            {
                interval = 0;
                if (EventFlag)
                {
                    m_numberDisplay.text = m_lineup[n].ToString();
                }
                else
                {
                    m_numberDisplay.text = m_branchRoadLineup[n];
                }
                n++;
                if (n > loop)
                {
                    n = 0;
                }
            }
            if (!m_start) { break; }
            interval += Time.deltaTime;
            yield return null;
        }
        if (EventFlag)
        {
            m_numberDisplay.text = m_lineup[n].ToString();
            m_number = m_lineup[n];
        }
        else
        {
            m_numberDisplay.text = m_branchRoadLineup[n];
            m_number = n;
        }
        m_lid.SetActive(false);
        yield return new WaitForSeconds(1f);
        m_StartButton.SetActive(true);
        this.gameObject.SetActive(false);
        m_gameManager.Progress();
    }
}
