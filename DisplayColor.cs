using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

// 해당 스크립트는 ybot 에 부착된다 
// 각 캐릭터가 어떤 버튼을 클릭했는지에 따라 자신의 색상을 바꾼다
// 참고 : 이를 prefab ybot 에 부착한 이후, 해당 ybot 을 참조하는 링크가 깨질 수 있으니 확인
public class DisplayColor : MonoBehaviour
{
    public int[] buttonNumbers; // 0 ~ 5 (6개 버튼), 각 캐릭터가 어떤 버튼을 클릭했는가
    public int[] viewID; // 각 캐릭터의 Photon View ID. 이를 통해 어떤 캐릭터가 어떤 버튼을 클릭했는지 알 수 있다
                         // ex) viewID[0] = 1001 란, 0번째 button  (빨간색)을 클릭한 캐릭터의 Photon View ID 가 1001 이란 뜻
    public Color32[] colors; // 실제 Editor 상에서 설정하는 색상들

    private GameObject namesObject;

    public void Start()
    {
    }

    // 특정 캐릭터가 색상 버튼을 클릭하면
    // 모든 player 에 대해 아래 함수가 실행된다.
    // 이를 통해 모든 클라이언트에서 AssignColor 가 실행된다.
    public void ChooseColor()
    {
        GetComponent<PhotonView>().RPC(
             "AssignColor",
             RpcTarget.AllBuffered); // 나중에 접속하는 사람들도 이 RPC 를 받게 된다
    }
    [PunRPC]
    void AssignColor()
    {
        if (namesObject == null)
        {
            namesObject = GameObject.Find("NamesBG");
        }

        // ex) Player 1 ~ 6 가 있을 때. 6개의 컴퓨터에서 Player 1 에 대한 아래의 함수가 호출되는 것
        PhotonView pv = GetComponent<PhotonView>();

        for (int buttonNumber = 0; buttonNumber < viewID.Length; buttonNumber++)
        {
            // 각 Player 오브젝트에서 자신의 PhotonView의 ViewID가
            // viewID[i]와 같으면, 해당 Player의 색상을 colors[i]로 변경합니다.
            // 예시: 빨간색(0번) 버튼을 누른 Player의 ViewID가 1001이면,
            // 모든 Player 오브젝트에서 viewID[0] == pv.ViewID인 Player의 색상이 빨간색으로 바뀝니다.

            if (pv.ViewID == viewID[buttonNumber])
            {
                // 해당 캐릭터의 색상을 colors[i] 로 바꾼다
                // Ybot 상에서 child 중에 Alpha_Surface GameObject 가 있다
                // 그것의 Material 의 색상을 변경할 것이다 
                // 이를 위해서 Material 이 set 되어 있는 Renderer Component 를 가져온다
                this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[buttonNumber];

                // 그리고 nick name 이 띄어지는 Floor Layout 왼쪽 상단쪽에 health bar 색상 등도 변경한다.
                if (namesObject == null)
                {
                    Debug.Log("No namesObject");
                }

                NicknameScript nickNameScript = namesObject.GetComponent<NicknameScript>();

                if (nickNameScript == null)
                {
                    Debug.Log("No nickname");
                }

                // nickname, health bar 활성화
                nickNameScript.nickNames[buttonNumber].gameObject.SetActive(true);
                nickNameScript.healthBars[buttonNumber].gameObject.SetActive(true);

                nickNameScript.nickNames[buttonNumber].text = pv.Owner.NickName;
            }
        }
    }

    // 아래와 같이 한번 더 AssignColor 를 RPC 가 아니게 호출하면, 2번째 Player 부터 색상이 안바뀐다.
    // public void ChooseColor(int buttonNumber, int myID)
    // {
    //     Debug.Log("Choose Color Call");
    // 
    //     AssignColor(buttonNumber, myID); // 그냥 직접 호출
    // }
    // void AssignColor(int buttonNumber, int myID)
    // {
    //     PhotonView pv = GetComponent<PhotonView>();
    //     if (pv.ViewID == myID)
    //     {
    //         this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[buttonNumber];
    //     }
    // }


}
