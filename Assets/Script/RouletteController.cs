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
    /// <summary>選ばれる数字の配列</summary>
    int[] m_lineup = { 1, 2, 3, 4, 5 };//, 6, 7, 8, 9, 10
    /// <summary>ルーレット始動のフラグ</summary>
    bool m_start = false;
    /// <summary>出た数字</summary>
    int m_number = 0;

    public int Number
    {
        get => m_number;
    }

    /// <summary>
    /// ルーレットをスタートさせる
    /// </summary>
    public Coroutine RouletteStart()
    {
        m_StartButton.SetActive(false);
        m_StopButton.SetActive(true);
        m_start = true;
        return StartCoroutine(Roulette());

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
    /// <returns></returns>
    IEnumerator Roulette()
    {
        int n = 0;
        float interval = 0;
        while (true)
        {
            if (!m_start) { break; }
            if (interval > m_speed)
            {
                interval = 0;
                m_numberDisplay.text = m_lineup[n].ToString();
                n++;
                if (n > m_lineup.Length - 1)
                {
                    n = 0;
                }
            }
            interval += Time.deltaTime;
            yield return null;
        }
        m_numberDisplay.text = m_lineup[n].ToString();
        m_number = m_lineup[n];
        yield return new WaitForSeconds(1f);
        m_StartButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
