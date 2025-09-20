using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LookAt : MonoBehaviour
{
    // 2D Screen Space 상의 Mouse 위치를 World Space 로 변환하는 과정이 여기에 포함된다
    private Vector3 worldPosition;
    private Vector3 screenPosition;

    // Cross Hair 을 움직이게 하기 위함
    // cross hair 은 2d ui canvas 상의 2d sprite 이미지. 따라서 그냥 바로 mouse 2d input pos 세팅 가능
    public GameObject crosshair;
    // public Text nickNameText; // Canvas 안의 NickName Text

    private void Start()
    {
        // Cursor.visible = false; // 마우스 커서를 화면에서 숨기는 역할

        // nickNameText.text = PhotonNetwork.LocalPlayer.NickName; // NickName Text 세팅
    }

    // void FixedUpdate() : 강의에서는 Fixed Update 로 표시 
    void Update()
    {
        screenPosition = Input.mousePosition; // mouse : 2d pos

        // aim 위치의 z 가 player 앞쪽에 위치할 수 있도록 하기 위해 screen space 상의 z 를 앞쪽에 둔다
        // Screen Space 상에서 z 는 카메라로부터 앞쪽으로 얼마나 떨어져 있는가
        // z = 0 이면, 카메라 위치 자체, z > 0 이면 카메라 전방으로 나아간 3d 공간상 좌표
        screenPosition.z = 3f;

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition); 

        // 이를 통해 해당 script 를 장착한 object 는 screen 2d space 상의 우리 마우스 위치를 
        // 곧 world space 상의 위치로 세팅하게 된다. 
        // 현재 object 의 위치는 우리가 마우스로 가리키는 world 상의 좌표가 되는 것이다 
        transform.position = worldPosition;

        crosshair.transform.position = Input.mousePosition;
    }
}
