using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelController : MonoBehaviour
{
    /// <summary>ランキングプレートプレハブ</summary>
    [SerializeField] RankingPlatePrefab m_rankingPlatePrefab = null;
    /// <summary>3位以降の表示リスト</summary>
    [SerializeField] GameObject m_rankingList = null;
    /// <summary>プレイヤーの配列</summary>
    PlayerController[] m_players;
    /// <summary>１，２，３位の配置配列</summary>
    [SerializeField] RectTransform[] m_point = null;
    /// <summary>詳細パネル</summary>
    [SerializeField] DetailPanelController m_detailPanel = null;
    /// <summary>１，２，３位の色配列</summary>
    Color[] m_rankingColoer = {
        new Color(1f, 0.84f, 0),
        new Color(0.862f, 0.866f, 0.866f),
        new Color(0.768f, 0.439f, 0.133f) };
    public void GetPlayers(PlayerController[] ps)
    {
        gameObject.SetActive(true);
        m_players = ps;
        RankingStart();
    }
    void RankingStart()
    {
        int loop = m_players.Length > 3 ? 3 : m_players.Length;
        for (int i = 0; i < loop; i++)
        {
            RankingPlatePrefab p = Instantiate(m_rankingPlatePrefab, transform);
            p.GetComponent<Image>().color = m_rankingColoer[i];
            p.GetComponent<RectTransform>().position = m_point[i].position;
            p.GetComponent<RectTransform>().localScale = new Vector2(2f, 2f);
            //p.Seting(i + 1, m_players[i],);
        }
        for (int i = 3; i < m_players.Length; i++)
        {
            RankingPlatePrefab p = Instantiate(m_rankingPlatePrefab, m_rankingList.transform);
            //p.Seting(i + 1, m_players[i],);
        }
    }
}
