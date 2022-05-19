using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>�i�s�̃X�e�[�g</summary>
    enum ProgressState
    {
        FadeIn = 0,
        TurnText = 1,
        Roulette = 2,
        PlayerMove = 3,
        RoadText = 4,
        Event = 5,
        TurnEnd = 6,
        FadeOut = 7,
        PlayerCheck = 8,
    }
    /// <summary>�ő�l��</summary>
    int m_maxEntry = 30;
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
    /// <summary>�j�̐F�i���F�j</summary>
    Color m_manColor = new Color(0.6f, 1, 1);
    /// <summary>���̐F�i�s���N�j</summary>
    Color m_womanColor = new Color(1, 0.6f, 1);
    /// <summary>�x���e�L�X�g</summary>
    [SerializeField] GameObject m_caveatText = null;
    /// <summary>�G���g���[�p�l��</summary>
    [SerializeField] GameObject m_entryPanel = null;


    //�Q�[���Ŏg���ϐ�
    /// <summary>�E�ƈꗗ</summary>
    string[] m_professions = { "�t���[�^�[", "�X�|�[�c�I��", "�v���O���}�[", "�p�e�B�V�G", "�����l", "��H" };
    /// <summary>�����ꗗ</summary>
    int[,] m_salarys = {
        {5000, 10000, 11000, 12000, 13000, 14000 },
        {10000, 30000, 29000, 28000, 27000, 26000 } };
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
    public int Salary(int profession, int salaryRank)
    {
        return m_salarys[profession, salaryRank];
    }
    /// <summary>
    /// �v���C���[�X�e�[�^�X�{�b�N�X���X�V����
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
    /// �\������e�L�X�g���X�V���ĕ\������
    /// </summary>
    /// <param name="t"></param>
    public void TextDisplay(string t)
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
                    TextDisplay(m_players[m_order].OwnerName + "����̔Ԃł�");
                }
                else
                {
                    m_state = ProgressState.TurnEnd;
                    TextDisplay(m_players[m_order].OwnerName + "����͂��x�݂̂悤�ł�");
                    m_players[m_order].Rest = false;
                }
                break;
            case ProgressState.TurnText:
                m_progressText.transform.parent.gameObject.SetActive(false);
                m_state = ProgressState.Roulette;
                m_camera.Move = true;
                m_roulette.gameObject.SetActive(true);
                m_roulette.RouletteStart();
                break;
            case ProgressState.Roulette:
                m_state = ProgressState.PlayerMove;
                m_players[m_order].MoveStart(m_roulette.Number, false, false);
                break;
            case ProgressState.PlayerMove:
                m_state = ProgressState.RoadText;
                TextDisplay(m_players[m_order].Location.EventText());
                break;
            case ProgressState.RoadText:
                Debug.Log(m_state);
                m_progressText.transform.parent.gameObject.SetActive(false);
                m_state = ProgressState.Event;
                StartCoroutine(RoadEvent(m_players[m_order], m_players[m_order].Location));
                Debug.Log(m_state);
                break;
            case ProgressState.Event:
                Debug.Log(m_state);
                m_state = ProgressState.TurnEnd;
                TextDisplay(m_players[m_order].OwnerName + "����̔Ԃ͏I���ł�");
                break;
            case ProgressState.TurnEnd:
                Debug.Log(m_state);
                m_progressText.transform.parent.gameObject.SetActive(false);
                m_state = ProgressState.FadeOut;
                StartCoroutine(Fade(false));
                break;
            case ProgressState.FadeOut:
                Debug.Log(m_state);
                m_state = ProgressState.PlayerCheck;
                TurnChange();
                PlayerController p = m_players[m_order];
                p.transform.position = p.Location.StopPint.position;
                m_camera.PositionSet(p.transform.position);
                m_camera.Move = false;
                break;
            case ProgressState.PlayerCheck:
                Debug.Log(m_state);
                m_state = ProgressState.FadeIn;
                StartCoroutine(Fade(true));
                break;
        }
    }
    /// <summary>
    /// �}�X�̃C�x���g
    /// </summary>
    /// <returns></returns>
    public virtual IEnumerator RoadEvent(PlayerController player, RoadController road)
    {
        switch (road.Event)
        {
            case RoadEvents.Go:
                yield return player.MoveStart(road.EventParameter, false, true);
                break;
            case RoadEvents.Return:
                yield return player.MoveStart(road.EventParameter, true, true);
                break;
            case RoadEvents.Rest:
                player.Rest = true;
                Progress();
                break;
            case RoadEvents.GetMoney:
                player.GetMoney(road.EventParameter);
                
                Progress();
                break;
            case RoadEvents.PayMoney:
                player.GetMoney(road.EventParameter * -1);
                Progress();
                break;
            case RoadEvents.FindWork:
                player.Profession = road.EventParameter;
                Progress();
                break;
            case RoadEvents.Payday:
                player.GetMoney(Salary(player.Profession, player.SalaryRank));
                player.PaydayFlag = false;
                Progress();
                break;
            case RoadEvents.Marriage:
                Progress();
                break;
            case RoadEvents.Childbirth:
                Progress();
                break;
            case RoadEvents.RoadBranch:
                yield return Branch(player);
                Progress();
                break;
            case RoadEvents.Goal:
                player.Goal = true;
                Progress();
                break;
        }
        yield return null;
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
    public void GameStart()
    {
        if (m_peopleNum == 0)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
            m_coroutine = StartCoroutine(Caveat("�P�l�����Ȃ���I"));
            return;
        }
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
            m_players[i].Seting(en.Name, m_first, this);
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
        m_state = ProgressState.FadeOut;
        m_order = m_players.Length;
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
