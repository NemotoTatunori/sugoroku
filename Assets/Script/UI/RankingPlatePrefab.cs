using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// <summary>
    /// 情報を受け取って設定する
    /// </summary>
    /// <param name="r">順位</param>
    /// <param name="p">プレイヤー</param>
    public void Seting(int r,PlayerController p)
    {
        m_ranking = r;
        m_player = p;
        m_rankingText.text = m_ranking + "位";
        m_nameText.text = m_player.Owner.Name;
    }
}
