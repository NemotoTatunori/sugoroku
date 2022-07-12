using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBoxController : MonoBehaviour
{
    /// <summary>名前のテキスト</summary>
    [SerializeField] Text m_nameBox = null;
    /// <summary>お金のテキスト</summary>
    [SerializeField] Text m_moneyBox = null;
    /// <summary>職業のテキスト</summary>
    [SerializeField] Text m_professionBox = null;
    /// <summary>給料ランクのテキスト</summary>
    [SerializeField] Text m_salaryRankBox = null;
    /// <summary>
    /// プレイヤーステータスボックスを更新する
    /// </summary>
    public void PlayerStatusBoxUpdata(PlayerController p, WorkData professions)
    {
        m_nameBox.text = p.Owner.Name;
        m_moneyBox.text = p.Money.ToString();
        m_professionBox.text = professions.GetData(p.Profession).WorkName.ToString();
        m_salaryRankBox.text = p.SalaryRank.ToString();
    }
}
