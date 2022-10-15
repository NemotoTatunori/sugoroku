using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddHumanPanelController : MonoBehaviour
{
    /// <summary>�Q�[���}�l�[�W���[</summary>
    GameManager m_gameManager;
    /// <summary>�C�x���g���e��\������e�L�X�g</summary>
    [SerializeField] Text m_eventNameText = null;
    /// <summary>�ǉ�����l�̖��O���󂯎��e�L�X�g</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>�����C�x���g�{�^��</summary>
    [SerializeField] GameObject m_fianceButton;
    /// <summary>�j�̎q�{�^��</summary>
    [SerializeField] GameObject m_boyButton;
    /// <summary>���̎q�{�^��</summary>
    [SerializeField] GameObject m_girlButton;
    /// <summary>�e�L�X�g�ɕ\�����镶�����</summary>
    string[] m_eventName = { "������", "�q�������܂ꂽ�I" };
    /// <summary>
    /// �\������p�l���̃Z�b�e�B���O
    /// </summary>
    /// <param name="p">�p�^�[��</param>
    public void Set(int p)
    {
        switch (p)
        {
            case 0:
                m_eventNameText.text = m_eventName[p];
                m_fianceButton.SetActive(true);
                m_boyButton.SetActive(false);
                m_girlButton.SetActive(false);
                break;
            case 1:
                m_eventNameText.text = m_eventName[p];
                m_fianceButton.SetActive(false);
                m_boyButton.SetActive(true);
                m_girlButton.SetActive(true);
                break;
        }
    }
    /// <summary>
    /// �l��ǉ�����{�^���ɂ���
    /// </summary>
    /// <param name="p">�p�^�[��</param>
    public void AddHumanButton(int p)
    {
        m_gameManager.AddHumanEvent(p, m_nameText.text);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// �Q�[���}�l�[�W���[���󂯎��
    /// </summary>
    /// <param name="gm">�Q�[���}�l�[�W���[</param>
    public void SetGameManager(GameManager gm)
    {
        m_gameManager = gm;
    }
}
