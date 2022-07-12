using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusBoxController : MonoBehaviour
{
    /// <summary>���O�̃e�L�X�g</summary>
    [SerializeField] Text m_nameBox = null;
    /// <summary>�����̃e�L�X�g</summary>
    [SerializeField] Text m_moneyBox = null;
    /// <summary>�E�Ƃ̃e�L�X�g</summary>
    [SerializeField] Text m_professionBox = null;
    /// <summary>���������N�̃e�L�X�g</summary>
    [SerializeField] Text m_salaryRankBox = null;
    /// <summary>
    /// �v���C���[�X�e�[�^�X�{�b�N�X���X�V����
    /// </summary>
    public void PlayerStatusBoxUpdata(PlayerController p, WorkData professions)
    {
        m_nameBox.text = p.Owner.Name;
        m_moneyBox.text = p.Money.ToString();
        m_professionBox.text = professions.GetData(p.Profession).WorkName.ToString();
        m_salaryRankBox.text = p.SalaryRank.ToString();
    }
}
