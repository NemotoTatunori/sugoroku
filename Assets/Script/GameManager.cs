using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>最大人数</summary>
    int m_maxEntry = 30;
    Coroutine m_coroutine;

    //エントリーパネルで使う変数
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


    //ゲームで使う変数
    /// <summary>職業一覧</summary>
    string[] m_professions = { "スポーツ選手", "プログラマー", "パティシエ", "料理人", "大工" };
    /// <summary>給料一覧</summary>
    int[,] m_salarys = {
        { 10000, 11000, 12000, 13000, 14000 },
        { 30000, 29000, 28000, 27000, 26000 } };
    /// <summary>カメラ</summary>
    [SerializeField] GameObject m_camera = null;
    /// <summary>最初のマス</summary>
    [SerializeField] RoadController m_first = null;
    /// <summary>車のプレハブ</summary>
    [SerializeField] GameObject m_carPrefab = null;
    /// <summary>プレイヤーたちの情報</summary>
    [SerializeField] PlayerController[] m_players = null;
    /// <summary>マスの配列</summary>
    RoadController[,,] m_Roads;
    /// <summary>手番を管理する</summary>
    [SerializeField] int m_order = 0;
    /// <summary>ルーレット</summary>
    [SerializeField] RouletteController m_roulette = null;
    /// <summary>ルーレットのデフォルト数</summary>
    int[] m_rouletteLineupDefault = { 1, 2, 3, 4, 5 };
    /// <summary>マスのテキストを表示するテキスト</summary>
    [SerializeField] Text m_roadText = null;

    [SerializeField] Text m_playerStatusNameBoxText = null;
    [SerializeField] Text m_playerStatusMoneyBoxText = null;
    [SerializeField] Text m_playerStatusProfessionBoxText = null;
    [SerializeField] Text m_playerStatusSalaryRankBoxText = null;
    /// <summary>ルーレットのプロパティ</summary>
    public RouletteController Roulette
    {
        get => m_roulette;
    }

    void Start()
    {
        m_Roads = new RoadController[5, 2, 20];
        m_first.RoadSetUp(null, m_first.RoadNumber, this);
    }
    void Update()
    {

    }

    /// <summary>
    /// マスの情報を取得する
    /// </summary>
    /// <param name="rc">マスの情報</param>
    public void GetRoads(RoadController rc)
    {
        string[] srn = rc.RoadNumber.Split(char.Parse("-"));
        int[] irn = new int[srn.Length];
        for (int i = 0; i < irn.Length; i++)
        {
            irn[i] = int.Parse(srn[i]);
        }
        m_Roads[irn[0], irn[1], irn[2]] = rc;
    }
    /// <summary>
    /// 手番を変える
    /// </summary>
    void TurnChange()
    {
        bool ag = true;
        foreach (var item in m_players)
        {
            if (!item.Goal)
            {
                ag = false;
                break;
            }
        }
        if (!ag)
        {
            m_order++;
            if (m_order >= m_players.Length)
            {
                m_order = 0;
            }
            if (m_players[m_order].Goal)
            {
                TurnChange();
            }
            else
            {
                m_roulette.GetLineup(m_rouletteLineupDefault);
                PlayerStatusBoxUpdata();
                m_coroutine = StartCoroutine(GameProgress());
            }
        }
        else
        {
            Debug.Log("全員ゴールした");
            //全員ゴールした時の処理を書く
        }
    }
    /// <summary>
    /// 給料を返す
    /// </summary>
    /// <param name="profession">職業</param>
    /// <param name="salaryRank">ランク</param>
    /// <returns>給料</returns>
    public int Salary(int profession, int salaryRank)
    {
        return m_salarys[profession, salaryRank];
    }
    /// <summary>
    /// プレイヤーステータスボックスを更新する
    /// </summary>
    public void PlayerStatusBoxUpdata()
    {
        PlayerController p = m_players[m_order];
        m_playerStatusNameBoxText.text = p.OwnerName;
        m_playerStatusMoneyBoxText.text = p.Money.ToString();
        m_playerStatusProfessionBoxText.text = m_professions[p.Profession];
        m_playerStatusSalaryRankBoxText.text = p.SalaryRank.ToString();
    }

    /// <summary>
    /// ゲームサイクル
    /// </summary>
    /// <returns></returns>
    IEnumerator GameProgress()
    {
        PlayerController p = m_players[m_order];
        if (!p.Rest)
        {
            //ルーレットを表示
            m_roulette.gameObject.SetActive(true);

            //ルーレットを動かす
            yield return m_roulette.RouletteStart();

            //出た数値を車に送る
            yield return p.MoveStart(m_roulette.Number, false);

            //マスのテキストを表示する
            m_roadText.transform.parent.gameObject.SetActive(true);
            m_roadText.text = p.Location.EventText();

            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_roadText.transform.parent.gameObject.SetActive(false);
                    break;
                }
                yield return null;
            }

            //止まったマスのイベントを呼ぶ
            yield return p.Location.RoadEvent(m_players[m_order]);
        }
        else
        {
            p.Rest = false;
        }
        if (p.PaydayFlag)
        {
            p.GetMoney(Salary(p.Profession, p.SalaryRank));
        }
        TurnChange();
    }

    //エントリーパネルで使うメソッド
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
        float px = (m_peopleNum - 1) * -2.5f;
        if (m_peopleNum >= 11) { px = 9 * -2.5f; }
        int x = 0;
        int z = 0;
        for (int i = 0; i < m_peopleNum; i++)
        {
            GameObject car = Instantiate(m_carPrefab);
            car.transform.position = new Vector3(px + x * 5, 0, z * -10);
            m_players[i] = car.GetComponent<PlayerController>();
            EntryNamePrefab en = m_entryNameDisplay.transform.GetChild(i).GetComponent<EntryNamePrefab>();
            m_players[i].Seting(en.Name, m_first);
            m_players[i].AddHuman(en.Seibetu, en.Name);
            x++;
            if (x >= 10)
            {
                x = 0;
                z++;
            }
        }
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
        }
        PlayerStatusBoxUpdata();
        m_coroutine = StartCoroutine(GameProgress());
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
        en.Name = m_entryName.text;
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
    /// <summary>
    /// 警告テキスト表示
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
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
