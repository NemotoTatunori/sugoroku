using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TreasureNames
{
    スカラベ,
    ダイアモンド,
    掛け軸,
    王冠,
    金塊,
    草薙剣,
    八咫鏡,
    八尺瓊勾玉,
    バッククロージャー,
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
