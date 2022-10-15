using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockMarketController : MonoBehaviour
{
    /// <summary>会社情報プレハブ</summary>
    [SerializeField] CompanyBox m_companyBoxPrefab = null;
    /// <summary>会社情報配列</summary>
    CompanyBox[] m_companies;
    /// <summary>プレハブの配置位置</summary>
    Vector3[] m_positions ={
        new Vector3(-450,0,0),
        new Vector3(-150, 0, 0) ,
        new Vector3(150, 0, 0) ,
        new Vector3(450, 0, 0)
    };

    public void Setting()
    {
        m_companies = new CompanyBox[m_positions.Length];
        for (int i = 0; i < m_positions.Length; i++)
        {
            CompanyBox company = Instantiate(m_companyBoxPrefab, this.gameObject.transform);
            company.transform.position = m_positions[i];
            m_companies[i] = company;
        }
    }

    void CompaniyDetail()
    {

    }
}
