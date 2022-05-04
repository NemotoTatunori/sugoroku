using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForkedRoadController : RoadController
{

    public override void RoadSetUp(RoadController road, string rn)
    {
        PrevRoadSet(road);
        //Ÿ‚Ìƒ}ƒX‚Éî•ñ‚ğ‘—‚é
        if (m_nextRoads == null)
        {
            return;
        }
        for (int i = 0; i < m_nextRoads.Length; i++)
        {
            NumberSet(rn, i);
            if (!m_nextRoads[i].PositionCorrection)
            {
                Vector3 now = m_nextConnect[i].position;
                Vector3 next = m_nextRoads[i].PrevConnect[0].position;
                Vector3 a = m_nextRoads[i].gameObject.transform.position;
                m_nextRoads[i].PositionSetUp(a + (now - next));
            }
            m_nextRoads[i].RoadSetUp(this, RoadNumber);
        }
    }

    new void NumberSet(string rn, int bn)
    {
        if (RoadNumber == "")
        {
            string[] sn = rn.Split(char.Parse("-"));
            int[] n = new int[sn.Length];
            for (int i = 0; i < n.Length; i++)
            {
                n[i] = int.Parse(sn[i]);
            }
            n[2] = bn;
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
            RoadNumber = an;
        }
    }
}
