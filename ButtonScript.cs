using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Floor Layout Scene 의 BG 아래 여러 Btn 들이 있다
// 해당 여러 색상에 붙어있는 Script 들
// 당연히 해당 Button 들은 Photon View Component 를 가지고 있어야 한다
public class ButtonScript : MonoBehaviour
{
    public GameObject[] players;
    private int myID;
    private GameObject panel;
    private GameObject namesObject;

    public void Start()
    {
        Cursor.visible = true; // 마우스 커서를 화면에서 보이게 한다. 화면에서 버튼을 클릭하기 위함

        panel = GameObject.Find("ChoosePanel"); // 해당 button 들이 포함된 상위 Panel Object

        namesObject = GameObject.Find("NamesBG");
    }


    /*
    전체 흐름 예시

    플레이어 A가 빨간색 버튼(0번)을 클릭:

    	ButtonScript의 SelectButton(0) 호출 → 내 Player의 ViewID(1001) 획득
        for : "SelectedColor" RPC 호출 → 모든 클라이언트에서 실행
            모든 Player의 DisplayColor의 viewID[0] = 1001로 설정
            for 모든 Player의 DisplayColor의 ChooseColor() 호출
                for 각 Player의 DisplayColor에서 "AssignColor" RPC 호출 → 모든 클라이언트에서 실행
                    모든 Player 오브젝트에서 viewID[0] == pv.ViewID인 Player의 색상을 빨간색으로 변경
    ---
    요약
    •	버튼 클릭 → 내 Player의 ViewID 획득 → 모든 클라이언트에 RPC로 선택 정보 전달
    •	모든 Player의 DisplayColor에 선택 정보 저장 → 모든 Player의 색상 변경 함수 호출
    •	각 Player 오브젝트에서 자신의 ViewID와 선택된 ViewID가 일치하면 색상 변경
    */

    // 자. 해당 함수는 Button onClick 에 등록되어 있다. 인자 buttonNumber 는
    // Editor 상에서 직접 세팅해줘야 한다. ex) 6개라면 0 ~ 5
    // 현재 Scene에 있는 모든 Player 오브젝트를 찾고,
    // 내 Player(내가 조종하는 Player)의 PhotonView의 ViewID를 myID에 저장합니다.
    // 버튼을 누른 Button 오브젝트의 PhotonView를 통해
    // "SelectedColor" RPC를** 모든 클라이언트(AllBuffered)**에 호출합니다.
    public void SelectButton(int buttonNumber)
    {
        // 현재 Scene 에 있는 모든 Player 들을 가져온다
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            // 각 Player 들의 Photon View Component 를 가져온다
            PhotonView pv = players[i].GetComponent<PhotonView>();

            if (pv.IsMine == false)
            {
                continue;
            }

            // 해당 Player 가 나의 Player 라면
            myID = players[i].GetComponent<PhotonView>().ViewID;

            Debug.Log("My ID : " + myID);
            break; // 일단 찾으면 끝
        }

        GetComponent<PhotonView>().RPC(
            "SelectedColor", 
            RpcTarget.AllBuffered, // 나중에 접속하는 사람들도 이 RPC 를 받게 된다
            buttonNumber, 
            myID);

        // 버튼을 누르고 나면  panel 을 비활성화 시킨다. 
        panel.SetActive(false);

        // 커서도 다시 안보이게 한다
        Cursor.visible = false;
    }


    // 특정 player 가 색상을 고르면 아래 함수가 모든 클라이언트에서 실행된다
    // 모든 player 의 DisplayColor Script 의 viewID 배열에
    // 누가 어떤 버튼을 눌렀는지 정보를 넣어준다
    // 예시: 빨간색(0번) 버튼을 누른 Player의 ViewID가 1001이면,
    // 모든 Player의 DisplayColor의 viewID[0] = 1001이 됩니다.
    // 그리고 DisplayColor Script 의 ChooseColor() 함수를 호출합니다
    [PunRPC]
    void SelectedColor(int buttonNumber, int myID)
    {
        // Debug.Log($"Button Number : {buttonNumber}, ViewID : {viewID}");
        // 현재 Scene 에 있는 모든 Player 들을 가져온다
        players = GameObject.FindGameObjectsWithTag("Player");

        Debug.Log("PlayerNumber : " + players.Length);

        for (int i = 0; i < players.Length; i++)
        {
            var displayColor = players[i].GetComponent<DisplayColor>();

            if (displayColor == null)
            {
                Debug.LogError($"Player {players[i].name}에 DisplayColor 컴포넌트가 없습니다.");
                continue;
            }
            if (displayColor.viewID == null || buttonNumber >= displayColor.viewID.Length)
            {
                Debug.LogError($"Player {players[i].name}의 viewID 배열이 null이거나 크기가 부족합니다.");
                continue;
            }

            // 각 Player 들 정보를 가져온다
            // 그리고 각 Player 의 DisplayColor Script 에 누가 어떤 버튼을 눌렀는지 정보를 넣어준다
            // 예를 들어, player 1 이 red 를 선택하면, 모든 다른 컴퓨터의 player 1 의 DisplayColor Script 에 이 정보가 세팅되는 것이다.
            displayColor.viewID[buttonNumber] = myID;

            // 그 다음 아래 함수를 호출하면, 이 또한 내부적으로 RPC 호출이 된다
            // 그래서 실제 모든 사람 컴퓨터에서 color 가 바뀌게 되는 것이다
            displayColor.ChooseColor();
        }

        namesObject.GetComponent<Timer>().BeginTimer(); // 색상 선택 이후, 타이머 시작

        this.transform.gameObject.SetActive(false); // 버튼 비활성화
    }
}
