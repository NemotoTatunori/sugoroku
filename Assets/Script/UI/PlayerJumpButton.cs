using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerJumpButton : MonoBehaviour
{
    /// <summary>名前のテキスト</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>移動先のポイント</summary>
    PlayerController m_player;
    /// <summary>呼ぶ関数</summary>
    Action<Vector3> m_action;
    /// <summary>
    /// 情報を設定する
    /// </summary>
    /// <param name="n">名前</param>
    /// <param name="p">場所</param>
    /// <param name="a">関数</param>
    public void Setting(PlayerController p, Action<Vector3> a)
    {
        m_player = p;
        m_nameText.text = m_player.Owner.Name;
        m_action = a;
    }
    /// <summary>
    /// ジャンプ
    /// </summary>
    public void Jump()
    {
        m_action.Invoke(m_player.transform.position);
    }
}
