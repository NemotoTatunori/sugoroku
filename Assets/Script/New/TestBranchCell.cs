using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBranchCell : BranchCellBase
{
    protected override int BranchEvent(int branchCount)
    {
        int random = Random.Range(0, branchCount);
        Debug.Log($"Name{name} BranchCell");
        Debug.Log($"Return {random}");
        return random;
    }
}
