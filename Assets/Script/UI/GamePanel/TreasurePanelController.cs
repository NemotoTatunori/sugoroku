using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasurePanelController : MonoBehaviour
{
    /// <summary>表示する名前</summary>
    [SerializeField] Text m_nameBox = null;
    /// <summary>所持しているお宝一覧表示場所</summary>
    [SerializeField] GameObject m_TreasureBox = null;
    /// <summary>お宝データ</summary>
    [SerializeField] TreasureData m_treasureData = null;
    /// <summary>表示するプレハブ</summary>
    [SerializeField] GameObject m_treasureImage = null;
    /// <summary>お宝データのプロパティ</summary>
    public TreasureData TreasureData => m_treasureData;
    /// <summary>
    /// UIをセッティングする
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="treasures">お宝リスト</param>
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
