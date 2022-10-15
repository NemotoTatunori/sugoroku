using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CompanyBox : MonoBehaviour
{
    [SerializeField] Text m_name = null;
    [SerializeField] Text m_prise = null;
    [SerializeField] Button m_button = null;
    public void Setting(string name, int prise, Action action)
    {
        m_name.text = name;
        m_prise.text = prise.ToString();
        m_button.onClick.AddListener(() => action());
    }
}
