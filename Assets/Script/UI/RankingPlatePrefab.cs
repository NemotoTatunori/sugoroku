using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RankingPlatePrefab : MonoBehaviour
{
    /// <summary>順位</summary>
    int m_ranking;
    /// <summary>プレイヤー</summary>
    PlayerController m_player;
    /// <summary>順位表示のテキスト</summary>
    [SerializeField] Text m_rankingText;
    /// <summary>名前表示のテキスト</summary>
    [SerializeField] Text m_nameText;
    Action m_detail;
    /// <summary>
    /// 情報を受け取って設定する
    /// </summary>
    /// <param name="r">順位</param>
    /// <param name="p">プレイヤー</param>
    public void Seting(int r,PlayerController p,Action a)
    {
        m_ranking = r;
        m_player = p;
        m_rankingText.text = m_ranking + "位";
        m_nameText.text = m_player.Owner.Name;
        m_detail = a;
    }
    public void DetailButton()
    {
        m_detail();
        Debug.Log($"{m_player.Owner.Name}は{m_player.Money}円持っている");
    }
}
