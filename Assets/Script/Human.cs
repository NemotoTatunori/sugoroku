using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    /// <summary>名前</summary>
    [SerializeField] string m_name = null;
    /// <summary>性別</summary>
    [SerializeField] string m_seibetu = null;
    /// <summary>体の色一覧</summary>
    [SerializeField] Material[] m_bodyColors = null;
    /// <summary>体</summary>
    [SerializeField] GameObject m_body;
    void Start()
    {

    }
    /// <summary>
    /// 人のセッティング
    /// </summary>
    /// <param name="seibetu">性別</param>
    /// <param name="name">名前</param>
    public void Seting(bool seibetu,string name)
    {
        m_name = name;
        if (seibetu)
        {
            m_seibetu = "男";
            m_body.GetComponent<Renderer>().material = m_bodyColors[0];
        }
        else
        {
            m_seibetu = "女";
            m_body.GetComponent<Renderer>().material = m_bodyColors[1];
        }
    }
}
