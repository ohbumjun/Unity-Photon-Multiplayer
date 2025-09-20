using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public InputField playerNickname; // InputField ������Ʈ�� ������ ����
    private string setName = ""; // NameInput �Ʒ� Text �� �Էµ� ���� ������ ����
    public GameObject connecting; // Connecting Text �� �ش��ϴ� Object

    // Start is called before the first frame update
    void Start()
    {
        connecting.SetActive(false); // ó������ Connecting Text �� ����
    }

    // NameInput �� OnEndEdit �� ����� �Լ� : typing �� ��ĥ �� ���� ȣ��Ǵ� �Լ�
    public void UpdateText()
    {
        setName = playerNickname.text; 
        PhotonNetwork.LocalPlayer.NickName = setName; // Server �� �� �̸��� ������
    }

    public void EnterButton()
    {
        Debug.Log("Set Name : " + setName);

        // name �� ���� ������ ��쿡�� ����
        if (setName != "")
        {
            Debug.Log("Try Connect to Server");

            connecting.SetActive(true); // Connecting Text �� �Ҵ�

            PhotonNetwork.AutomaticallySyncScene = true; // ��� ����� ���� room �� join �ϰ� �ȴ�.

            // Photon ��ġ �� �� Setting ������ ��� ex) Assets/Photon/PhotoUnityNetworking/Resources
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    
    public void ExitButton()
    {
        Application.Quit();
    }

    public override void OnConnectedToMaster()
    {
        // Send Message to Server, ���� �ٽ� Receive �ϸ� �׶� ��μ� Console �� ���
        // ���� : ���� script �� ������ ��� ����� ��ǻ�Ϳ��� ����ȴ�
        Debug.Log("Connected to server");

        // Lobby �� �̹� Room �� ���� ���� ������ ����ִ�.
        // ������ Lobby �� ����, Room �� ���� �ϴ� �� ���� Mater �� ������ �Ǿ�� �Ѵ�
        // �̸� ���� ���� �Լ��� Lobby Scene ���� �̵��ϴ� �ڵ带 �ִ´�
        SceneManager.LoadScene("Lobby");
    }

}
