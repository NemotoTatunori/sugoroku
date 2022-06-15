using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RankingPlatePrefab : MonoBehaviour
{
    /// <summary>����</summary>
    int m_ranking;
    /// <summary>�v���C���[</summary>
    PlayerController m_player;
    /// <summary>���ʕ\���̃e�L�X�g</summary>
    [SerializeField] Text m_rankingText;
    /// <summary>���O�\���̃e�L�X�g</summary>
    [SerializeField] Text m_nameText;
    /// <summary>�ڍ׉�ʂ��ĂԊ֐�</summary>
    Action<PlayerController> m_detail;
    /// <summary>
    /// �����󂯎���Đݒ肷��
    /// </summary>
    /// <param name="r">����</param>
    /// <param name="p">�v���C���[</param>
    public void Seting(int r,PlayerController p,Action<PlayerController> a)
    {
        m_ranking = r;
        m_player = p;
        m_rankingText.text = m_ranking + "��";
        m_nameText.text = m_player.Owner.Name;
        m_detail = a;
    }
    public void DetailButton()
    {
        m_detail.Invoke(m_player);
    }
}
