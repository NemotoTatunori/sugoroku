using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddHumanPanelController : MonoBehaviour
{
    /// <summary>ゲームマネージャー</summary>
    GameManager m_gameManager;
    /// <summary>イベント内容を表示するテキスト</summary>
    [SerializeField] Text m_eventNameText = null;
    /// <summary>追加する人の名前を受け取るテキスト</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>結婚イベントボタン</summary>
    [SerializeField] GameObject m_fianceButton;
    /// <summary>男の子ボタン</summary>
    [SerializeField] GameObject m_boyButton;
    /// <summary>女の子ボタン</summary>
    [SerializeField] GameObject m_girlButton;
    /// <summary>テキストに表示する文字候補</summary>
    string[] m_eventName = { "婚姻届", "子供が生まれた！" };
    /// <summary>
    /// 表示するパネルのセッティング
    /// </summary>
    /// <param name="p">パターン</param>
    public void Set(int p)
    {
        switch (p)
        {
            case 0:
                m_eventNameText.text = m_eventName[p];
                m_fianceButton.SetActive(true);
                m_boyButton.SetActive(false);
                m_girlButton.SetActive(false);
                break;
            case 1:
                m_eventNameText.text = m_eventName[p];
                m_fianceButton.SetActive(false);
                m_boyButton.SetActive(true);
                m_girlButton.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// 人を追加するボタンにつける
    /// </summary>
    /// <param name="p">パターン</param>
    public void AddHumanButton(int p)
    {
        m_gameManager.AddHumanEvent(p, m_nameText.text);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// ゲームマネージャーを受け取る
    /// </summary>
    /// <param name="gm">ゲームマネージャー</param>
    public void SetGameManager(GameManager gm)
    {
        m_gameManager = gm;
    }
}
