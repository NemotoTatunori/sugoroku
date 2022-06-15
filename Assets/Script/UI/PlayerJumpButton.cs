using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerJumpButton : MonoBehaviour
{
    /// <summary>���O�̃e�L�X�g</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>�ړ���̃|�C���g</summary>
    PlayerController m_player;
    /// <summary>�ĂԊ֐�</summary>
    Action<Vector3> m_action;
    /// <summary>
    /// ����ݒ肷��
    /// </summary>
    /// <param name="n">���O</param>
    /// <param name="p">�ꏊ</param>
    /// <param name="a">�֐�</param>
    public void Setting(PlayerController p, Action<Vector3> a)
    {
        m_player = p;
        m_nameText.text = m_player.Owner.Name;
        m_action = a;
    }
    /// <summary>
    /// �W�����v
    /// </summary>
    public void Jump()
    {
        m_action.Invoke(m_player.transform.position);
    }
}
