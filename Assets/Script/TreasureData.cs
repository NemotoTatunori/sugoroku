using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TreasureNames
{
    �X�J���x,
    �_�C�A�����h,
    �|����,
    ����,
    ����,
    ���㌕,
    ���@��,
    ����������,
    �o�b�N�N���[�W���[,
}

[CreateAssetMenu(fileName = "TreasureData")]
public class TreasureData : ScriptableObject
{
    [SerializeField] public List<Treasure> m_treasures = new List<Treasure>();
    public Treasure GetData(int i) => m_treasures[i];
}

[System.Serializable]
public class Treasure
{
    [SerializeField] TreasureNames m_treasureName;
    [SerializeField] int m_price;
    public TreasureNames TreasureName => m_treasureName;
    public int Price => m_price;
}
