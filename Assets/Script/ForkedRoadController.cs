using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ForkedRoadController : RoadController
{
    [SerializeField] public bool m_sentaku;
    public override void RoadSetUp(RoadController road, string rn, Action<RoadController> getRoad)
    {
        m_getRoad = getRoad;
        if (RoadNumber == "")
        {
            RoadNumber = rn;
        }
        PrevRoadSet(road);
        //次のマスに情報を送る
        if (m_nextRoads == null)
        {
            return;
        }
        for (int i = 0; i < m_nextRoads.Length; i++)
        {
            if (!m_nextRoads[i].PositionCorrection)
            {
                Vector3 now = m_nextConnect[i].position;
                Vector3 next = m_nextRoads[i].PrevConnect[0].position;
                Vector3 a = m_nextRoads[i].gameObject.transform.position;
                m_nextRoads[i].PositionSetUp(a + (now - next));
            }
            m_nextRoads[i].RoadSetUp(this, NextNumber(rn, i), getRoad);
        }
    }

    protected override string NextNumber(string rn, int bn)
    {
        string[] sn = rn.Split(char.Parse("-"));
        int[] n = new int[sn.Length];
        for (int i = 0; i < n.Length; i++)
        {
            n[i] = int.Parse(sn[i]);
        }
        n[1] = bn;
        n[2]++;
        string an = "";
        for (int i = 0; i < n.Length; i++)
        {
            an += n[i].ToString();
            if (i < n.Length - 1)
            {
                an += char.Parse("-");
            }
        }
        return an;
    }

    public override RoadController NextRoad(int branch)
    {
        return m_nextRoads[branch];
    }
}
