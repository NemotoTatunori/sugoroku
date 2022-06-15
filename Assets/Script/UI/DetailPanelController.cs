using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanelController : MonoBehaviour
{
    /// <summary>名前のテキスト</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>所持金のテキスト</summary>
    [SerializeField] Text m_moneyText = null;
    /// <summary>家族</summary>
    [SerializeField] GameObject m_family = null;
    /// <summary>
    /// 情報を設定する
    /// </summary>
    /// <param name="player">プレイヤー</param>
    public void Setting(PlayerController player)
    {
        m_nameText.text = player.Owner.Name;
        m_moneyText.text = player.Money.ToString();
        for (int i = 0; i < player.FamilyNum; i++)
        {
            m_family.transform.GetChild(i).GetComponent<Text>().text = player.Family[i].Name;
        }
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 初期状態にして閉じる
    /// </summary>
    public void Close()
    {
        m_nameText.text = "";
        m_moneyText.text = "";
        for (int i = 0; i < m_family.transform.childCount; i++)
        {
            m_family.transform.GetChild(i).GetComponent<Text>().text = "";
        }
        gameObject.SetActive(false);
    }
}
