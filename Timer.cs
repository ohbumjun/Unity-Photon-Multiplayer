using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text minutesText;
    public Text secondsText;
    public int minutes = 4;
    public int seconds = 59;

    public void BeginTimer()
    {
        GetComponent<PhotonView>().RPC(
             "Count",
             RpcTarget.AllBuffered); // ���߿� �����ϴ� ����鵵 �� RPC �� �ް� �ȴ�
    }

    [PunRPC]
    void Count()
    {
        // repeat every second
        BeginCounting();
    }

    void BeginCounting()
    {
        CancelInvoke(); // Ȥ�� ������ ����ǰ� �ִ� �� �ִٸ� ���
        InvokeRepeating("TimeCountDown", 1.0f, 1.0f); //1�ʸ��� TimeCountDown �Լ� ����
    }

    private void TimeCountDown()
    {
        if (seconds > 10) // 11 �� �̻��̸� 1�� ���� ��Ų �� �״�� ǥ��
        {
            seconds--;
            secondsText.text = seconds.ToString();
        }
        else if (seconds > 0 && seconds < 11) // 10��  ���ϸ� 0 �ٿ��� ǥ��
        {
            seconds--;
            secondsText.text = "0" + seconds.ToString();
        }
        else if (seconds == 0 && minutes > 0) // �ʰ� 0 �̰� ���� �����ִٸ� 1�� ���ҽ�Ű�� 59�ʷ� �ʱ�ȭ
        {
            secondsText.text = "0" + minutes.ToString();
            
            minutes--;
            minutesText.text = minutes.ToString(); // 59 ���� ����, 0 �տ� �Ⱥ��̰� 59�� ǥ��

            seconds = 59; // �ٽ� second �� 59 �� ��ü
            secondsText.text = seconds.ToString(); // 59 ���� ����, 0 �տ� �Ⱥ��̰� 59�� ǥ��
        }
    }
}
