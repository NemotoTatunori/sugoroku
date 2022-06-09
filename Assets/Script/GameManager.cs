using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>�i�s�̃X�e�[�g</summary>
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
    /// <summary>�E�ƈꗗ</summary>
    string[] m_professions = { "�t���[�^�[", "�X�|�[�c�I��", "�v���O���}�[", "�p�e�B�V�G", "�����l", "��H" };
    /// <summary>�����ꗗ</summary>
    int[,] m_salarys = {
        {5000, 10000, 11000, 12000, 13000, 14000 },
        {10000, 30000, 29000, 28000, 27000, 26000 } };
    /// <summary>�S�[���������̃{�[�i�X</summary>
    int m_goalBonus = 100000;
    /// <summary>�S�[�������l��</summary>
    int m_goalNumber = 0;
    /// <summary>�J����</summary>
    [SerializeField] CameraController m_camera = null;
    /// <summary>�ŏ��̃}�X</summary>
    [SerializeField] RoadController m_first = null;
    /// <summary>�Ԃ̃v���n�u</summary>
    [SerializeField] GameObject m_carPrefab = null;
    /// <summary>�v���C���[�����̏��</summary>
    PlayerController[] m_players = null;
    /// <summary>�}�X�̔z��</summary>
    RoadController[,,] m_Roads;
    /// <summary>��Ԃ��Ǘ�����</summary>
    int m_order = 0;
    /// <summary>���[���b�g</summary>
    RouletteController m_roulette = null;
    /// <summary>���[���b�g�̃f�t�H���g��</summary>
    int[] m_rouletteLineupDefault = { 1, 2, 3, 4, 5 };
    /// <summary>�i�s�̃X�e�[�g</summary>
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
        m_Roads[irn[0], irn[1], irn[2]] = rc;
    }
    /// <summary>
    /// ��Ԃ�ς���
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
            Debug.Log("�S���S�[������");
            //�S���S�[���������̏���������
        }
    }
    /// <summary>
    /// ������Ԃ�
    /// </summary>
    /// <param name="profession">�E��</param>
    /// <param name="salaryRank">�����N</param>
    /// <returns>����</returns>
    int Salary(int profession, int salaryRank)
    {
        return m_salarys[salaryRank, profession];
    }
    /// <summary>
    /// �Q�[���i�s�̃X�e�[�g�Ǘ�
    /// </summary>
    public void Progress()
    {
        switch (m_state)
        {
            case ProgressState.FadeIn:
                if (!m_players[m_order].Rest)
                {
                    m_state = ProgressState.TurnText;
                    m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "����̔Ԃł�");
                }
                else
                {
                    m_state = ProgressState.TurnEnd;
                    m_gamePanel.TextDisplay(m_players[m_order].Owner.Name + "����͂��x�݂̂悤�ł�");
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
    /// �}�X�̃C�x���g
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
                m_gamePanel.TextDisplay(player.Owner.Name + "�����" + road.EventParameter + "�����I");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                break;
            case RoadEvents.PayMoney:
                player.GetMoney(-road.EventParameter);
                m_gamePanel.TextDisplay(player.Owner.Name + "�����" + road.EventParameter + "�x������");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                break;
            case RoadEvents.FindWork:
                m_gamePanel.FindWorkText(m_professions[road.EventParameter]);
                break;
            case RoadEvents.Payday:
                player.GetMoney(Salary(player.Profession, player.SalaryRank));
                m_gamePanel.TextDisplay(player.Owner.Name + "����͋����Ƃ���" + m_salarys[player.SalaryRank, player.Profession] + "�����I");
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
                m_gamePanel.TextDisplay(player.Owner.Name + "����̋��������N���オ�����I");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                break;
            case RoadEvents.Goal:
                int bonus = m_goalBonus - m_goalNumber * 10000;
                player.Goal = true;
                player.GetMoney(bonus);
                m_goalNumber++;
                m_gamePanel.TextDisplay(player.Owner.Name + "��" + m_goalNumber + "�ʂŃS�[�������I\n�܋��Ƃ���" + bonus + "��������I");
                m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(player, m_professions);
                break;
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
        m_gamePanel.PlayerStatusBox.PlayerStatusBoxUpdata(m_players[m_order], m_professions);
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
                Debug.Log("�������[�v");
                break;
            }
        }
        player.MoveStart(mv, false, true, m_camera);
    }
    /// <summary>
    /// ���򓹂̍s������߂�
    /// </summary>
    /// <param name="player">���߂�v���C���[</param>
    /// <returns></returns>
    IEnumerator Branch(PlayerController player)
    {
        int r = player.Location.NextRoads.Length;
        m_roulette.gameObject.SetActive(true);
        string[] lineup;
        if (r == 2)
        {
            lineup = new string[] { "��", "�E" };
        }
        else
        {
            lineup = new string[] { "��", "�^��", "�E" };
        }
        m_roulette.GetBranchRoadLineup(lineup);
        yield return m_roulette.RouletteStart(false);
        player.BranchNumber = m_roulette.Number;
    }
    /// <summary>
    /// �t�F�[�h�𐧌䂷��
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
    /// �Q�[�����X�^�[�g������
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
