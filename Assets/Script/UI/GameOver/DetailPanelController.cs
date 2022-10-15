using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailPanelController : MonoBehaviour
{
    /// <summary>���O�̃e�L�X�g</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>�������̃e�L�X�g</summary>
    [SerializeField] Text m_moneyText = null;
    /// <summary>�Ƒ�</summary>
    [SerializeField] GameObject m_family = null;
    /// <summary>
    /// ����ݒ肷��
    /// </summary>
    /// <param name="player">�v���C���[</param>
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
    /// ������Ԃɂ��ĕ���
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
