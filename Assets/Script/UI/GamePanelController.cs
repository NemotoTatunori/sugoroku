using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelController : MonoBehaviour
{
    /// <summary>�v���C���[�X�e�[�^�X�{�b�N�X</summary>
    [SerializeField] PlayerStatusBoxController m_playerStatusBox = null;
    /// <summary>�e�L�X�g��\������</summary>
    [SerializeField] Text m_progressText = null;
    /// <summary>���[���b�g</summary>
    [SerializeField] RouletteController m_roulette = null;
    /// <summary>�l�ǉ��p�l��</summary>
    [SerializeField] AddHumanPanelController m_addHumanPanel = null;
    /// <summary>�A�E�p�l���̃e�L�X�g</summary>
    [SerializeField] Text m_findWorkPanel = null;
    /// <summary>�^�[���G���h�{�^��</summary>
    [SerializeField] GameObject m_turnEndButton = null;
    GameManager m_gameManager;
    /// <summary>�v���C���[�X�e�[�^�X�{�b�N�X�̃v���p�e�B</summary>
    public PlayerStatusBoxController PlayerStatusBox => m_playerStatusBox;
    /// <summary>���[���b�g�̃v���p�e�B</summary>
    public RouletteController Roulette => m_roulette;
    /// <summary>�l�ǉ��p�l���̃v���p�e�B</summary>
    public AddHumanPanelController AddHumanPanel => m_addHumanPanel;
    /// <summary>�\������e�L�X�g�̃v���p�e�B</summary>
    public GameObject ProgressText => m_progressText.transform.parent.gameObject;
    /// <summary>�^�[���G���h�{�^���̃v���p�e�B</summary>
    public GameObject TurnEndButton => m_turnEndButton;

    public void GetGameManager(GameManager gm)
    {
        m_gameManager = gm;
        m_roulette.GetGameManager(gm);
        m_addHumanPanel.GetGameManager(gm);
    }
    /// <summary>
    /// �\������e�L�X�g���X�V���ĕ\������
    /// </summary>
    /// <param name="t"></param>
    public void TextDisplay(string t)
    {
        ProgressText.SetActive(true);
        m_progressText.text = t;
    }
    /// <summary>
    /// �A�E�p�l���ɕ����𔽉f������
    /// </summary>
    /// <param name="t">�E��</param>
    public void FindWorkText(string t)
    {
        m_findWorkPanel.transform.parent.gameObject.SetActive(true);
        m_findWorkPanel.text = t + "�ɂȂ�܂����H\n\n���Ȃ�ƕ���I���܂Ői��";
    }
    /// <summary>
    /// �A�E�p�l���̓������󂯎��
    /// </summary>
    /// <param name="answer">����</param>
    public void FindWorkMove(bool answer)
    {
        m_findWorkPanel.transform.parent.gameObject.SetActive(false);
        m_gameManager.FindWorkMove(answer);
    }
    public void Next()
    {
        m_gameManager.Progress();
    }
}