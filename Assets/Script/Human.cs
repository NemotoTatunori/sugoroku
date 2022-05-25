using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    /// <summary>���O</summary>
    [SerializeField] string m_name = null;
    /// <summary>����(t:�j,f:��)</summary>
    [SerializeField] bool m_seibetu;
    /// <summary>�̂̐F�ꗗ</summary>
    [SerializeField] Material[] m_bodyColors = null;
    /// <summary>��</summary>
    [SerializeField] GameObject m_body;
    /// <summary>���O�̃v���p�e�B</summary>
    public string Name => m_name;
    /// <summary>���ʂ̃v���p�e�B</summary>
    public bool Seibetu => m_seibetu;
    /// <summary>
    /// �l�̃Z�b�e�B���O
    /// </summary>
    /// <param name="seibetu">����</param>
    /// <param name="name">���O</param>
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
