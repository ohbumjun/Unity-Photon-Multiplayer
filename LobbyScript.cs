using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// RealTime 을 사용하기 위해 MonoBehaviourPunCallbacks 사용
public class LobbyScript : MonoBehaviourPunCallbacks
{
    // Create Definition of new typed lobby
    TypedLobby killCount = new TypedLobby("killCount", LobbyType.Default);
    TypedLobby teamBattle = new TypedLobby("teamBattle", LobbyType.Default);
    TypedLobby noRespawn = new TypedLobby("noRespawn", LobbyType.Default);

    // Canvas 에 현재 Script 가 있고, 해당 Text 에는 Room Number Text 객체를 세팅
    public Text roomNumber;

    public string levelName = "";

    // Canvas 에 현재 Lobby Script 가 붙어있음
    // 그리고 그 아래 Button Return to Menu Button onClick 에 현재 내용 세팅
    public void BackToMenu()
    {
        Debug.Log("Back to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    // KillCount 버튼을 클릭하면 호출되는 콜백함수
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
        // Lobby 에 접속하자마자  Random Room Join 시도
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // Random Room Join 실패 시 새로운 방 생성
        Debug.Log("No rooms available, creating a new room.");
        string roomName = "Room " + Random.Range(1000, 10000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 6; // 최대 플레이어 수 설정
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    public override void OnJoinedRoom()
    {
        // 현재 접속한 방의 이름을 UI 에 표시
        roomNumber.text = PhotonNetwork.CurrentRoom.Name;

        // 방에 접속하자마자 게임 씬으로 전환
        PhotonNetwork.LoadLevel(levelName);
    }
}
