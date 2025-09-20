using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LookAt : MonoBehaviour
{
    // 2D Screen Space ���� Mouse ��ġ�� World Space �� ��ȯ�ϴ� ������ ���⿡ ���Եȴ�
    private Vector3 worldPosition;
    private Vector3 screenPosition;

    // Cross Hair �� �����̰� �ϱ� ����
    // cross hair �� 2d ui canvas ���� 2d sprite �̹���. ���� �׳� �ٷ� mouse 2d input pos ���� ����
    public GameObject crosshair;
    // public Text nickNameText; // Canvas ���� NickName Text

    private void Start()
    {
        // Cursor.visible = false; // ���콺 Ŀ���� ȭ�鿡�� ����� ����

        // nickNameText.text = PhotonNetwork.LocalPlayer.NickName; // NickName Text ����
    }

    // void FixedUpdate() : ���ǿ����� Fixed Update �� ǥ�� 
    void Update()
    {
        screenPosition = Input.mousePosition; // mouse : 2d pos

        // aim ��ġ�� z �� player ���ʿ� ��ġ�� �� �ֵ��� �ϱ� ���� screen space ���� z �� ���ʿ� �д�
        // Screen Space �󿡼� z �� ī�޶�κ��� �������� �󸶳� ������ �ִ°�
        // z = 0 �̸�, ī�޶� ��ġ ��ü, z > 0 �̸� ī�޶� �������� ���ư� 3d ������ ��ǥ
        screenPosition.z = 3f;

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition); 

        // �̸� ���� �ش� script �� ������ object �� screen 2d space ���� �츮 ���콺 ��ġ�� 
        // �� world space ���� ��ġ�� �����ϰ� �ȴ�. 
        // ���� object �� ��ġ�� �츮�� ���콺�� ����Ű�� world ���� ��ǥ�� �Ǵ� ���̴� 
        transform.position = worldPosition;

        crosshair.transform.position = Input.mousePosition;
    }
}
