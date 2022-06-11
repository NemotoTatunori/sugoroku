using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EntryNamePrefab : MonoBehaviour
{
    /// <summary>名前</summary>
    [SerializeField] string m_name;
    /// <summary>性別</summary>
    [SerializeField] bool m_seibetu;
    /// <summary>名前のテキスト</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>名前のプロパティ</summary>
    public string Name => m_name;
    /// <summary>性別のプロパティ</summary>
    public bool Seibetu => m_seibetu;
    Action<GameObject> m_rename;
    /// <summary>
    /// 設定する情報を受け取る
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="seibetu">性別</param>
    /// <param name="entry">エントリーパネル</param>
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
