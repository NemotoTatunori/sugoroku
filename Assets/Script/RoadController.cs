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
    [SerializeField] Transform[] m_nextConnect = null;
    /// <summary>�O�̃}�X�̐ڑ���</summary>
    [SerializeField] Transform m_prevConnect = null;
    /// <summary>���̃}�X</summary>
    [SerializeField] RoadController[] m_nextRoads = null;
    /// <summary>�O�̃}�X</summary>
    [SerializeField] RoadController m_prevRoads = null;
    /// <summary>�}�X�̃e�L�X�g</summary>
    [SerializeField] string m_eventText = null;

    public Transform PrevConnect
    {
        get => m_prevConnect;
    }

    void Start()
    {
        m_roadNumberText.text = m_roadNumber;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// �}�X�̈ʒu�𒲐߂���
    /// </summary>
    public void PositionSetUp()
    {
        if (m_nextRoads == null)
        {
            return;
        }
        for (int i = 0; i < m_nextRoads.Length; i++)
        {
            Vector3 now = m_nextConnect[i].position;
            Vector3 next = m_nextRoads[i].PrevConnect.position;
            Vector3 a = m_nextRoads[i].gameObject.transform.position;
            m_nextRoads[i].gameObject.transform.position = a + (now - next);
            m_nextRoads[i].PositionSetUp();
        }
    }
}
