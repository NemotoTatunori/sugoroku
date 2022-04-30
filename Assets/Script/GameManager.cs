using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //エントリーパネルで使う変数
    /// <summary>最大人数</summary>
    int m_maxEntry = 30;
    /// <summary>参加する人の名前を入力する場所</summary>
    [SerializeField] Text m_entryName = null;
    /// <summary>参加する人の名前を表示するプレハブ</summary>
    [SerializeField] GameObject m_entryNamePrehab = null;
    /// <summary>参加する人の名前を表示する場所</summary>
    [SerializeField] GameObject m_entryNameDisplay = null;
    /// <summary>参加数のテキスト</summary>
    [SerializeField] Text m_peopleNumText = null;
    /// <summary>エントリー数</summary>
    int m_peopleNum = 0;
    /// <summary>男の色（水色）</summary>
    Color m_manColor = new Color(0.6f, 1, 1);
    /// <summary>女の色（ピンク）</summary>
    Color m_womanColor = new Color(1, 0.6f, 1);
    /// <summary>警告テキスト</summary>
    [SerializeField] GameObject m_caveatText = null;
    /// <summary>エントリーパネル</summary>
    [SerializeField] GameObject m_entryPanel = null;

    Coroutine m_coroutine;

    //ゲームで使う変数
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
    /// <summary>
    /// ゲームをスタートさせる
    /// </summary>
    public void GameStart()
    {
        if (m_peopleNum == 0)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
            m_coroutine = StartCoroutine(Caveat("１人もいないよ！"));
            return;
        }
        m_entryPanel.SetActive(false);
        m_players = new PlayerController[m_peopleNum];
        for (int i = 0; i < m_peopleNum; i++)
        {
            GameObject car = Instantiate(m_carPrefab);
            car.transform.position = new Vector3(i * 3, 0, 0);
            m_players[i] = car.GetComponent<PlayerController>();
            EntryNamePrefab en = m_entryNameDisplay.transform.GetChild(i).GetComponent<EntryNamePrefab>();
            m_players[i].Set();
            m_players[i].AddHuman(en.Seibetu,en.Name);
        }
    }
    /// <summary>
    /// エントリーさせる
    /// </summary>
    /// <param name="seibetu">性別</param>
    public void AddName(bool seibetu)
    {
        if (m_peopleNum >= m_maxEntry)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
            m_coroutine = StartCoroutine(Caveat("最大" + m_maxEntry + "人まで！"));
            return;
        }
        m_peopleNum++;
        GameObject player = Instantiate(m_entryNamePrehab);
        EntryNamePrefab en = player.GetComponent<EntryNamePrefab>();
        en.Name= m_entryName.text;
        en.Seibetu = seibetu;
        player.transform.SetParent(m_entryNameDisplay.transform);
        player.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        m_entryNameDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(0, m_peopleNum * 50);
        m_peopleNumText.text = m_peopleNum + "人";
        if (seibetu)
        {
            player.GetComponent<Image>().color = m_manColor;
        }
        else
        {
            player.GetComponent<Image>().color = m_womanColor;
        }
    }
    /// <summary>
    /// エントリーを取り消す
    /// </summary>
    /// <param name="name">ネームプレハブ</param>
    public void RemoveName(GameObject name)
    {
        m_peopleNum--;
        Destroy(name);
        m_peopleNumText.text = m_peopleNum + "人";
    }

    IEnumerator Caveat(string text)
    {
        m_caveatText.SetActive(true);
        Text t = m_caveatText.transform.GetChild(0).GetComponent<Text>();
        t.text = text;
        for (float i = 0; i < 2; i += Time.deltaTime)
        {
            yield return null;
        }
        t.text = "";
        m_caveatText.SetActive(false);
    }
}
