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
    /// <summary>���O�̃v���p�e�B</summary>
    public string Name
    {
        get => m_name;
        set
        {
            m_name = value;
            m_nameText.text = m_name;
        }
    }
    /// <summary>���ʂ̃v���p�e�B</summary>
    public bool Seibetu
    {
        get => m_seibetu;
        set
        {
            m_seibetu = value;
        }
    }

    public void Destroy()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.RemoveName(this.gameObject);
    }
}
