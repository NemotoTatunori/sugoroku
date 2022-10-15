using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoadBranchPanelController : MonoBehaviour
{
    /// <summary>左行きボタン</summary>
    [SerializeField] Button m_leftButton = null;
    /// <summary>真中行きボタン</summary>
    [SerializeField] Button m_centerButton = null;
    /// <summary>右行きボタン</summary>
    [SerializeField] Button m_rightButton = null;
    /// <summary>表示パターン</summary>
    int m_pattern;
    /// <summary>選択報告関数</summary>
    Action<int> m_selectionBranch;
    /// <summary>
    /// 呼び出す関数を受け取る
    /// </summary>
    /// <param name="selectionBranch">報告関数</param>
    public void SetAction(Action<int> selectionBranch)
    {
        m_selectionBranch = selectionBranch;
    }
    /// <summary>
    /// UIをセッティングする
    /// </summary>
    /// <param name="pattern">表示パターン</param>
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
    /// 行先の答えを受け取る
    /// </summary>
    /// <param name="answer">行先</param>
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
