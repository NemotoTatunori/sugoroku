using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryNamePrefab : MonoBehaviour
{
    /// <summary>名前</summary>
    [SerializeField] string m_name;
    /// <summary>性別</summary>
    [SerializeField] bool m_seibetu;
    /// <summary>名前のテキスト</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>名前のプロパティ</summary>
    public string Name
    {
        get => m_name;
        set
        {
            m_name = value;
            m_nameText.text = m_name;
        }
    }
    /// <summary>性別のプロパティ</summary>
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
