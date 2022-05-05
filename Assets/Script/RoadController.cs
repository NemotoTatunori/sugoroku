using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoadController : MonoBehaviour
{
    /// <summary>�}�X�ԍ�</summary>
    [SerializeField] string m_roadNumber;
    /// <summary>�}�X�ԍ��\��</summary>
    [SerializeField] Text m_roadNumberText = null;
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
    [SerializeField] string m_eventText = null;
    /// <summary>�X�g�b�v�}�X�̃t���O</summary>
    [SerializeField] bool m_stopFlag = false;
    /// <summary>�ʒu�␳�̃t���O</summary>
    bool m_positionCorrection = false;
    /// <summary>�Q�[���}�l�[�W���[</summary>
    protected GameManager m_gameManager;
    /// <summary>�O�̃}�X�̐ڑ����̃v���p�e�B</summary>
    public Transform[] PrevConnect
    {
        get => m_prevConnect;
    }
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
    public bool PositionCorrection
    {
        get => m_positionCorrection;
    }
    /// <summary>�X�g�b�v�}�X�̃v���p�e�B</summary>
    public bool StopFlag
    {
        get => m_stopFlag;
    }
    /// <summary>�Ԃ��~�܂�ꏊ�̃v���p�e�B</summary>
    public Transform StopPint
    {
        get => m_stopPoint;
    }
    /// <summary>��O�̃}�X�̃v���p�e�B</summary>
    public RoadController PrevRoad
    {
        get => m_prevRoads[0];
    }

    void Start()
    {
        if (m_roadNumber != "")
        {
            m_roadNumberText.text = m_roadNumber;
        }
    }
    /// <summary>
    /// �}�X����ݒ肷��
    /// </summary>
    public virtual void RoadSetUp(RoadController road, string rn)
    {
        m_gameManager = FindObjectOfType<GameManager>();
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
        m_nextRoads[0].RoadSetUp(this, NextNumber(RoadNumber,0));
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
    public virtual RoadController NextRoad()
    {
        if (m_nextRoads.Length == 0)
        {
            return null;
        }
        return m_nextRoads[0];
    }
}
