using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteController : MonoBehaviour
{
    /// <summary>������\������e�L�X�g</summary>
    [SerializeField] Text m_numberDisplay = null;
    /// <summary>�X�^�[�g�{�^��</summary>
    [SerializeField] GameObject m_StartButton = null;
    /// <summary>�X�g�b�v�{�^��</summary>
    [SerializeField] GameObject m_StopButton = null;
    /// <summary>�������؂�ւ�鎞��</summary>
    [SerializeField] float m_speed = 0.1f;
    /// <summary>�I�΂�鐔���̔z��</summary>
    int[] m_lineup = { 1, 2, 3, 4, 5 };//, 6, 7, 8, 9, 10
    /// <summary>���[���b�g�n���̃t���O</summary>
    bool m_start = false;
    /// <summary>�o������</summary>
    int m_number = 0;

    public int Number
    {
        get => m_number;
    }

    /// <summary>
    /// ���[���b�g���X�^�[�g������
    /// </summary>
    public Coroutine RouletteStart()
    {
        m_StartButton.SetActive(false);
        m_StopButton.SetActive(true);
        m_start = true;
        return StartCoroutine(Roulette());

    }
    /// <summary>
    /// �I�΂�鐔���̌����󂯎��
    /// </summary>
    /// <param name="lineup">���</param>
    public void GetLineup(int[] lineup)
    {
        m_lineup = lineup;
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
    /// <returns></returns>
    IEnumerator Roulette()
    {
        int n = 0;
        float interval = 0;
        while (true)
        {
            if (!m_start) { break; }
            if (interval > m_speed)
            {
                interval = 0;
                m_numberDisplay.text = m_lineup[n].ToString();
                n++;
                if (n > m_lineup.Length - 1)
                {
                    n = 0;
                }
            }
            interval += Time.deltaTime;
            yield return null;
        }
        m_numberDisplay.text = m_lineup[n].ToString();
        m_number = m_lineup[n];
        yield return new WaitForSeconds(1f);
        m_StartButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
