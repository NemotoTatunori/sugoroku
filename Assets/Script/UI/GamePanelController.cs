using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GamePanelController : MonoBehaviour
{
    /// <summary>プレイヤーステータスボックス</summary>
    [SerializeField] PlayerStatusBoxController m_playerStatusBox = null;
    /// <summary>テキストを表示する</summary>
    [SerializeField] Text m_progressText = null;
    /// <summary>ルーレット</summary>
    [SerializeField] RouletteController m_roulette = null;
    /// <summary>人追加パネル</summary>
    [SerializeField] AddHumanPanelController m_addHumanPanel = null;
    /// <summary>就職パネルのテキスト</summary>
    [SerializeField] Text m_findWorkPanel = null;
    /// <summary>プレイヤーボタンリスト</summary>
    [SerializeField] GameObject m_playerJumpButtonList = null;
    /// <summary>プレイヤーボタン</summary>
    [SerializeField] PlayerJumpButton m_playerJumpButtonPrefab = null;
    /// <summary>ターンエンドボタン</summary>
    [SerializeField] GameObject m_turnEndButton = null;
    GameManager m_gameManager;
    /// <summary>プレイヤーステータスボックスのプロパティ</summary>
    public PlayerStatusBoxController PlayerStatusBox => m_playerStatusBox;
    /// <summary>ルーレットのプロパティ</summary>
    public RouletteController Roulette => m_roulette;
    /// <summary>人追加パネルのプロパティ</summary>
    public AddHumanPanelController AddHumanPanel => m_addHumanPanel;
    /// <summary>表示するテキストのプロパティ</summary>
    public GameObject ProgressText => m_progressText.transform.parent.gameObject;
    /// <summary>ターンエンドボタンのプロパティ</summary>
    public GameObject TurnEndButton => m_turnEndButton;
    /// <summary>プレイヤーボタンリストのプロパティ</summary>
    public GameObject PlayerJumpButtonList => m_playerJumpButtonList;
    /// <summary>
    /// プレイヤージャンプボタン生成
    /// </summary>
    /// <param name="players">プレイヤー配列</param>
    /// <param name="action">関数</param>
    public void GetPlayer(PlayerController[] players, Action<Vector3> action)
    {
        GameObject list = m_playerJumpButtonList.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        list.GetComponent<RectTransform>().sizeDelta = new Vector2(0, players.Length * 50);
        for (int i = 0; i < players.Length; i++)
        {
            PlayerJumpButton pb = Instantiate(m_playerJumpButtonPrefab, list.transform);
            pb.Setting(players[i], action);
        }
    }
    /// <summary>
    /// ゲームマネージャーの情報を受け取る
    /// </summary>
    /// <param name="gm">ゲームマネージャー</param>
    public void GetGameManager(GameManager gm)
    {
        m_gameManager = gm;
        m_roulette.GetGameManager(gm);
        m_addHumanPanel.GetGameManager(gm);
    }
    /// <summary>
    /// 表示するテキストを更新して表示する
    /// </summary>
    /// <param name="t"></param>
    public void TextDisplay(string t)
    {
        ProgressText.SetActive(true);
        m_progressText.text = t;
    }
    /// <summary>
    /// 就職パネルに文字を反映させる
    /// </summary>
    /// <param name="t">職業</param>
    public void FindWorkText(string t)
    {
        m_findWorkPanel.transform.parent.gameObject.SetActive(true);
        m_findWorkPanel.text = t + "になりますか？\n\n※なると分岐終わりまで進む";
    }
    /// <summary>
    /// 就職パネルの答えを受け取る
    /// </summary>
    /// <param name="answer">答え</param>
    public void FindWorkMove(bool answer)
    {
        m_findWorkPanel.transform.parent.gameObject.SetActive(false);
        m_gameManager.FindWorkMove(answer);
    }
    public void Next()
    {
        m_gameManager.Progress();
    }
}
