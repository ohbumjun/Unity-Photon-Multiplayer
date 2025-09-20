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
             RpcTarget.AllBuffered); // 나중에 접속하는 사람들도 이 RPC 를 받게 된다
    }

    [PunRPC]
    void Count()
    {
        // repeat every second
        BeginCounting();
    }

    void BeginCounting()
    {
        CancelInvoke(); // 혹시 이전에 실행되고 있던 게 있다면 취소
        InvokeRepeating("TimeCountDown", 1.0f, 1.0f); //1초마다 TimeCountDown 함수 실행
    }

    private void TimeCountDown()
    {
        if (seconds > 10) // 11 초 이상이면 1초 감소 시킨 값 그대로 표시
        {
            seconds--;
            secondsText.text = seconds.ToString();
        }
        else if (seconds > 0 && seconds < 11) // 10초  이하면 0 붙여서 표시
        {
            seconds--;
            secondsText.text = "0" + seconds.ToString();
        }
        else if (seconds == 0 && minutes > 0) // 초가 0 이고 분이 남아있다면 1분 감소시키고 59초로 초기화
        {
            secondsText.text = "0" + minutes.ToString();
            
            minutes--;
            minutesText.text = minutes.ToString(); // 59 변경 이후, 0 앞에 안붙이고 59만 표시

            seconds = 59; // 다시 second 를 59 로 대체
            secondsText.text = seconds.ToString(); // 59 변경 이후, 0 앞에 안붙이고 59만 표시
        }
    }
}
