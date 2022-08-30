using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasurePanelController : MonoBehaviour
{
    /// <summary>�\�����閼�O</summary>
    [SerializeField] Text m_nameBox = null;
    /// <summary>�������Ă��邨��ꗗ�\���ꏊ</summary>
    [SerializeField] GameObject m_TreasureBox = null;
    /// <summary>����f�[�^</summary>
    [SerializeField] TreasureData m_treasureData = null;
    /// <summary>�\������v���n�u</summary>
    [SerializeField] GameObject m_treasureImage = null;
    /// <summary>����f�[�^�̃v���p�e�B</summary>
    public TreasureData TreasureData => m_treasureData;
    /// <summary>
    /// UI���Z�b�e�B���O����
    /// </summary>
    /// <param name="name">���O</param>
    /// <param name="treasures">���󃊃X�g</param>
    public void Setting(string name,List<int> treasures)
    {
        m_nameBox.text = name;
        foreach (var item in treasures)
        {
            GameObject tre = Instantiate(m_treasureImage,m_TreasureBox.transform);
            tre.transform.GetChild(0).GetComponent<Text>().text = m_treasureData.GetData(item).TreasureName.ToString();
            tre.transform.GetChild(1).GetComponent<Text>().text = m_treasureData.GetData(item).Price.ToString();
        }
    }
    public void Reset()
    {
        m_nameBox.text = "";
        for (int i = 0; i < m_TreasureBox.transform.childCount; i++)
        {
            Destroy(m_TreasureBox.transform.GetChild(i).gameObject);
        }
    }
}
