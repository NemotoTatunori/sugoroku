using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    /// <summary>ゲーム進行のステート</summary>
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
    /// <summary>プレイヤー進行のステート</summary>
    enum EventState
    {
        /// <summary>通常</summary>
        Normal = 0,
        /// <summary>プレイヤー行動選択</summary>
        PlayerSelection = 1,
        /// <summary>プレイヤー移動</summary>
        PlayerMove = 2,
        /// <summary>プレイヤーイベント</summary>
        PlayerEvent = 3,
    }
    /// <summary>フェードパネル</summary>
    [SerializeField] Image m_fadePanel = null;
    /// <summary>エントリーパネルパネル</summary>
    [SerializeField] EntryPanelController m_entryPanel = null;
    /// <summary>ゲームパネル</summary>
    [SerializeField] GamePanelController m_gamePanel = null;
    /// <summary>ゲームオーバーパネル</summary>
    [SerializeField] GameOverPanelController m_gameOverPanel = null;
    /// <summary>職業データ</summary>
    [SerializeField] WorkData m_workData = null;
    /// <summary>ゴールした時のボーナス</summary>
    int m_goalBonus = 1000000;
    /// <summary>ゴールした人数</summary>
    int m_goalNumber = 0;
    /// <summary>カメラ</summary>
    [SerializeField] CameraController m_camera = null;
    /// <summary>最初のマス</summary>
    [SerializeField] RoadController m_first = null;
    /// <summary>プレイヤーたちの情報</summary>
    PlayerController[] m_players = null;
    /// <summary>ゴールした人たちの金額順位</summary>
    PlayerController[] m_goalRanking = null;
    /// <summary>マスの配列</summary>
    //RoadController[,,] m_Roads;
    /// <summary>手番を管理する</summary>
    int m_order = 0;
    /// <summary>現在手番のプレイヤー</summary>
    PlayerController m_orderPlayer;
    /// <summary>ルーレット</summary>
    RouletteController m_roulette = null;
    /// <summary>ルーレットのデフォルト数</summary>
    int[] m_rouletteLineupDefault = { 1, 2, 3, 4, 5 };
    /// <summary>ゲーム進行のステート</summary>
    ProgressState m_progressState = ProgressState.FadeIn;
    /// <summary>プレイヤー進行のステート</summary>
    EventState m_eventState = EventState.Normal;
    List<PlayerController> m_backClipGoku = new List<PlayerController>();
    void Start()
    {
        m_gamePanel.GetGameManager(this);
        m_roulette = m_gamePanel.Roulette;
        m_entryPanel.GetGameManager(this, m_first);
        //m_Roads = new RoadController[5, 2, 20];
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
        //m_Roads[irn[0], irn[1], irn[2]] = rc;
    }
    /// <summary>
    /// 手番を変える
    /// </summary>
    void TurnChange()
    {
        if (!(m_players.Length <= m_goalNumber))
        {
            if (!m_orderPlayer.Goal)
            {
                m_order++;
            }
            if (m_order >= m_players.Length - m_goalNumber)
            {
                m_order = 0;
            }
            m_orderPlayer = m_players[m_order];
            m_roulette.GetLineup(m_rouletteLineupDefault);
            m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_workData);
            Progress();
        }
        else
        {
            m_gamePanel.gameObject.SetActive(false);
            m_gameOverPanel.GetPlayers(m_goalRanking);
        }
    }
    /// <summary>
    /// 給料を返す
    /// </summary>
    /// <param name="profession">職業</param>
    /// <param name="salaryRank">ランク</param>
    /// <returns>給料</returns>
    int Salary(PlayerController player)
    {
        float salaryOriginal = m_workData.GetData(player.Profession).Salary;
        float magnification = m_workData.GetData(player.Profession).Magnification;
        int salary = (int)salaryOriginal + (int)(salaryOriginal * magnification * player.SalaryRank);
        return salary;
    }
    /// <summary>
    /// プレイヤー行動ステート管理
    /// </summary>
    public void EndReport()
    {
        switch (m_eventState)
        {
            case EventState.Normal:
                Progress();
                break;
            case EventState.PlayerSelection:

                break;
            case EventState.PlayerMove:

                break;
            case EventState.PlayerEvent:

                break;
        }
    }
    /// <summary>
    /// ゲーム進行のステート管理
    /// </summary>
    public void Progress()
    {
        switch (m_progressState)
        {
            case ProgressState.FadeIn:
                if (!m_orderPlayer.Rest)
                {
                    m_progressState = ProgressState.TurnText;
                    m_gamePanel.TextDisplay(m_orderPlayer.Owner.Name + "さんの番です");
                }
                else
                {
                    m_progressState = ProgressState.TurnEnd;
                    m_gamePanel.TextDisplay(m_orderPlayer.Owner.Name + "さんはお休みのようです");
                    m_orderPlayer.Rest = false;
                }
                break;
            case ProgressState.TurnText:
                m_gamePanel.ProgressText.SetActive(false);
                m_progressState = ProgressState.Roulette;
                StartCoroutine(Roulette());
                break;
            case ProgressState.Roulette:
                m_progressState = ProgressState.PlayerMove;
                StartCoroutine(PlayerMove(m_roulette.Number, false, false, m_orderPlayer));
                break;
            case ProgressState.PlayerMove:
                m_progressState = ProgressState.RoadText;
                m_gamePanel.TextDisplay(m_orderPlayer.Location.EventText());
                break;
            case ProgressState.RoadText:
                m_gamePanel.ProgressText.SetActive(false);
                m_progressState = ProgressState.Event;
                RoadEvent(m_orderPlayer);
                break;
            case ProgressState.Event:
                m_progressState = ProgressState.TurnEnd;
                m_gamePanel.PlayerJumpButtonList.SetActive(true);
                m_gamePanel.ProgressText.SetActive(false);
                m_camera.Move = true;
                m_gamePanel.TurnEndButton.SetActive(true);
                break;
            case ProgressState.TurnEnd:
                m_gamePanel.PlayerJumpButtonList.SetActive(false);
                m_gamePanel.TurnEndButton.SetActive(false);
                m_progressState = ProgressState.FadeOut;
                StartCoroutine(Fade(false));
                break;
            case ProgressState.FadeOut:
                m_progressState = ProgressState.PlayerCheck;
                if (m_orderPlayer.PaydayFlag)
                {
                    m_orderPlayer.GetMoney(Salary(m_orderPlayer));
                    m_orderPlayer.PaydayFlag = false;
                }
                TurnChange();
                m_orderPlayer.transform.position = m_orderPlayer.Location.StopPint.position;
                m_camera.PositionSet(m_orderPlayer.transform.position);
                m_camera.Move = false;
                break;
            case ProgressState.PlayerCheck:
                m_progressState = ProgressState.FadeIn;
                StartCoroutine(Fade(true));
                break;
        }
    }
    IEnumerator Roulette()
    {
        m_roulette.gameObject.SetActive(true);
        yield return m_roulette.RouletteStart(true);
        Progress();
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
                StartCoroutine(PlayerMove(road.EventParameter, false, true, player));
                break;
            case RoadEvents.Return:
                StartCoroutine(PlayerMove(road.EventParameter, true, true, player));
                break;
            case RoadEvents.Rest:
                player.Rest = true;
                Progress();
                break;
            case RoadEvents.GetMoney:
                player.GetMoney(road.EventParameter);
                m_gamePanel.TextDisplay(player.Owner.Name + "さんは" + road.EventParameter + "得た！");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                break;
            case RoadEvents.PayMoney:
                player.GetMoney(-road.EventParameter);
                m_gamePanel.TextDisplay(player.Owner.Name + "さんは" + road.EventParameter + "支払った");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                break;
            case RoadEvents.FindWork:
                m_gamePanel.FindWorkText(m_workData.GetData(road.EventParameter).WorkName.ToString());
                break;
            case RoadEvents.Payday:
                player.GetMoney(Salary(m_orderPlayer));
                m_gamePanel.TextDisplay(player.Owner.Name + "さんは給料として" + Salary(m_orderPlayer) + "得た！");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
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
                player.SalaryRank++;
                m_gamePanel.TextDisplay(player.Owner.Name + "さんの給料ランクが上がった！");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                break;
            case RoadEvents.ReductionInPay:
                if (player.SalaryRank <= 0)
                {
                    player.SalaryRank--;
                    m_gamePanel.TextDisplay(player.Owner.Name + "さんの給料ランクが下がった・・・");
                    m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                }
                else
                {
                    m_gamePanel.TextDisplay(player.Owner.Name + "さんの給料ランクはこれ以上下がらない");
                }
                break;
            case RoadEvents.Goal:
                int bonus = m_goalBonus - m_goalNumber * 100000;
                player.Goal = true;
                m_goalRanking[m_goalNumber] = player;
                m_goalNumber++;
                if (bonus > 0)
                {
                    player.GetMoney(bonus);
                    m_gamePanel.TextDisplay(player.Owner.Name + "が" + m_goalNumber + "位でゴールした！\n賞金として" + bonus + "もらった！");
                }
                else
                {
                    m_gamePanel.TextDisplay(player.Owner.Name + "が" + m_goalNumber + "位でゴールした！\n賞金はもらえなかった・・・");
                }
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                PlayersSort(player);
                break;
        }
    }
    /// <summary>
    /// プレイヤー移動
    /// </summary>
    /// <param name="player">移動するプレイヤー</param>
    /// <returns></returns>
    IEnumerator PlayerMove(int m, bool e, bool reverse, PlayerController player)
    {
        for (int i = 0; i < m; i++)
        {
            RoadController nextPint;
            if (!reverse)
            {
                nextPint = player.Location.NextRoad(player.BranchNumber);
                transform.LookAt(nextPint.transform.position);
            }
            else
            {
                nextPint = player.Location.PrevRoad;
            }
            Vector3 next = nextPint.StopPint.position;
            yield return player.MoveStart(next, m_camera);
            player.Location = nextPint;
            if (player.Location.NextRoad(player.BranchNumber) == null)
            {
                reverse = true;
            }
            if (player.Location.Event == RoadEvents.Payday && !e && !reverse)
            {
                player.PaydayFlag = true;
            }
            if (player.Location.StopFlag && !e)
            {
                break;
            }
            if (player.Location.Event == RoadEvents.RoadBranch && i != m - 1)
            {
                yield return StartCoroutine(Branch(player));
            }
            yield return null;
        }
        Progress();
    }
    /// <summary>
    /// ゴールした際に手番と順位をソートする
    /// </summary>
    /// <param name="player">ゴールしたプレイヤー</param>
    void PlayersSort(PlayerController player)
    {
        for (int i = m_order; i < m_players.Length - m_goalNumber; i++)
        {
            m_players[i] = m_players[i + 1];
        }
        m_players[m_players.Length - m_goalNumber] = player;
        for (int i = m_goalNumber - 1; i > 0; i--)
        {
            if (m_goalRanking[i].Money > m_goalRanking[i - 1].Money)
            {
                m_goalRanking[i] = m_goalRanking[i - 1];
                m_goalRanking[i - 1] = player;
            }
            else
            {
                break;
            }
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
        m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_workData);
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
        m_progressState = ProgressState.RoadText;
        RoadController road = m_orderPlayer.Location;
        if (road.EventParameter != 18)
        {
            m_orderPlayer.Profession = road.EventParameter;
        }
        else
        {
            m_backClipGoku.Add(m_orderPlayer);
            if (CheckBackClip())
            {
                m_orderPlayer.Profession = road.EventParameter + 1;
            }
            else
            {
                m_orderPlayer.Profession = road.EventParameter;
            }
        }
        if (m_backClipGoku.Count != 0)
        {
            if (CheckBackClip())
            {
                foreach (var item in m_backClipGoku)
                {
                    item.Profession = m_workData.m_works.Count - 1;
                }
            }
        }

        m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_orderPlayer, m_workData);
        int mv = 1;
        while (road.NextRoad(0).Event != RoadEvents.Payday)
        {
            road = road.NextRoad(0);
            mv++;
        }
        StartCoroutine(PlayerMove(mv, false, false, m_orderPlayer));
    }
    /// <summary>
    /// 他にバッククロージャー職人がいるか調べる
    /// </summary>
    /// <returns>いる、いない</returns>
    bool CheckBackClip()
    {
        int[] line = { 16, 17, 18, 19 };
        return CheckProfession(line, false);
    }
    /// <summary>
    /// 指定の職業がいるか調べる
    /// </summary>
    /// <param name="proNum">調べる職業番号</param>
    /// <param name="inOrderPlayer">手番者を含めるか</param>
    /// <returns></returns>
    bool CheckProfession(int[] proNum, bool inOrderPlayer)
    {
        foreach (var item in m_players)
        {
            if (!inOrderPlayer && item == m_orderPlayer) { continue; }
            for (int i = 0; i < proNum.Length; i++)
            {
                if (item.Profession == proNum[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 分岐道の行先を決める
    /// </summary>
    /// <param name="player">決めるプレイヤー</param>
    /// <returns></returns>
    public IEnumerator Branch(PlayerController player)
    {
        int r = player.Location.EventParameter;
        m_roulette.gameObject.SetActive(true);
        string[] lineup;
        if (r == 0)
        {
            lineup = new string[] { "左", "右" };
        }
        else if (r == 1)
        {
            lineup = new string[] { "左", "真中" };
        }
        else if (r == 2)
        {
            lineup = new string[] { "真中", "右" };
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
    void CameraJump(Vector3 point)
    {
        m_camera.PositionSet(point);
    }
    /// <summary>
    /// ゲームをスタートさせる
    /// </summary>
    public void GameStart(PlayerController[] players)
    {
        m_players = players;
        m_goalRanking = new PlayerController[m_players.Length];
        m_progressState = ProgressState.PlayerCheck;
        m_order = 0;
        m_orderPlayer = m_players[m_order];
        m_orderPlayer.transform.position = m_first.StopPint.position;
        m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_workData);
        StartCoroutine(Fade(false));
        m_entryPanel.gameObject.SetActive(false);
        m_gamePanel.gameObject.SetActive(true);
        m_gamePanel.GetPlayer(m_players, CameraJump);
    }
}