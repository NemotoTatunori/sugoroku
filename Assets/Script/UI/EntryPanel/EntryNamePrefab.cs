using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryNamePrefab : MonoBehaviour
{
    /// <summary>���O</summary>
    [SerializeField] string m_name;
    /// <summary>����</summary>
    [SerializeField] bool m_seibetu;
    /// <summary>���O�̃e�L�X�g</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>�G���g���[�p�l��</summary>
    EntryPanelController m_entryPanel;
    /// <summary>���O�̃v���p�e�B</summary>
    public string Name => m_name;
    /// <summary>���ʂ̃v���p�e�B</summary>
    public bool Seibetu => m_seibetu;
    /// <summary>
    /// �ݒ肷������󂯎��
    /// </summary>
    /// <param name="name">���O</param>
    /// <param name="seibetu">����</param>
    /// <param name="entry">�G���g���[�p�l��</param>
    public void Seting(string name,bool seibetu,EntryPanelController entry)
    {
        m_name = name;
        m_nameText.text = m_name;
        m_seibetu = seibetu;
        m_entryPanel = entry;
    }
    public void Destroy()
    {
        m_entryPanel.RemoveName(this.gameObject);
    }
}
