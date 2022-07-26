using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    /// <summary>�Q�[���i�s�̃X�e�[�g</summary>
    enum ProgressState
    {
        /// <summary>�t�F�[�h�C��</summary>
        FadeIn = 0,
        /// <summary>��Ԃ�\��</summary>
        TurnText = 1,
        /// <summary>���[���b�g</summary>
        Roulette = 2,
        /// <summary>�v���C���[�ړ�</summary>
        PlayerMove = 3,
        /// <summary>�}�X�̃e�L�X�g�\��</summary>
        RoadText = 4,
        /// <summary>�}�X�C�x���g</summary>
        Event = 5,
        /// <summary>��ԏI���</summary>
        TurnEnd = 6,
        /// <summary>�t�F�[�h�A�E�g</summary>
        FadeOut = 7,
        /// <summary>��Ԍ���</summary>
        PlayerCheck = 8,
    }
    /// <summary>�t�F�[�h�p�l��</summary>
    [SerializeField] Image m_fadePanel = null;
    /// <summary>�G���g���[�p�l���p�l��</summary>
    [SerializeField] EntryPanelController m_entryPanel = null;
    /// <summary>�Q�[���p�l��</summary>
    [SerializeField] GamePanelController m_gamePanel = null;
    /// <summary>�Q�[���I�[�o�[�p�l��</summary>
    [SerializeField] GameOverPanelController m_gameOverPanel = null;
    /// <summary>�E�ƃf�[�^</summary>
    [SerializeField] WorkData m_workData = null;
    /// <summary>�S�[���������̃{�[�i�X</summary>
    int m_goalBonus = 1000000;
    /// <summary>�S�[�������l��</summary>
    int m_goalNumber = 0;
    /// <summary>�J����</summary>
    [SerializeField] CameraController m_camera = null;
    /// <summary>�ŏ��̃}�X</summary>
    [SerializeField] RoadController m_first = null;
    /// <summary>�v���C���[�����̏��</summary>
    PlayerController[] m_players = null;
    /// <summary>�S�[�������l�����̋��z����</summary>
    PlayerController[] m_goalRanking = null;
    /// <summary>�}�X�̔z��</summary>
    //RoadController[,,] m_Roads;
    /// <summary>��Ԃ��Ǘ�����</summary>
    int m_order = 0;
    /// <summary>���ݎ�Ԃ̃v���C���[</summary>
    PlayerController m_orderPlayer;
    /// <summary>���[���b�g</summary>
    RouletteController m_roulette = null;
    /// <summary>���[���b�g�̃f�t�H���g��</summary>
    int[] m_rouletteLineupDefault = { 1, 2, 3, 4, 5 };
    /// <summary>�Q�[���i�s�̃X�e�[�g</summary>
    ProgressState m_progressState = ProgressState.FadeIn;
    /// <summary>���N���X�̕Ԏ��̃t���O</summary>
    bool m_reply = false;
    List<PlayerController> m_backClipGoku = new List<PlayerController>();
    void Start()
    {
        m_gamePanel.GetGameManager(this);
        m_roulette = m_gamePanel.Roulette;
        m_entryPanel.Setting(GameStart, m_first);
        m_gamePanel.RoadBranchPanel.SetAction(SelectionBranch);
        m_first.RoadSetUp(null, m_first.RoadNumber, GetRoads);
    }
    /// <summary>
    /// �}�X�̏����擾����
    /// </summary>
    /// <param name="rc">�}�X�̏��</param>
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
    /// ��Ԃ�ς���
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
    /// ������Ԃ�
    /// </summary>
    /// <param name="profession">�E��</param>
    /// <param name="salaryRank">�����N</param>
    /// <returns>����</returns>
    int Salary(PlayerController player)
    {
        float salaryOriginal = m_workData.GetData(player.Profession).Salary;
        float magnification = m_workData.GetData(player.Profession).Magnification;
        int salary = (int)salaryOriginal + (int)(salaryOriginal * magnification * player.SalaryRank);
        return salary;
    }
    /// <summary>
    /// �Q�[���i�s�̃X�e�[�g�Ǘ�
    /// </summary>
    public void Progress()
    {
        switch (m_progressState)
        {
            case ProgressState.FadeIn:
                if (!m_orderPlayer.Rest)
                {
                    m_progressState = ProgressState.TurnText;
                    m_gamePanel.TextDisplay(m_orderPlayer.Owner.Name + "����̔Ԃł�");
                }
                else
                {
                    m_progressState = ProgressState.TurnEnd;
                    m_gamePanel.TextDisplay(m_orderPlayer.Owner.Name + "����͂��x�݂̂悤�ł�");
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
                StartCoroutine(Fade(false, true));
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
                StartCoroutine(Fade(true, true));
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
    /// �}�X�̃C�x���g
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
                m_gamePanel.TextDisplay(player.Owner.Name + "�����" + road.EventParameter + "�����I");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                break;
            case RoadEvents.PayMoney:
                player.GetMoney(-road.EventParameter);
                m_gamePanel.TextDisplay(player.Owner.Name + "�����" + road.EventParameter + "�x������");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                break;
            case RoadEvents.FindWork:
                m_gamePanel.FindWorkText(m_workData.GetData(road.EventParameter).WorkName.ToString());
                break;
            case RoadEvents.Payday:
                player.GetMoney(Salary(m_orderPlayer));
                m_gamePanel.TextDisplay(player.Owner.Name + "����͋����Ƃ���" + Salary(m_orderPlayer) + "�����I");
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
                StartCoroutine(Branch(player, true));
                break;
            case RoadEvents.PayRaise:
                player.SalaryRank++;
                m_gamePanel.TextDisplay(player.Owner.Name + "����̋��������N���オ�����I");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                break;
            case RoadEvents.ReductionInPay:
                if (player.SalaryRank <= 0)
                {
                    player.SalaryRank--;
                    m_gamePanel.TextDisplay(player.Owner.Name + "����̋��������N�����������E�E�E");
                    m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                }
                else
                {
                    m_gamePanel.TextDisplay(player.Owner.Name + "����̋��������N�͂���ȏ㉺����Ȃ�");
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
                    m_gamePanel.TextDisplay(player.Owner.Name + "��" + m_goalNumber + "�ʂŃS�[�������I\n�܋��Ƃ���" + bonus + "��������I");
                }
                else
                {
                    m_gamePanel.TextDisplay(player.Owner.Name + "��" + m_goalNumber + "�ʂŃS�[�������I\n�܋��͂��炦�Ȃ������E�E�E");
                }
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_workData);
                PlayersSort(player);
                break;
        }
    }
    /// <summary>
    /// �v���C���[�ړ�
    /// </summary>
    /// <param name="player">�ړ�����v���C���[</param>
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
                yield return StartCoroutine(Branch(player, false));
            }
            yield return null;
        }
        Progress();
    }
    /// <summary>
    /// �S�[�������ۂɎ�ԂƏ��ʂ��\�[�g����
    /// </summary>
    /// <param name="player">�S�[�������v���C���[</param>
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
    /// �l��ǉ�����C�x���g
    /// </summary>
    /// <param name="p">�p�^�[��</param>
    /// <param name="name">���O</param>
    public void AddHumanEvent(int p, string name)
    {
        switch (p)
        {
            case 0:
                bool s = m_players[m_order].Owner.Seibetu ? false : true;
                m_players[m_order].AddHuman(s, name);
                m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "����͌��������I");
                break;
            case 1:
                m_players[m_order].AddHuman(true, name);
                m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "�����ɒj�̎q�����܂ꂽ�I");
                break;
            case 2:
                m_players[m_order].AddHuman(false, name);
                m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "�����ɏ��̎q�����܂ꂽ�I");
                break;
        }
        Congratulations(m_players[m_order], m_players[m_order].Location.EventParameter);
        m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_workData);
    }
    /// <summary>
    /// �j��������
    /// </summary>
    /// <param name="player">���炤�v���C���[</param>
    /// <param name="money">���z</param>
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
    /// �A�E�}�X����
    /// </summary>
    /// <param name="answer">����</param>
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
    /// ���Ƀo�b�N�N���[�W���[�E�l�����邩���ׂ�
    /// </summary>
    /// <returns>����A���Ȃ�</returns>
    bool CheckBackClip()
    {
        int[] line = { 16, 17, 18, 19 };
        return CheckProfession(line, false);
    }
    /// <summary>
    /// �w��̐E�Ƃ����邩���ׂ�
    /// </summary>
    /// <param name="proNum">���ׂ�E�Ɣԍ�</param>
    /// <param name="inOrderPlayer">��Ԏ҂��܂߂邩</param>
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
    /// ���򓹂̍s������߂�
    /// </summary>
    /// <param name="player">���߂�v���C���[</param>
    /// <returns></returns>
    public IEnumerator Branch(PlayerController player, bool pro)
    {
        int pattern = player.Location.EventParameter;
        if (player.Location.gameObject.GetComponent<ForkedRoadController>().m_sentaku)
        {
            m_gamePanel.RoadBranchPanel.gameObject.SetActive(true);
            m_gamePanel.RoadBranchPanel.Setting(pattern);
            while (!m_reply)
            {
                yield return null;
            }
            m_reply = false;
        }
        else
        {
            m_roulette.gameObject.SetActive(true);
            string[] lineup;
            if (pattern == 0)
            {
                lineup = new string[] { "��", "�E" };
            }
            else if (pattern == 1)
            {
                lineup = new string[] { "��", "�^��" };
            }
            else if (pattern == 2)
            {
                lineup = new string[] { "�^��", "�E" };
            }
            else
            {
                lineup = new string[] { "��", "�^��", "�E" };
            }
            m_roulette.GetBranchRoadLineup(lineup);
            yield return m_roulette.RouletteStart(false);
            player.BranchNumber = m_roulette.Number;
            if (pro)
            {
                Progress();
            }
        }
    }
    /// <summary>
    /// ����I���̌��ʂ��󂯎��
    /// </summary>
    /// <param name="road">�s��</param>
    public void SelectionBranch(int road)
    {
        m_reply = true;
        m_orderPlayer.BranchNumber = road;
    }
    /// <summary>
    /// �t�F�[�h�𐧌䂷��
    /// </summary>
    /// <param name="inOut"></param>
    /// <returns></returns>
    IEnumerator Fade(bool inOut, bool progress)
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
        m_fadePanel.gameObject.SetActive(false);
        if (progress)
        {
            Progress();
        }

    }
    void CameraJump(Vector3 point)
    {
        m_camera.PositionSet(point);
    }
    /// <summary>
    /// �Q�[�����X�^�[�g������
    /// </summary>
    public void GameStart(PlayerController[] players)
    {
        m_players = players;
        m_goalRanking = new PlayerController[m_players.Length];
        m_progressState = ProgressState.PlayerCheck;
        m_order = 0;
        m_orderPlayer = m_players[m_order];
        m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_workData);
        StartCoroutine(Opening());
        m_gamePanel.GetPlayer(m_players, CameraJump);
    }
    /// <summary>
    /// �I�[�v�j���O���[�r�[
    /// </summary>
    /// <returns></returns>
    IEnumerator Opening()
    {
        yield return StartCoroutine(Fade(false, false));
        m_entryPanel.gameObject.SetActive(false);
        m_gamePanel.gameObject.SetActive(true);
        float start = m_players[0].transform.position.x;
        float end = m_players.Length > 10 ? m_players[9].transform.position.x : m_players[m_players.Length - 1].transform.position.x;
        Vector3 camera = new Vector3(start, 10, 20);
        m_camera.transform.position = camera;
        m_camera.transform.rotation = Quaternion.Euler(30, 180, 0);
        yield return StartCoroutine(Fade(true, false));
        while (start <= end)
        {
            start += Time.deltaTime * 10;
            m_camera.transform.position = new Vector3(start, 10, 20);
            yield return null;
        }
        yield return StartCoroutine(Fade(false, false));
        m_orderPlayer.transform.position = m_first.StopPint.position;
        CameraJump(m_orderPlayer.transform.position);
        yield return StartCoroutine(Fade(true, false));
        Progress();
    }
}