using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoadBranchPanelController : MonoBehaviour
{
    /// <summary>���s���{�^��</summary>
    [SerializeField] Button m_leftButton = null;
    /// <summary>�^���s���{�^��</summary>
    [SerializeField] Button m_centerButton = null;
    /// <summary>�E�s���{�^��</summary>
    [SerializeField] Button m_rightButton = null;
    /// <summary>�\���p�^�[��</summary>
    int m_pattern;
    /// <summary>�I��񍐊֐�</summary>
    Action<int> m_selectionBranch;
    /// <summary>
    /// �Ăяo���֐����󂯎��
    /// </summary>
    /// <param name="selectionBranch">�񍐊֐�</param>
    public void SetAction(Action<int> selectionBranch)
    {
        m_selectionBranch = selectionBranch;
    }
    /// <summary>
    /// UI���Z�b�e�B���O����
    /// </summary>
    /// <param name="pattern">�\���p�^�[��</param>
    public void Setting(int pattern)
    {
        m_pattern = pattern;
        switch (pattern)
        {
            case 0:
                m_centerButton.gameObject.SetActive(false);
                m_leftButton.onClick.AddListener(() => Answer(0));
                m_rightButton.onClick.AddListener(() => Answer(1));
                break;
            case 1:
                m_rightButton.gameObject.SetActive(false);
                m_leftButton.onClick.AddListener(() => Answer(0));
                m_centerButton.onClick.AddListener(() => Answer(1));
                break;
            case 2:
                m_leftButton.gameObject.SetActive(false);
                m_centerButton.onClick.AddListener(() => Answer(0));
                m_rightButton.onClick.AddListener(() => Answer(1));
                break;
            case 3:
                m_leftButton.onClick.AddListener(() => Answer(0));
                m_centerButton.onClick.AddListener(() => Answer(1));
                m_rightButton.onClick.AddListener(() => Answer(2));
                break;
        }
    }
    /// <summary>
    /// �s��̓������󂯎��
    /// </summary>
    /// <param name="answer">�s��</param>
    void Answer(int answer)
    {
        gameObject.SetActive(false);
        m_selectionBranch(answer);
        EventReset();
    }
    void EventReset()
    {
        switch (m_pattern)
        {
            case 0:
                m_centerButton.gameObject.SetActive(true);
                m_leftButton.onClick.RemoveAllListeners();
                m_rightButton.onClick.RemoveAllListeners();
                break;
            case 1:
                m_rightButton.gameObject.SetActive(true);
                m_leftButton.onClick.RemoveAllListeners();
                m_centerButton.onClick.RemoveAllListeners();
                break;
            case 2:
                m_leftButton.gameObject.SetActive(true);
                m_centerButton.onClick.RemoveAllListeners();
                m_rightButton.onClick.RemoveAllListeners();
                break;
            case 3:
                m_leftButton.onClick.RemoveAllListeners();
                m_centerButton.onClick.RemoveAllListeners();
                m_rightButton.onClick.RemoveAllListeners();
                break;
        }
    }
}
