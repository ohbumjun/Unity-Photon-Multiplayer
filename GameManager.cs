using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public InputField playerNickname; // InputField 컴포넌트를 연결할 변수
    private string setName = ""; // NameInput 아래 Text 에 입력된 값을 저장할 변수
    public GameObject connecting; // Connecting Text 에 해당하는 Object

    // Start is called before the first frame update
    void Start()
    {
        connecting.SetActive(false); // 처음에는 Connecting Text 를 끈다
    }

    // NameInput 의 OnEndEdit 에 연결된 함수 : typing 을 마칠 때 마다 호출되는 함수
    public void UpdateText()
    {
        setName = playerNickname.text; 
        PhotonNetwork.LocalPlayer.NickName = setName; // Server 에 내 이름을 보낸다
    }

    public void EnterButton()
    {
        Debug.Log("Set Name : " + setName);

        // name 을 실제 세팅한 경우에만 진행
        if (setName != "")
        {
            Debug.Log("Try Connect to Server");

            connecting.SetActive(true); // Connecting Text 를 켠다

            PhotonNetwork.AutomaticallySyncScene = true; // 모든 사람이 같은 room 에 join 하게 된다.

            // Photon 설치 후 뜬 Setting 파일을 사용 ex) Assets/Photon/PhotoUnityNetworking/Resources
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public void ExitButton()
    {
        Application.Quit();
    }

    public override void OnConnectedToMaster()
    {
        // Send Message to Server, 이후 다시 Receive 하면 그때 비로소 Console 에 뜬다
        // 참고 : 현재 script 는 접속한 모든 사람의 컴퓨터에서 실행된다
        Debug.Log("Connected to server");

        // Lobby 에 이미 Room 에 들어가는 등의 로직이 들어있다.
        // 하지만 Lobby 에 들어가건, Room 에 들어가건 일단 그 전에 Mater 랑 연결이 되어야 한다
        // 이를 위해 현재 함수에 Lobby Scene 으로 이동하는 코드를 넣는다
        SceneManager.LoadScene("Lobby");
    }

}
