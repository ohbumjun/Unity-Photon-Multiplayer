using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// RealTime �� ����ϱ� ���� MonoBehaviourPunCallbacks ���
public class LobbyScript : MonoBehaviourPunCallbacks
{
    // Create Definition of new typed lobby
    TypedLobby killCount = new TypedLobby("killCount", LobbyType.Default);
    TypedLobby teamBattle = new TypedLobby("teamBattle", LobbyType.Default);
    TypedLobby noRespawn = new TypedLobby("noRespawn", LobbyType.Default);

    // Canvas �� ���� Script �� �ְ�, �ش� Text ���� Room Number Text ��ü�� ����
    public Text roomNumber;

    public string levelName = "";

    // Canvas �� ���� Lobby Script �� �پ�����
    // �׸��� �� �Ʒ� Button Return to Menu Button onClick �� ���� ���� ����
    public void BackToMenu()
    {
        Debug.Log("Back to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    // KillCount ��ư�� Ŭ���ϸ� ȣ��Ǵ� �ݹ��Լ�
    public void JoinGameKillCount()
    {
        levelName = "Floor Layout";
        PhotonNetwork.JoinLobby(killCount); 
    }
    public void JoinGameTeamBattle()
    {
        levelName = "Floor Layout";
        PhotonNetwork.JoinLobby(teamBattle);
    }
    public void JoinGameNoRespawn()
    {
        levelName = "Floor Layout";
        PhotonNetwork.JoinLobby(noRespawn);
    }

    public override void OnJoinedLobby()
    {
        // Lobby �� �������ڸ���  Random Room Join �õ�
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // Random Room Join ���� �� ���ο� �� ����
        Debug.Log("No rooms available, creating a new room.");
        string roomName = "Room " + Random.Range(1000, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6; // �ִ� �÷��̾� �� ����
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        // ���� ������ ���� �̸��� UI �� ǥ��
        roomNumber.text = PhotonNetwork.CurrentRoom.Name;

        // �濡 �������ڸ��� ���� ������ ��ȯ
        PhotonNetwork.LoadLevel(levelName);
    }
}
