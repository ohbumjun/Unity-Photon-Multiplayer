using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

// �ش� ��ũ��Ʈ�� ybot �� �����ȴ� 
// �� ĳ���Ͱ� � ��ư�� Ŭ���ߴ����� ���� �ڽ��� ������ �ٲ۴�
// ���� : �̸� prefab ybot �� ������ ����, �ش� ybot �� �����ϴ� ��ũ�� ���� �� ������ Ȯ��
public class DisplayColor : MonoBehaviour
{
    public int[] buttonNumbers; // 0 ~ 5 (6�� ��ư), �� ĳ���Ͱ� � ��ư�� Ŭ���ߴ°�
    public int[] viewID; // �� ĳ������ Photon View ID. �̸� ���� � ĳ���Ͱ� � ��ư�� Ŭ���ߴ��� �� �� �ִ�
                         // ex) viewID[0] = 1001 ��, 0��° button  (������)�� Ŭ���� ĳ������ Photon View ID �� 1001 �̶� ��
    public Color32[] colors; // ���� Editor �󿡼� �����ϴ� �����

    private GameObject namesObject;

    public void Start()
    {
    }

    // Ư�� ĳ���Ͱ� ���� ��ư�� Ŭ���ϸ�
    // ��� player �� ���� �Ʒ� �Լ��� ����ȴ�.
    // �̸� ���� ��� Ŭ���̾�Ʈ���� AssignColor �� ����ȴ�.
    public void ChooseColor()
    {
        GetComponent<PhotonView>().RPC(
             "AssignColor",
             RpcTarget.AllBuffered); // ���߿� �����ϴ� ����鵵 �� RPC �� �ް� �ȴ�
    }
    [PunRPC]
    void AssignColor()
    {
        if (namesObject == null)
        {
            namesObject = GameObject.Find("NamesBG");
        }

        // ex) Player 1 ~ 6 �� ���� ��. 6���� ��ǻ�Ϳ��� Player 1 �� ���� �Ʒ��� �Լ��� ȣ��Ǵ� ��
        PhotonView pv = GetComponent<PhotonView>();

        for (int buttonNumber = 0; buttonNumber < viewID.Length; buttonNumber++)
        {
            // �� Player ������Ʈ���� �ڽ��� PhotonView�� ViewID��
            // viewID[i]�� ������, �ش� Player�� ������ colors[i]�� �����մϴ�.
            // ����: ������(0��) ��ư�� ���� Player�� ViewID�� 1001�̸�,
            // ��� Player ������Ʈ���� viewID[0] == pv.ViewID�� Player�� ������ ���������� �ٲ�ϴ�.

            if (pv.ViewID == viewID[buttonNumber])
            {
                // �ش� ĳ������ ������ colors[i] �� �ٲ۴�
                // Ybot �󿡼� child �߿� Alpha_Surface GameObject �� �ִ�
                // �װ��� Material �� ������ ������ ���̴� 
                // �̸� ���ؼ� Material �� set �Ǿ� �ִ� Renderer Component �� �����´�
                this.transform.GetChild(1).GetComponent<Renderer>().material.color = colors[buttonNumber];

                // �׸��� nick name �� ������� Floor Layout ���� ����ʿ� health bar ���� � �����Ѵ�.
                if (namesObject == null)
                {
                    Debug.Log("No namesObject");
                }

                NicknameScript nickNameScript = namesObject.GetComponent<NicknameScript>();

                if (nickNameScript == null)
                {
                    Debug.Log("No nickname");
                }

                // nickname, health bar Ȱ��ȭ
                nickNameScript.nickNames[buttonNumber].gameObject.SetActive(true);
                nickNameScript.healthBars[buttonNumber].gameObject.SetActive(true);

                nickNameScript.nickNames[buttonNumber].text = pv.Owner.NickName;
            }
        }
    }

    // �Ʒ��� ���� �ѹ� �� AssignColor �� RPC �� �ƴϰ� ȣ���ϸ�, 2��° Player ���� ������ �ȹٲ��.
    // public void ChooseColor(int buttonNumber, int myID)
    // {
    //     Debug.Log("Choose Color Call");
    // 
    //     AssignColor(buttonNumber, myID); // �׳� ���� ȣ��
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
