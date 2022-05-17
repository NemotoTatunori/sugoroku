using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    /// <summary>�v���C���[�̖��O</summary>
    [SerializeField] string m_ownerName = null;
    /// <summary>�E��</summary>
    [SerializeField] int m_profession = 0;
    /// <summary>���������N</summary>
    [SerializeField] int m_salaryRank = 0;
    /// <summary>������</summary>
    [SerializeField] int m_money = 0;
    /// <summary>���݈ʒu</summary>
    [SerializeField] RoadController m_location = null;
    /// <summary>���Ȃ̈ʒu</summary>
    [SerializeField] Transform[] m_Seats = null;
    /// <summary>���Ȃɐl�����邩�̃t���O</summary>
    [SerializeField] bool[] m_sittings = null;
    /// <summary>����Ă���l����</summary>
    [SerializeField] Human[] m_family;
    /// <summary>����Ă���l��</summary>
    int m_familyNum = 0;
    /// <summary>�������̃t���O</summary>
    bool m_paydayFlag = false;
    /// <summary>���x�݂̃t���O</summary>
    [SerializeField] bool m_rest = false;
    /// <summary>�l�̃v���n�u</summary>
    [SerializeField] GameObject m_humanPrefab = null;
    /// <summary>��������</summary>
    [SerializeField] float m_moveSpeed = 30;
    /// <summary>���O��\������ꏊ</summary>
    [SerializeField] Image m_nameTextImage = null;
    /// <summary>���O��\������e�L�X�g</summary>
    [SerializeField] Text m_nameText = null;
    /// <summary>���򓹂̍s��</summary>
    int m_branchNumber = 0;
    /// <summary>�S�[���̃t���O</summary>
    bool m_goal;
    /// <summary>���O�̃v���p�e�B</summary>
    public string OwnerName
    {
        get => m_ownerName;
    }
    /// <summary>�������̃v���p�e�B</summary>
    public int Money
    {
        get => m_money;
    }
    /// <summary>���݈ʒu�̃v���p�e�B</summary>
    public RoadController Location
    {
        get => m_location;
        set
        {
            m_location = value;
        }
    }
    /// <summary>�E�Ƃ̃v���p�e�B</summary>
    public int Profession
    {
        get => m_profession;
        set
        {
            m_profession = value;
        }
    }
    /// <summary>���������N�̃v���p�e�B</summary>
    public int SalaryRank
    {
        get => m_salaryRank;
        set
        {
            m_salaryRank = value;
        }
    }
    /// <summary>�������̃t���O�̃v���p�e�B</summary>
    public bool PaydayFlag
    {
        get => m_paydayFlag;
        set
        {
            m_paydayFlag = value;
        }
    }
    /// <summary>���x�݂̃t���O�̃v���p�e�B</summary>
    public bool Rest
    {
        get => m_rest;
        set
        {
            m_rest = value;
        }
    }
    /// <summary>���򓹂̍s��̃t���O�̃v���p�e�B</summary>
    public int BranchNumber
    {
        get => m_branchNumber;
        set
        {
            m_branchNumber = value;
        }
    }
    /// <summary>�S�[���̃t���O�̃v���p�e�B</summary>
    public bool Goal
    {
        get => m_goal;
        set
        {
            m_goal = value;
        }
    }

    void Update()
    {
        m_nameTextImage.transform.rotation = Camera.main.transform.rotation;
    }
    /// <summary>
    /// ��������ϓ�������
    /// </summary>
    /// <param name="m">����</param>
    public void GetMoney(int m)
    {
        m_money += m;
    }
    /// <summary>
    /// �v���C���[�̏���ݒ肷��
    /// </summary>
    /// <param name="name">���O</param>
    /// <param name="location">�ŏ��̃}�X</param>
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
        m_nameText.text = m_ownerName;
        m_location = location;
    }
    /// <summary>
    /// �Ƒ��𑝂₷
    /// </summary>
    /// <param name="seibetu">����</param>
    /// <param name="name">���O</param>
    public void AddHuman(bool seibetu, string name)
    {
        m_familyNum++;
        GameObject h = Instantiate(m_humanPrefab, m_Seats[m_familyNum - 1]);
        m_family[m_familyNum - 1] = h.GetComponent<Human>();
        m_family[m_familyNum - 1].Seting(seibetu, name);
    }
    /// <summary>
    /// �ړ�����R���[�`����Ԃ�
    /// </summary>
    /// <param name="m">�i�ސ�</param>
    /// <param name="reverse">�i�ޕ���</param>
    /// <returns></returns>
    public Coroutine MoveStart(int m, bool reverse, bool e)
    {
        return StartCoroutine(Move(m, reverse, e));
    }
    /// <summary>
    /// �ړ�����R���[�`��
    /// </summary>
    /// <param name="m">�i�ސ�</param>
    /// <param name="reverse">�i�ޕ���</param>
    /// <returns></returns>
    IEnumerator Move(int m, bool reverse, bool e)
    {
        yield return null;
        for (int i = 0; i < m; i++)
        {
            Vector3 now = m_location.StopPint.position;
            RoadController nextPint;
            if (!reverse)
            {
                nextPint = m_location.NextRoad(m_branchNumber);
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
            if (m_location.NextRoad(m_branchNumber) == null)
            {
                reverse = true;
            }
            if (m_location.Event == RoadController.RoadEvents.Payday && !e)
            {
                PaydayFlag = true;
            }
            if (m_location.StopFlag && !e)
            {
                break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// ����������Ԃ�
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
    /// ���l�̍���Ԃ�
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
