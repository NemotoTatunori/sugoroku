using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    /// <summary>名前</summary>
    [SerializeField] string m_name = null;
    /// <summary>性別(t:男,f:女)</summary>
    [SerializeField] bool m_seibetu;
    /// <summary>体の色一覧</summary>
    [SerializeField] Material[] m_bodyColors = null;
    /// <summary>体</summary>
    [SerializeField] GameObject m_body;
    /// <summary>名前のプロパティ</summary>
    public string Name => m_name;
    /// <summary>性別のプロパティ</summary>
    public bool Seibetu => m_seibetu;
    /// <summary>
    /// 人のセッティング
    /// </summary>
    /// <param name="seibetu">性別</param>
    /// <param name="name">名前</param>
    public void Seting(bool seibetu,string name)
    {
        m_name = name;
        m_seibetu = seibetu;
        if (m_seibetu)
        {
            m_body.GetComponent<Renderer>().material = m_bodyColors[0];
        }
        else
        {
            m_body.GetComponent<Renderer>().material = m_bodyColors[1];
        }
    }
}
