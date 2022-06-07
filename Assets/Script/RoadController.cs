using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>�C�x���g�̎��</summary>
public enum RoadEvents
{
    /// <summary>�i��</summary>
    Go = 0,
    /// <summary>�߂�</summary>
    Return = 1,
    /// <summary>�x��</summary>
    Rest = 2,
    /// <summary>���������炤</summary>
    GetMoney = 3,
    /// <summary>�����𕥂�</summary>
    PayMoney = 4,
    /// <summary>�A�E</summary>
    FindWork = 5,
    /// <summary>������</summary>
    Payday = 6,
    /// <summary>����</summary>
    Marriage = 7,
    /// <summary>�o�Y</summary>
    Childbirth = 8,
    /// <summary>����</summary>
    RoadBranch = 9,
    /// <summary>����</summary>
    PayRaise = 10,
    /// <summary>�S�[��</summary>
    Goal = 11,
}
public class RoadController : MonoBehaviour
{
    /// <summary>�C�x���g�̃X�e�[�g</summary>
    [SerializeField] RoadEvents m_event = RoadEvents.GetMoney;
    /// <summary>�C�x���g�̃p�����[�^�[</summary>
    [SerializeField] int m_eventParameter = 1000;
    /// <summary>�}�X�ԍ�</summary>
    [SerializeField] string m_roadNumber;
    /// <summary>�}�X�ԍ��\��</summary>
    [SerializeField] TextMesh m_roadNumberText = null;
    /// <summary>�Ԃ��~�܂�ꏊ</summary>
    [SerializeField] Transform m_stopPoint = null;
    /// <summary>���̃}�X�̐ڑ���</summary>
    [SerializeField] protected Transform[] m_nextConnect = null;
    /// <summary>�O�̃}�X�̐ڑ���</summary>
    [SerializeField] Transform[] m_prevConnect = null;
    /// <summary>���̃}�X</summary>
    [SerializeField] protected RoadController[] m_nextRoads = null;
    /// <summary>�O�̃}�X</summary>
    [SerializeField] protected RoadController[] m_prevRoads = null;
    /// <summary>�}�X�̃e�L�X�g</summary>
    [SerializeField] Text m_eventText = null;
    /// <summary>�X�g�b�v�}�X�̃t���O</summary>
    [SerializeField] bool m_stopFlag = false;
    /// <summary>�}�X�̐F��\������</summary>
    [SerializeField] GameObject m_roadColorDisplay = null;
    /// <summary>�}�X�̐F���</summary>
    [SerializeField] Material[] m_roadColors = null;
    /// <summary>�ʒu�␳�̃t���O</summary>
    bool m_positionCorrection = false;
    /// <summary>�Q�[���}�l�[�W���[</summary>
    protected GameManager m_gameManager;
    /// <summary>�O�̃}�X�̐ڑ����̃v���p�e�B</summary>
    public Transform[] PrevConnect => m_prevConnect;
    /// <summary>�}�X�ԍ��̃v���p�e�B</summary>
    public string RoadNumber
    {
        get => m_roadNumber;
        set
        {
            m_roadNumber = value;
            m_roadNumberText.text = m_roadNumber;
        }
    }
    /// <summary>�ʒu�␳�̃v���p�e�B</summary>
    public bool PositionCorrection => m_positionCorrection;
    /// <summary>�X�g�b�v�}�X�̃v���p�e�B</summary>
    public bool StopFlag => m_stopFlag;
    /// <summary>�Ԃ��~�܂�ꏊ�̃v���p�e�B</summary>
    public Transform StopPint => m_stopPoint;
    /// <summary>��O�̃}�X�̃v���p�e�B</summary>
    public RoadController PrevRoad => m_prevRoads[0];
    /// <summary>�C�x���g�̃v���p�e�B</summary>
    public RoadEvents Event => m_event;
    /// <summary>�C�x���g�p�����[�^�[�̃v���p�e�B</summary>
    public int EventParameter => m_eventParameter;

    void Start()
    {
        if (m_roadNumber != "")
        {
            m_roadNumberText.text = m_roadNumber;
        }
        if (m_stopFlag)
        {
            m_roadColorDisplay.GetComponent<Renderer>().material = m_roadColors[1];
        }
    }
    /// <summary>
    /// �}�X����ݒ肷��
    /// </summary>
    public virtual void RoadSetUp(RoadController road, string rn, GameManager gameManager)
    {
        m_gameManager = gameManager;
        if (RoadNumber == "")
        {
            RoadNumber = rn;
        }
        PrevRoadSet(road);
        //���̃}�X�ɏ��𑗂�
        if (m_nextRoads.Length == 0)
        {
            return;
        }
        if (!m_nextRoads[0].PositionCorrection)
        {
            Vector3 now = m_nextConnect[0].position;
            Vector3 next = m_nextRoads[0].PrevConnect[0].position;
            Vector3 a = m_nextRoads[0].gameObject.transform.position;
            m_nextRoads[0].PositionSetUp(a + (now - next));
        }
        m_gameManager.GetRoads(this);
        m_nextRoads[0].RoadSetUp(this, NextNumber(RoadNumber, 0), gameManager);
    }
    /// <summary>
    /// ��O�̃}�X��o�^����
    /// </summary>
    /// <param name="road">��O�̃}�X</param>
    protected void PrevRoadSet(RoadController road)
    {
        if (m_prevRoads.Length == 0)
        {
            m_prevRoads = new RoadController[1];
            m_prevRoads[0] = road;
        }
        else
        {
            RoadController[] prevRoads = m_prevRoads;
            m_prevRoads = new RoadController[prevRoads.Length + 1];
            int i = 0;
            while (i < m_prevRoads.Length - 1)
            {
                m_prevRoads[i] = prevRoads[i];
                i++;
            }
            m_prevRoads[i] = road;
        }
    }
    /// <summary>
    /// �}�X�ԍ���ݒ肷��
    /// </summary>
    /// <param name="rn">�O�̃}�X�̔ԍ�</param>
    protected virtual string NextNumber(string rn, int bn)
    {
        string[] sn = rn.Split(char.Parse("-"));
        int[] n = new int[sn.Length];
        for (int i = 0; i < n.Length; i++)
        {
            n[i] = int.Parse(sn[i]);
        }
        n[2]++;
        string an = "";
        for (int i = 0; i < n.Length; i++)
        {
            an += n[i].ToString();
            if (i < n.Length - 1)
            {
                an += char.Parse("-");
            }
        }
        return an;
    }
    /// <summary>
    /// �}�X�̈ʒu�𒲐߂���
    /// </summary>
    public void PositionSetUp(Vector3 p)
    {
        m_positionCorrection = true;
        this.transform.position = p;
    }
    /// <summary>
    /// ���̃}�X��Ԃ�
    /// </summary>
    /// <returns>���̃}�X</returns>
    public virtual RoadController NextRoad(int branch)
    {
        if (m_nextRoads.Length == 0)
        {
            return null;
        }
        return m_nextRoads[0];
    }

    /// <summary>
    /// �}�X�̃C�x���g�e�L�X�g��Ԃ�
    /// </summary>
    /// <returns>�C�x���g�e�L�X�g</returns>
    public string EventText()
    {
        string BText = m_eventText.text;
        string AText = "";
        foreach (var item in BText)
        {
            if (item == char.Parse("#"))
            {
                AText += m_eventParameter;
            }
            else
            {
                AText += item;
            }
        }
        return AText;
    }


}
