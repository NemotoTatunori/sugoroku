using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour
{
    /// <summary>���O</summary>
    [SerializeField] string m_name = null;
    /// <summary>����</summary>
    [SerializeField] string m_seibetu = null;
    /// <summary>�̂̐F�ꗗ</summary>
    [SerializeField] Material[] m_bodyColors = null;
    /// <summary>��</summary>
    [SerializeField] GameObject m_body;
    void Start()
    {

    }
    /// <summary>
    /// �l�̃Z�b�e�B���O
    /// </summary>
    /// <param name="seibetu">����</param>
    /// <param name="name">���O</param>
    public void Seting(bool seibetu,string name)
    {
        m_name = name;
        if (seibetu)
        {
            m_seibetu = "�j";
            m_body.GetComponent<Renderer>().material = m_bodyColors[0];
        }
        else
        {
            m_seibetu = "��";
            m_body.GetComponent<Renderer>().material = m_bodyColors[1];
        }
    }
}
