using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteController : MonoBehaviour
{
    /// <summary>���ڂ̃e�L�X�g</summary>
    [SerializeField] Text m_name = null;
    /// <summary>���ʂ�\������e�L�X�g</summary>
    [SerializeField] Text m_numberDisplay = null;
    /// <summary>�X�^�[�g�{�^��</summary>
    [SerializeField] GameObject m_StartButton = null;
    /// <summary>�X�g�b�v�{�^��</summary>
    [SerializeField] GameObject m_StopButton = null;
    /// <summary>�������؂�ւ�鎞��</summary>
    [SerializeField] float m_speed = 0.1f;
    /// <summary>�������B���W</summary>
    [SerializeField] GameObject m_lid = null;
    /// <summary>�I�΂�鐔���̔z��</summary>
    int[] m_lineup = { 1, 2, 3, 4, 5 };
    /// <summary>�I�΂�镶���̔z��</summary>
    string[] m_branchRoadLineup = { "��", "�E" };
    /// <summary>���[���b�g�n���̃t���O</summary>
    bool m_start = false;
    /// <summary>�o������</summary>
    int m_number = 0;
    public string Name
    {
        get => m_name.text;
        set
        {
            m_name.text = value;
        }
    }
    /// <summary>�o�������̃v���p�e�B</summary>
    public int Number => m_number;
    /// <summary>
    /// ���[���b�g���X�^�[�g������
    /// </summary>
    public Coroutine RouletteStart(bool f)
    {
        m_StartButton.SetActive(false);
        m_StopButton.SetActive(true);
        m_start = true;
        return StartCoroutine(Roulette(f));
    }
    /// <summary>
    /// �I�΂�鐔���̌����󂯎��
    /// </summary>
    /// <param name="lineup">���</param>
    public void SetLineup(int[] lineup)
    {
        m_lineup = lineup;
    }
    /// <summary>
    /// �I�΂�镪�򓹂̌����󂯎��
    /// </summary>
    /// <param name="lineup">���</param>
    public void SetBranchRoadLineup(string[] lineup)
    {
        m_branchRoadLineup = lineup;
    }
    /// <summary>
    /// ���[���b�g���~�߂�
    /// </summary>
    public void RouletteStop()
    {
        m_start = false;
        m_StopButton.SetActive(false);
    }
    /// <summary>
    /// ���[���b�g�𓮂���
    /// </summary>
    /// <param name="IntFlag">�C�x���g�̃t���O</param>
    /// <returns></returns>
    IEnumerator Roulette(bool IntFlag)
    {
        int loop = IntFlag ? m_lineup.Length - 1 : m_branchRoadLineup.Length - 1;
        m_lid.SetActive(true);
        int n = 0;
        float interval = 0;
        while (true)
        {
            if (interval > m_speed)
            {
                interval = 0;
                if (IntFlag)
                {
                    m_numberDisplay.text = m_lineup[n].ToString();
                }
                else
                {
                    m_numberDisplay.text = m_branchRoadLineup[n];
                }
                n++;
                if (n > loop)
                {
                    n = 0;
                }
            }
            if (!m_start) { break; }
            interval += Time.deltaTime;
            yield return null;
        }
        if (IntFlag)
        {
            m_numberDisplay.text = m_lineup[n].ToString();
            m_number = m_lineup[n];
        }
        else
        {
            m_numberDisplay.text = m_branchRoadLineup[n];
            m_number = n;
        }
        m_lid.SetActive(false);
        yield return new WaitForSeconds(1f);
        m_StartButton.SetActive(true);
        gameObject.SetActive(false);
    }
}
