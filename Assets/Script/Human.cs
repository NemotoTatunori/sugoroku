using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    /// <summary>���O</summary>
    [SerializeField] string m_name = null;
    /// <summary>����(t:�j,f:��)</summary>
    [SerializeField] bool m_seibetu;
    /// <summary>�E��</summary>
    [SerializeField] int m_profession = 0;
    /// <summary>���������N</summary>
    [SerializeField] int m_salaryRank = 0;
    /// <summary>�̂̐F�ꗗ</summary>
    [SerializeField] Material[] m_bodyColors = null;
    /// <summary>��</summary>
    [SerializeField] GameObject m_body;
    /// <summary>���O�̃v���p�e�B</summary>
    public string Name => m_name;
    /// <summary>���ʂ̃v���p�e�B</summary>
    public bool Seibetu => m_seibetu;
    /// <summary>�E�Ƃ̃v���p�e�B</summary>
    public int Profession
    {
        get => m_profession;
        set
        {
            m_profession = value;
        }
    }
    /// <summary>���������N�̃v���p�e�B</summary>
    public int SalaryRank
    {
        get => m_salaryRank;
        set
        {
            m_salaryRank = value;
        }
    }
    /// <summary>
    /// �l�̃Z�b�e�B���O
    /// </summary>
    /// <param name="seibetu">����</param>
    /// <param name="name">���O</param>
    /// <param name="pro">�E��</param>
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
