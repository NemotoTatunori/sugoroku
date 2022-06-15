using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanelController : MonoBehaviour
{
    /// <summary>�����L���O�v���[�g�v���n�u</summary>
    [SerializeField] RankingPlatePrefab m_rankingPlatePrefab = null;
    /// <summary>3�ʈȍ~�̕\�����X�g</summary>
    [SerializeField] GameObject m_rankingList = null;
    /// <summary>�v���C���[�̔z��</summary>
    PlayerController[] m_players;
    /// <summary>�P�C�Q�C�R�ʂ̔z�u�z��</summary>
    [SerializeField] RectTransform[] m_point = null;
    /// <summary>�ڍ׃p�l��</summary>
    [SerializeField] DetailPanelController m_detailPanel = null;
    /// <summary>�P�C�Q�C�R�ʂ̐F�z��</summary>
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
