using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>�v���C���[�̖��O</summary>
    [SerializeField] string m_name = null;
    /// <summary>������</summary>
    [SerializeField] int m_money = 0;
    /// <summary>���݈ʒu</summary>
    [SerializeField] string m_location = null;
    /// <summary>���Ȃ̈ʒu</summary>
    [SerializeField] Transform[] m_Seats = null;
    /// <summary>���Ȃɐl�����邩�̃t���O</summary>
    [SerializeField] bool[] m_sittings = null;
    /// <summary>����Ă���l����</summary>
    [SerializeField] Human[] m_family;
    /// <summary>����Ă���l��</summary>
    int m_familyNum = 0;
    /// <summary>�l�̃v���n�u</summary>
    [SerializeField] GameObject m_humanPrefab = null;
    
    public void Set()
    {
        m_sittings = new bool[m_Seats.Length];
        m_family = new Human[m_Seats.Length];
        for (int i = 1; i < m_sittings.Length; i++)
        {
            m_sittings[i] = false;
        }
        m_sittings[0] = true;
        
    }
    /// <summary>
    /// �Ƒ��𑝂₷
    /// </summary>
    /// <param name="seibetu">����</param>
    /// <param name="name">���O</param>
    public void AddHuman(bool seibetu, string name)
    {
        m_familyNum++;
        GameObject h = Instantiate(m_humanPrefab, m_Seats[m_familyNum - 1]);
        m_family[m_familyNum - 1] = h.GetComponent<Human>();
        m_family[m_familyNum - 1].Seting(seibetu,name);
    }

    public void MoveStart(int m)
    {
        StartCoroutine(Move(m));
    }

    IEnumerator Move(int m)
    {
        yield return null;
    }
}
