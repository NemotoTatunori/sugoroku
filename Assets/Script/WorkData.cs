using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WorkName
{
    �t���[�^�[,
    �v���O���}�[,
    ��H,
    �t�O,
    ���΂��|�l�̃T�|�[�^�[,
    Google�̎Ј�,
    �L���b�v�E�l,
    �Ȃ񂩂߂�����Љ�v�����Ă�l,
    ���t,
    �G�t,
    �p�e�B�V�G,
    �A�j���Ƃ��Ń{�R�{�R�ɂ����l,
    �A���v�X�̏���,
    �߂�����_�j����l,
    �Љ�s�K����,
    �����l,
    �o�b�N�N���[�W���[�E�l_��,
    �o�b�N�N���[�W���[�E�l_��,
    �o�b�N�N���[�W���[�E�l_��,
    �o�b�N�N���[�W���[�E�l_�S�b�h
}

[CreateAssetMenu(fileName = "WorkData")]
public class WorkData : ScriptableObject
{
    [SerializeField] public List<Work> m_works = new List<Work>();
    public Work GetData(int id) => m_works[id];

}

[System.Serializable]
public class Work
{
    [SerializeField] WorkName m_workName;
    [SerializeField] int m_salary;
    [SerializeField] float m_magnification;
    public WorkName WorkName
    {
        get
        {
            return m_workName;
        }
    }
    public int Salary => m_salary;
    public float Magnification => m_magnification;
}