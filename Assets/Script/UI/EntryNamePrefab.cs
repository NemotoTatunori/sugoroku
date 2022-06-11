using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EntryNamePrefab : MonoBehaviour
{
    /// <summary>���O</summary>
    [SerializeField] string m_name;
    /// <summary>����</summary>
    [SerializeField] bool m_seibetu;
    /// <summary>���O�̃e�L�X�g</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>���O�̃v���p�e�B</summary>
    public string Name => m_name;
    /// <summary>���ʂ̃v���p�e�B</summary>
    public bool Seibetu => m_seibetu;
    Action<GameObject> m_rename;
    /// <summary>
    /// �ݒ肷������󂯎��
    /// </summary>
    /// <param name="name">���O</param>
    /// <param name="seibetu">����</param>
    /// <param name="entry">�G���g���[�p�l��</param>
    public void Seting(string name, bool seibetu,Action<GameObject> action)
    {
        m_name = name;
        m_nameText.text = m_name;
        m_seibetu = seibetu;
        m_rename = action;
    }
    public void Destroy()
    {
        m_rename.Invoke(this.gameObject);
    }
}
