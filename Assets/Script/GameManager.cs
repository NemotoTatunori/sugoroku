using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>進行のステート</summary>
    enum ProgressState
    {
        /// <summary>フェードイン</summary>
        FadeIn = 0,
        /// <summary>手番を表示</summary>
        TurnText = 1,
        /// <summary>ルーレット</summary>
        Roulette = 2,
        /// <summary>プレイヤー移動</summary>
        PlayerMove = 3,
        /// <summary>マスのテキスト表示</summary>
        RoadText = 4,
        /// <summary>マスイベント</summary>
        Event = 5,
        /// <summary>手番終わり</summary>
        TurnEnd = 6,
        /// <summary>フェードアウト</summary>
        FadeOut = 7,
        /// <summary>手番交換</summary>
        PlayerCheck = 8,
    }
    /// <summary>フェードパネル</summary>
    [SerializeField] Image m_fadePanel = null;
    /// <summary>エントリーパネルパネル</summary>
    [SerializeField] EntryPanelController m_entryPanel = null;
    /// <summary>ゲームパネル</summary>
    [SerializeField] GamePanelController m_gamePanel = null;
    /// <summary>職業一覧</summary>
    string[] m_professions = { "フリーター", "スポーツ選手", "プログラマー", "パティシエ", "料理人", "大工" };
    /// <summary>給料一覧</summary>
    int[,] m_salarys = {
        {5000, 10000, 11000, 12000, 13000, 14000 },
        {10000, 30000, 29000, 28000, 27000, 26000 } };
    /// <summary>ゴールした時のボーナス</summary>
    int m_goalBonus = 100000;
    /// <summary>ゴールした人数</summary>
    int m_goalNumber = 0;
    /// <summary>カメラ</summary>
    [SerializeField] CameraController m_camera = null;
    /// <summary>最初のマス</summary>
    [SerializeField] RoadController m_first = null;
    /// <summary>車のプレハブ</summary>
    [SerializeField] GameObject m_carPrefab = null;
    /// <summary>プレイヤーたちの情報</summary>
    PlayerController[] m_players = null;
    /// <summary>マスの配列</summary>
    RoadController[,,] m_Roads;
    /// <summary>手番を管理する</summary>
    int m_order = 0;
    /// <summary>ルーレット</summary>
    RouletteController m_roulette = null;
    /// <summary>ルーレットのデフォルト数</summary>
    int[] m_rouletteLineupDefault = { 1, 2, 3, 4, 5 };
    /// <summary>進行のステート</summary>
    ProgressState m_state = ProgressState.FadeIn;

    void Start()
    {
        m_gamePanel.GetGameManager(this);
        m_roulette = m_gamePanel.Roulette;
        m_entryPanel.GetGameManager(this, m_first);
        m_Roads = new RoadController[5, 2, 20];
        m_first.RoadSetUp(null, m_first.RoadNumber, this);
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
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_professions);
                Progress();
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
    int Salary(int profession, int salaryRank)
    {
        return m_salarys[salaryRank, profession];
    }
    /// <summary>
    /// ゲーム進行のステート管理
    /// </summary>
    public void Progress()
    {
        switch (m_state)
        {
            case ProgressState.FadeIn:
                if (!m_players[m_order].Rest)
                {
                    m_state = ProgressState.TurnText;
                    m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "さんの番です");
                }
                else
                {
                    m_state = ProgressState.TurnEnd;
                    m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "さんはお休みのようです");
                    m_players[m_order].Rest = false;
                }
                break;
            case ProgressState.TurnText:
                m_gamePanel.ProgressText.SetActive(false);
                m_state = ProgressState.Roulette;
                m_roulette.gameObject.SetActive(true);
                m_roulette.RouletteStart(true);
                break;
            case ProgressState.Roulette:
                m_state = ProgressState.PlayerMove;
                m_players[m_order].MoveStart(m_roulette.Number, false, false, m_camera);
                break;
            case ProgressState.PlayerMove:
                m_state = ProgressState.RoadText;
                m_gamePanel.TextDisplay(m_players[m_order].Location.EventText());
                break;
            case ProgressState.RoadText:
                m_gamePanel.ProgressText.SetActive(false);
                m_state = ProgressState.Event;
                RoadEvent(m_players[m_order]);
                break;
            case ProgressState.Event:
                m_state = ProgressState.TurnEnd;
                m_gamePanel.ProgressText.SetActive(false);
                m_camera.Move = true;
                m_gamePanel.TurnEndButton.SetActive(true);
                break;
            case ProgressState.TurnEnd:
                m_gamePanel.TurnEndButton.SetActive(false);
                m_state = ProgressState.FadeOut;
                StartCoroutine(Fade(false));
                break;
            case ProgressState.FadeOut:
                m_state = ProgressState.PlayerCheck;
                PlayerController p = m_players[m_order];
                if (p.PaydayFlag)
                {
                    p.GetMoney(Salary(p.Profession, p.SalaryRank));
                    p.PaydayFlag = false;
                }
                TurnChange();
                p = m_players[m_order];
                p.transform.position = p.Location.StopPint.position;
                m_camera.PositionSet(p.transform.position);
                m_camera.Move = false;
                break;
            case ProgressState.PlayerCheck:
                m_state = ProgressState.FadeIn;
                StartCoroutine(Fade(true));
                break;
        }
    }
    /// <summary>
    /// マスのイベント
    /// </summary>
    /// <returns></returns>
    void RoadEvent(PlayerController player)
    {
        RoadController road = player.Location;
        switch (road.Event)
        {
            case RoadEvents.Go:
                player.MoveStart(road.EventParameter, false, true, m_camera);
                break;
            case RoadEvents.Return:
                player.MoveStart(road.EventParameter, true, true, m_camera);
                break;
            case RoadEvents.Rest:
                player.Rest = true;
                Progress();
                break;
            case RoadEvents.GetMoney:
                player.GetMoney(road.EventParameter);
                m_gamePanel.TextDisplay(player.Owner.Name + "さんは" + road.EventParameter + "得た！");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                break;
            case RoadEvents.PayMoney:
                player.GetMoney(-road.EventParameter);
                m_gamePanel.TextDisplay(player.Owner.Name + "さんは" + road.EventParameter + "支払った");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                break;
            case RoadEvents.FindWork:
                m_gamePanel.FindWorkText(m_professions[road.EventParameter]);
                break;
            case RoadEvents.Payday:
                player.GetMoney(Salary(player.Profession, player.SalaryRank));
                m_gamePanel.TextDisplay(player.Owner.Name + "さんは給料として" + m_salarys[player.SalaryRank, player.Profession] + "得た！");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                player.PaydayFlag = false;
                break;
            case RoadEvents.Marriage:
                m_gamePanel.AddHumanPanel.gameObject.SetActive(true);
                m_gamePanel.AddHumanPanel.Set(0);
                break;
            case RoadEvents.Childbirth:
                m_gamePanel.AddHumanPanel.gameObject.SetActive(true);
                m_gamePanel.AddHumanPanel.Set(1);
                break;
            case RoadEvents.RoadBranch:
                StartCoroutine(Branch(player));
                break;
            case RoadEvents.PayRaise:
                player.SalaryRank = player.SalaryRank + 1;
                m_gamePanel.TextDisplay(player.Owner.Name + "さんの給料ランクが上がった！");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                break;
            case RoadEvents.Goal:
                int bonus = m_goalBonus - m_goalNumber * 10000;
                player.Goal = true;
                player.GetMoney(bonus);
                m_goalNumber++;
                m_gamePanel.TextDisplay(player.Owner.Name + "が" + m_goalNumber + "位でゴールした！\n賞金として" + bonus + "もらった！");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                break;
        }
    }
    /// <summary>
    /// 人を追加するイベント
    /// </summary>
    /// <param name="p">パターン</param>
    /// <param name="name">名前</param>
    public void AddHumanEvent(int p, string name)
    {
        switch (p)
        {
            case 0:
                bool s = m_players[m_order].Owner.Seibetu ? false : true;
                m_players[m_order].AddHuman(s, name);
                m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "さんは結婚した！");
                break;
            case 1:
                m_players[m_order].AddHuman(true, name);
                m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "さん宅に男の子が生まれた！");
                break;
            case 2:
                m_players[m_order].AddHuman(false, name);
                m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "さん宅に女の子が生まれた！");
                break;
        }
        Congratulations(m_players[m_order], m_players[m_order].Location.EventParameter);
        m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_professions);
    }
    /// <summary>
    /// 祝い金処理
    /// </summary>
    /// <param name="player">もらうプレイヤー</param>
    /// <param name="money">金額</param>
    void Congratulations(PlayerController player, int money)
    {
        int m = 0;
        foreach (var item in m_players)
        {
            if (item != player)
            {
                item.GetMoney(-money);
                m += money;
            }
        }
        player.GetMoney(m);
    }
    /// <summary>
    /// 就職マス処理
    /// </summary>
    /// <param name="answer">応え</param>
    public void FindWorkMove(bool answer)
    {
        if (!answer)
        {
            Progress();
            return;
        }
        m_state = ProgressState.RoadText;
        PlayerController player = m_players[m_order];
        RoadController road = player.Location;
        player.Profession = road.EventParameter;
        int mv = 1;
        while (road.NextRoad(0).Event != RoadEvents.Payday)
        {
            road = road.NextRoad(0);
            mv++;
            if (mv >= 100)
            {
                mv = 0;
                Debug.Log("無限ループ");
                break;
            }
        }
        player.MoveStart(mv, false, true, m_camera);
    }
    /// <summary>
    /// 分岐道の行先を決める
    /// </summary>
    /// <param name="player">決めるプレイヤー</param>
    /// <returns></returns>
    IEnumerator Branch(PlayerController player)
    {
        int r = player.Location.NextRoads.Length;
        m_roulette.gameObject.SetActive(true);
        string[] lineup;
        if (r == 2)
        {
            lineup = new string[] { "左", "右" };
        }
        else
        {
            lineup = new string[] { "左", "真中", "右" };
        }
        m_roulette.GetBranchRoadLineup(lineup);
        yield return m_roulette.RouletteStart(false);
        player.BranchNumber = m_roulette.Number;
    }
    /// <summary>
    /// フェードを制御する
    /// </summary>
    /// <param name="inOut"></param>
    /// <returns></returns>
    IEnumerator Fade(bool inOut)
    {
        m_fadePanel.gameObject.SetActive(true);
        float c;
        float a = m_fadePanel.color.a;
        if (inOut)
        {
            c = 0;
            while (c < a)
            {
                a -= Time.deltaTime;
                m_fadePanel.color = new Color(0, 0, 0, a);
                yield return null;
            }
            m_fadePanel.color = new Color(0, 0, 0, 0);
        }
        else
        {
            c = 1;
            while (c > a)
            {
                a += Time.deltaTime;
                m_fadePanel.color = new Color(0, 0, 0, a);
                yield return null;
            }
            m_fadePanel.color = new Color(0, 0, 0, 1);
        }
        Progress();
        m_fadePanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// ゲームをスタートさせる
    /// </summary>
    public void GameStart(PlayerController[] players)
    {
        m_players = players;
        m_state = ProgressState.PlayerCheck;
        m_order = 0;
        m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_professions);
        StartCoroutine(Fade(false));
        m_entryPanel.gameObject.SetActive(false);
    }
}
