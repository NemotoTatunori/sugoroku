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
    /// <summary>�ő�l��</summary>
    private int m_maxEntry = 30;
    Coroutine m_coroutine;
    /// <summary>�t�F�[�h�p�l��</summary>
    [SerializeField] Image m_fadePanel = null;

    //�G���g���[�p�l���Ŏg���ϐ�
    /// <summary>�Q������l�̖��O����͂���ꏊ</summary>
    [SerializeField] Text m_entryName = null;
    /// <summary>�Q������l�̖��O��\������v���n�u</summary>
    [SerializeField] GameObject m_entryNamePrehab = null;
    /// <summary>�Q������l�̖��O��\������ꏊ</summary>
    [SerializeField] GameObject m_entryNameDisplay = null;
    /// <summary>�Q�����̃e�L�X�g</summary>
    [SerializeField] Text m_peopleNumText = null;
    /// <summary>�G���g���[��</summary>
    int m_peopleNum = 0;
    /// <summary>�x���e�L�X�g</summary>
    [SerializeField] GameObject m_caveatText = null;
    /// <summary>�G���g���[�p�l��</summary>
    [SerializeField] GameObject m_entryPanel = null;
    /// <summary>�j�̐F�i���F�j</summary>
    Color m_manColor = new Color(0.6f, 1, 1);
    /// <summary>���̐F�i�s���N�j</summary>
    Color m_womanColor = new Color(1, 0.6f, 1);


    //�Q�[���Ŏg���ϐ�
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
    [SerializeField] PlayerController[] m_players = null;
    /// <summary>�}�X�̔z��</summary>
    RoadController[,,] m_Roads;
    /// <summary>��Ԃ��Ǘ�����</summary>
    [SerializeField] int m_order = 0;
    /// <summary>���[���b�g</summary>
    [SerializeField] RouletteController m_roulette = null;
    /// <summary>���[���b�g�̃f�t�H���g��</summary>
    int[] m_rouletteLineupDefault = { 1, 2, 3, 4, 5 };
    /// <summary>�e�L�X�g��\������</summary>
    [SerializeField] Text m_progressText = null;
    /// <summary>���O��\������</summary>
    [SerializeField] Text m_playerStatusNameBoxText = null;
    /// <summary>��������\������</summary>
    [SerializeField] Text m_playerStatusMoneyBoxText = null;
    /// <summary>�E�Ƃ�\������</summary>
    [SerializeField] Text m_playerStatusProfessionBoxText = null;
    /// <summary>���������N��\������</summary>
    [SerializeField] Text m_playerStatusSalaryRankBoxText = null;
    /// <summary>�Ԃɐl��ǉ�����p�l��</summary>
    [SerializeField] AddHumanPanelController m_addHumanPanel = null;
    /// <summary>�A�E�p�l��</summary>
    [SerializeField] GameObject m_findWorkPanel = null;
    /// <summary>�A�E�p�l���̕\���e�L�X�g</summary>
    [SerializeField] Text m_findWorkText = null;
    /// <summary>�^�[���G���h�{�^��</summary>
    [SerializeField] GameObject m_turnEndButton = null;
    /// <summary>�i�s�̃X�e�[�g</summary>
    ProgressState m_state = ProgressState.FadeIn;

    void Start()
    {
        m_addHumanPanel.GetGameManager(this);
        m_roulette.GetGameManager(this);
        m_Roads = new RoadController[5, 2, 20];
        m_first.RoadSetUp(null, m_first.RoadNumber, this);
    }

    void Update()
    {

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
                PlayerStatusBoxUpdata();
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
    /// �v���C���[�X�e�[�^�X�{�b�N�X���X�V����
    /// </summary>
    void PlayerStatusBoxUpdata()
    {
        PlayerController p = m_players[m_order];
        m_playerStatusNameBoxText.text = p.Owner.Name;
        m_playerStatusMoneyBoxText.text = p.Money.ToString();
        m_playerStatusProfessionBoxText.text = m_professions[p.Profession];
        m_playerStatusSalaryRankBoxText.text = p.SalaryRank.ToString();
    }
    /// <summary>
    /// �\������e�L�X�g���X�V���ĕ\������
    /// </summary>
    /// <param name="t"></param>
    void TextDisplay(string t)
    {
        m_progressText.transform.parent.gameObject.SetActive(true);
        m_progressText.text = t;
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
                    TextDisplay(m_players[m_order].Owner.Name + "����̔Ԃł�");
                }
                else
                {
                    m_state = ProgressState.TurnEnd;
                    TextDisplay(m_players[m_order].Owner.Name + "����͂��x�݂̂悤�ł�");
                    m_players[m_order].Rest = false;
                }
                break;
            case ProgressState.TurnText:
                m_progressText.transform.parent.gameObject.SetActive(false);
                m_state = ProgressState.Roulette;
                m_roulette.gameObject.SetActive(true);
                m_roulette.RouletteStart();
                break;
            case ProgressState.Roulette:
                m_state = ProgressState.PlayerMove;
                m_players[m_order].MoveStart(m_roulette.Number, false, false, m_camera);
                break;
            case ProgressState.PlayerMove:
                m_state = ProgressState.RoadText;
                TextDisplay(m_players[m_order].Location.EventText());
                break;
            case ProgressState.RoadText:
                m_progressText.transform.parent.gameObject.SetActive(false);
                m_state = ProgressState.Event;
                RoadEvent(m_players[m_order]);
                break;
            case ProgressState.Event:
                m_state = ProgressState.TurnEnd;
                m_progressText.transform.parent.gameObject.SetActive(false);
                m_camera.Move = true;
                m_turnEndButton.SetActive(true);
                break;
            case ProgressState.TurnEnd:
                m_turnEndButton.SetActive(false);
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
                TextDisplay(player.Owner.Name + "�����" + road.EventParameter + "�����I");
                PlayerStatusBoxUpdata();
                break;
            case RoadEvents.PayMoney:
                player.GetMoney(-road.EventParameter);
                TextDisplay(player.Owner.Name + "�����" + road.EventParameter + "�x������");
                PlayerStatusBoxUpdata();
                break;
            case RoadEvents.FindWork:
                m_findWorkText.text = m_professions[road.EventParameter] + "�ɂȂ�܂����H\n\n���Ȃ�ƕ���I���܂Ői��";
                m_findWorkPanel.SetActive(true);
                break;
            case RoadEvents.Payday:
                player.GetMoney(Salary(player.Profession, player.SalaryRank));
                TextDisplay(player.Owner.Name + "����͋����Ƃ���" + m_salarys[player.SalaryRank, player.Profession] + "�����I");
                PlayerStatusBoxUpdata();
                player.PaydayFlag = false;
                break;
            case RoadEvents.Marriage:
                m_addHumanPanel.gameObject.SetActive(true);
                m_addHumanPanel.Set(0);
                break;
            case RoadEvents.Childbirth:
                m_addHumanPanel.gameObject.SetActive(true);
                m_addHumanPanel.Set(1);
                break;
            case RoadEvents.RoadBranch:
                StartCoroutine(Branch(player));
                break;
            case RoadEvents.PayRaise:
                player.SalaryRank = player.SalaryRank + 1;
                TextDisplay(player.Owner.Name + "����̋��������N���オ�����I");
                PlayerStatusBoxUpdata();
                break;
            case RoadEvents.Goal:
                int bonus = m_goalBonus - m_goalNumber * 10000;
                player.Goal = true;
                player.GetMoney(bonus);
                m_goalNumber++;
                TextDisplay(player.Owner.Name + "��" + m_goalNumber + "�ʂŃS�[�������I\n�܋��Ƃ���" + bonus + "��������I");
                PlayerStatusBoxUpdata();
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
                TextDisplay(m_players[m_order].Owner.Name + "����͌��������I");
                break;
            case 1:
                m_players[m_order].AddHuman(true, name);
                TextDisplay(m_players[m_order].Owner.Name + "�����ɒj�̎q�����܂ꂽ�I");
                break;
            case 2:
                m_players[m_order].AddHuman(false, name);
                TextDisplay(m_players[m_order].Owner.Name + "�����ɏ��̎q�����܂ꂽ�I");
                break;
        }
        Congratulations(m_players[m_order], m_players[m_order].Location.EventParameter);
        PlayerStatusBoxUpdata();
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
        m_findWorkPanel.SetActive(false);
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
        m_roulette.gameObject.SetActive(true);
        int[] lineup = { 0, 1 };
        m_roulette.GetLineup(lineup);
        yield return m_roulette.RouletteStart();
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

    //�G���g���[�p�l���Ŏg�����\�b�h
    /// <summary>
    /// �Q�[�����X�^�[�g������
    /// </summary>
    public void GameStart(PlayerController[] players)
    {
        m_players = players;
        m_state = ProgressState.PlayerCheck;
        m_order = 0;
        PlayerStatusBoxUpdata();
        StartCoroutine(Fade(false));
        m_entryPanel.SetActive(false);
    }
    /// <summary>
    /// �G���g���[������
    /// </summary>
    /// <param name="seibetu">����</param>
    public void AddName(bool seibetu)
    {
        if (m_peopleNum >= m_maxEntry)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
            m_coroutine = StartCoroutine(Caveat("�ő�" + m_maxEntry + "�l�܂ŁI"));
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
        m_peopleNumText.text = m_peopleNum + "�l";
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
    /// �G���g���[��������
    /// </summary>
    /// <param name="name">�l�[���v���n�u</param>
    public void RemoveName(GameObject name)
    {
        m_peopleNum--;
        Destroy(name);
        m_peopleNumText.text = m_peopleNum + "�l";
    }
    /// <summary>
    /// �x���e�L�X�g�\��
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
