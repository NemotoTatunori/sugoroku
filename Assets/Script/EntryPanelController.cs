using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryPanelController : MonoBehaviour
{
    Coroutine m_coroutine;
    /// <summary>�ő�l��</summary>
    [SerializeField] private int m_maxEntry = 30;
    /// <summary>�ŏ��̃}�X</summary>
    [SerializeField] RoadController m_first = null;
    /// <summary>�Ԃ̃v���n�u</summary>
    [SerializeField] GameObject m_carPrefab = null;
    /// <summary>�Q������l�̖��O����͂���ꏊ</summary>
    [SerializeField] Text m_entryName = null;
    /// <summary>�Q������l�̖��O��\������v���n�u</summary>
    [SerializeField] GameObject m_entryNamePrehab = null;
    /// <summary>�Q������l�̖��O��\������ꏊ</summary>
    [SerializeField] GameObject m_entryNameDisplay = null;
    /// <summary>�Q�����̃e�L�X�g</summary>
    [SerializeField] Text m_peopleNumText = null;
    /// <summary>�G���g���[��</summary>
    int m_peopleNum = 0;
    /// <summary>�x���e�L�X�g</summary>
    [SerializeField] GameObject m_caveatText = null;
    /// <summary>�j�̐F�i���F�j</summary>
    Color m_manColor = new Color(0.6f, 1, 1);
    /// <summary>���̐F�i�s���N�j</summary>
    Color m_womanColor = new Color(1, 0.6f, 1);

    GameManager m_gameManager;
    /// <summary>
    /// �Q�[�����X�^�[�g������
    /// </summary>
    public void GameStart()
    {
        if (m_peopleNum == 0)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
            m_coroutine = StartCoroutine(Caveat("�P�l�����Ȃ���I"));
            return;
        }
        PlayerController[] players = new PlayerController[m_peopleNum];
        float px = (players.Length - 1) * -2.5f;
        if (players.Length >= 11) { px = 9 * -2.5f; }
        int x = 0;
        int z = 0;
        for (int i = 0; i < players.Length; i++)
        {
            GameObject car = Instantiate(m_carPrefab);
            car.transform.position = new Vector3(px + x * 5, 0, z * -10);
            players[i] = car.GetComponent<PlayerController>();
            EntryNamePrefab en = m_entryNameDisplay.transform.GetChild(i).GetComponent<EntryNamePrefab>();
            players[i].Seting(en.Seibetu, en.Name, m_first, m_gameManager);
            x++;
            if (x >= 10)
            {
                x = 0;
                z++;
            }
        }
        m_gameManager.GameStart(players);
    }
    /// <summary>
    /// �G���g���[������
    /// </summary>
    /// <param name="seibetu">����</param>
    public void AddName(bool seibetu)
    {
        if (m_peopleNum >= m_maxEntry)
        {
            if (m_coroutine != null)
            {
                StopCoroutine(m_coroutine);
            }
            m_coroutine = StartCoroutine(Caveat("�ő�" + m_maxEntry + "�l�܂ŁI"));
            return;
        }
        m_peopleNum++;
        GameObject player = Instantiate(m_entryNamePrehab);
        EntryNamePrefab en = player.GetComponent<EntryNamePrefab>();
        en.Name = m_entryName.text;
        en.Seibetu = seibetu;
        player.transform.SetParent(m_entryNameDisplay.transform);
        player.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        m_entryNameDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2(0, m_peopleNum * 50);
        m_peopleNumText.text = m_peopleNum + "�l";
        if (seibetu)
        {
            player.GetComponent<Image>().color = m_manColor;
        }
        else
        {
            player.GetComponent<Image>().color = m_womanColor;
        }
    }
    /// <summary>
    /// �G���g���[��������
    /// </summary>
    /// <param name="name">�l�[���v���n�u</param>
    public void RemoveName(GameObject name)
    {
        m_peopleNum--;
        Destroy(name);
        m_peopleNumText.text = m_peopleNum + "�l";
    }
    /// <summary>
    /// �x���e�L�X�g�\��
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    IEnumerator Caveat(string text)
    {
        m_caveatText.SetActive(true);
        Text t = m_caveatText.transform.GetChild(0).GetComponent<Text>();
        t.text = text;
        for (float i = 0; i < 2; i += Time.deltaTime)
        {
            yield return null;
        }
        t.text = "";
        m_caveatText.SetActive(false);
    }
}
