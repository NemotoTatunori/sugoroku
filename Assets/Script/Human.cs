using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    /// <summary>名前</summary>
    [SerializeField] string m_name = null;
    /// <summary>性別(t:男,f:女)</summary>
    [SerializeField] bool m_seibetu;
    /// <summary>職業</summary>
    [SerializeField] int m_profession = 0;
    /// <summary>給料ランク</summary>
    [SerializeField] int m_salaryRank = 0;
    /// <summary>体の色一覧</summary>
    [SerializeField] Material[] m_bodyColors = null;
    /// <summary>体</summary>
    [SerializeField] GameObject m_body;
    /// <summary>名前のプロパティ</summary>
    public string Name => m_name;
    /// <summary>性別のプロパティ</summary>
    public bool Seibetu => m_seibetu;
    /// <summary>職業のプロパティ</summary>
    public int Profession
    {
        get => m_profession;
        set
        {
            m_profession = value;
        }
    }
    /// <summary>給料ランクのプロパティ</summary>
    public int SalaryRank
    {
        get => m_salaryRank;
        set
        {
            m_salaryRank = value;
        }
    }
    /// <summary>
    /// 人のセッティング
    /// </summary>
    /// <param name="seibetu">性別</param>
    /// <param name="name">名前</param>
    /// <param name="pro">職業</param>
    public void Setting(bool seibetu, string name,int pro)
    {
        m_name = name;
        m_seibetu = seibetu;
        m_profession = pro;
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
