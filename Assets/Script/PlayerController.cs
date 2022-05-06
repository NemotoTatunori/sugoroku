using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>プレイヤーの名前</summary>
    [SerializeField] string m_ownerName = null;
    /// <summary>職業</summary>
    [SerializeField] int m_profession = 0;
    /// <summary>給料ランク</summary>
    [SerializeField] int m_salaryRank = 0;
    /// <summary>所持金</summary>
    [SerializeField] int m_money = 0;
    /// <summary>現在位置</summary>
    [SerializeField] RoadController m_location = null;
    /// <summary>座席の位置</summary>
    [SerializeField] Transform[] m_Seats = null;
    /// <summary>座席に人がいるかのフラグ</summary>
    [SerializeField] bool[] m_sittings = null;
    /// <summary>乗っている人たち</summary>
    [SerializeField] Human[] m_family;
    /// <summary>乗っている人数</summary>
    int m_familyNum = 0;
    /// <summary>給料日のフラグ</summary>
    bool m_paydayFlag = false;
    /// <summary>一回休みのフラグ</summary>
    bool m_rest = false;
    /// <summary>人のプレハブ</summary>
    [SerializeField] GameObject m_humanPrefab = null;
    /// <summary>動く速さ</summary>
    [SerializeField] float m_moveSpeed = 30;
    /// <summary>現在位置のプロパティ</summary>
    public RoadController Location
    {
        get => m_location;
    }
    /// <summary>職業のプロパティ</summary>
    public int Profession
    {
        get => m_profession;
        set
        {
            m_profession = value;
        }
    }
    /// <summary>給料ランクのプロパティ</summary>
    public int SalaryRank
    {
        get => m_salaryRank;
        set
        {
            m_salaryRank = value;
        }
    }
    /// <summary>給料日のフラグのプロパティ</summary>
    public bool PaydayFlag
    {
        get => m_paydayFlag;
        set
        {
            m_paydayFlag = value;
        }
    }
    /// <summary>一回休みのフラグのプロパティ</summary>
    public bool Rest
    {
        get => m_rest;
        set
        {
            m_rest = value;
        }
    }
    /// <summary>
    /// 所持金を変動させる
    /// </summary>
    /// <param name="m">増減</param>
    public void GetMoney(int m)
    {
        m_money += m;
    }
    /// <summary>
    /// プレイヤーの情報を設定する
    /// </summary>
    /// <param name="name">名前</param>
    /// <param name="location">最初のマス</param>
    public void Seting(string name, RoadController location)
    {
        m_sittings = new bool[m_Seats.Length];
        m_family = new Human[m_Seats.Length];
        for (int i = 1; i < m_sittings.Length; i++)
        {
            m_sittings[i] = false;
        }
        m_sittings[0] = true;
        m_ownerName = name;
        m_location = location;
    }
    /// <summary>
    /// 家族を増やす
    /// </summary>
    /// <param name="seibetu">性別</param>
    /// <param name="name">名前</param>
    public void AddHuman(bool seibetu, string name)
    {
        m_familyNum++;
        GameObject h = Instantiate(m_humanPrefab, m_Seats[m_familyNum - 1]);
        m_family[m_familyNum - 1] = h.GetComponent<Human>();
        m_family[m_familyNum - 1].Seting(seibetu, name);
    }
    /// <summary>
    /// 移動するコルーチンを返す
    /// </summary>
    /// <param name="m">進む数</param>
    /// <param name="reverse">進む方向</param>
    /// <returns></returns>
    public Coroutine MoveStart(int m, bool reverse)
    {
        return StartCoroutine(Move(m, reverse));
    }
    /// <summary>
    /// 移動するコルーチン
    /// </summary>
    /// <param name="m">進む数</param>
    /// <param name="reverse">進む方向</param>
    /// <returns></returns>
    IEnumerator Move(int m, bool reverse)
    {
        yield return null;
        for (int i = 0; i < m; i++)
        {
            Vector3 now = m_location.StopPint.position;
            RoadController nextPint;
            if (!reverse)
            {
                nextPint = m_location.NextRoad();
            }
            else
            {
                nextPint = m_location.PrevRoad;
            }
            Vector3 next = nextPint.StopPint.position;
            float x = now.x;
            float y = now.y;
            float z = now.z;
            int f = 1;
            while (f > 0)
            {
                f = 0;
                if (NumberDifference(x, next.x) > 0.1f)
                {
                    x += Moving(x, next.x);
                    f++;
                }
                if (NumberDifference(y, next.y) > 0.1f)
                {
                    y += Moving(y, next.y);
                    f++;
                }
                if (NumberDifference(z, next.z) > 0.1f)
                {
                    z += Moving(z, next.z);
                    f++;
                }
                transform.position = new Vector3(x, y, z);
                yield return null;
            }
            transform.position = next;
            m_location = nextPint;
            if (m_location.NextRoad() == null)
            {
                reverse = true;
            }
            if (m_location.StopFlag)
            {
                break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 動く距離を返す
    /// </summary>
    /// <param name="n1"></param>
    /// <param name="n2"></param>
    /// <returns></returns>
    float Moving(float n1, float n2)
    {
        if (n1 < n2)
        {
            return Time.deltaTime * m_moveSpeed;
        }
        else
        {
            return Time.deltaTime * -1 * m_moveSpeed;
        }
    }
    /// <summary>
    /// 数値の差を返す
    /// </summary>
    /// <param name="n1"></param>
    /// <param name="n2"></param>
    /// <returns></returns>
    float NumberDifference(float n1, float n2)
    {
        float a;
        if (n1 > n2)
        {
            a = n1 - n2;
        }
        else
        {
            a = n2 - n1;
        }
        return a;
    }
}
