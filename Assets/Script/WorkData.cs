using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WorkNames
{
    フリーター,
    プログラマー,
    大工,
    フグ,
    お笑い芸人のサポーター,
    Googleの社員,
    キャップ職人,
    なんかめっちゃ社会貢献してる人,
    漁師,
    絵師,
    パティシエ,
    アニメとかでボコボコにされる人,
    アルプスの少女,
    めっちゃ論破する人,
    社会不適合者,
    料理人,
    バッククロージャー職人_極,
    バッククロージャー職人_剣,
    バッククロージャー職人_獄,
    バッククロージャー職人_ゴッド
}

[CreateAssetMenu(fileName = "WorkData")]
public class WorkData : ScriptableObject
{
    [SerializeField] List<Work> m_works = new List<Work>();
    public Work GetData(int id) => m_works[id];
    public int WorkNum => m_works.Count;
}

[System.Serializable]
public class Work
{
    [SerializeField] WorkNames m_workName;
    [SerializeField] int m_salary;
    [SerializeField] float m_magnification;
    public WorkNames WorkName
    {
        get
        {
            return m_workName;
        }
    }
    public int Salary => m_salary;
    public float Magnification => m_magnification;
}
