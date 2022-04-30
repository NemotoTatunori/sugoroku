using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryNamePrefab : MonoBehaviour
{
    public void Destroy()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        gm.RemoveName(this.gameObject);
    }
}
