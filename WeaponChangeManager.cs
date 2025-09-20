using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

/*
 * >> Weapon Prefab ���� ���
 * - �翬�� �ش� Weapon ���簡 Network �� ����ȭ�Ǿ�� �ϹǷ�, PhotonView Component �� �־�� �Ѵ�
 * - �׸��� Box Collider Component �� �־�� �Ѵ�. Is Trigger �ɼ��� true �� �����Ѵ�
 * - Rigidbody Component �� �־�� �Ѵ�. Is Kinematic �ɼ��� true �� �����Ѵ�.
 *   �׷��߸� �߷� ������ �ȹް� Scene �� ���ְ� �ȴ�
 */

// �ش� script �� �翬�� prefab �� �پ��ִ�.
public class WeaponChangeManager : MonoBehaviour
{
    // ī�޶� ����
    private GameObject camObject;
    private CinemachineVirtualCamera cam;

    // Player �� ������ ���� weapon
    // YBot Prefab �� �پ��ִ� GameObject ���� ���⿡ �����Ѵ�.
    public GameObject[] weapons;

    // Weapon Spawn ����
    public GameObject testForWeapons;

    // ������ �Ʒ� Armor Weapon UI ���� ����� 
    private Image weaponIcon;
    private Text ammoAmtText;
    public Sprite[] weaponIcons; // ���� Resources ���� �󿡼� Weapon Sprite ���� Editor ���� ����
    public int[] ammoAmts;
    private int weaponNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Weapon UI
        {
            weaponIcon = GameObject.Find("WeaponUI").GetComponent<Image>();
            ammoAmtText = GameObject.Find("ArmorAmount").GetComponent<Text>();
        }
        // Camera
        {
            // �Ʒ� �ڵ�� ���� weapon change �� �־�� �ϴµ� �����Ƽ� ���⿡ ����
            camObject = GameObject.Find("PlayerCam");

            // Scene �� Instantiate �Ǿ� �ִ� ���� �÷��̾�� �߿��� ����  �����ϴ� �÷��̾��� 
            if (this.gameObject.GetComponent<PhotonView>().IsMine == true)
            {
                cam = camObject.GetComponent<CinemachineVirtualCamera>();
                cam.Follow = this.gameObject.transform;
                cam.LookAt = this.gameObject.transform;
            }
            else
            {
                // ���� �����ϴ� �÷��̾ �ƴ϶��, �� �ٸ� ����� �����ϴ� �÷��̾���
                // enable false �� ���� �ٸ� ��� �Ÿ� control ���� ���ϰ� �Ѵ�
                enabled = false;

                // �׷��ٸ� ���� movement �� �ǽð� ����ȭ�Ǿ �ٸ� ����鵵 �� �� �ְ� �Ϸ���
                // ��� �ؾ� �ұ� ?
                // PhotonView Component �� �ϴ� �߰��Ѵ�. �ش� Component ��
                // ���� ������Ʈ�� �������� ��Ʈ��ũ �� ����ȭ ��Ų��.
                // ���� ���� server ���� ������ �����ϰ��� �ϴ� ��� object �� PhotonView Component �� �־�� �Ѵ�
                // ex) Rig ������ update �Ǹ� �ش� ������ ����ȭ�ȴ�.
                // �׸��� �̷��� �߰��� Photon Transform View Component �� Photon View Component ��
                // Observed Components ����Ʈ�� �ڵ����� �߰��ȴ�

                // ����, Obeservable �� Observable Search �ɼ��� Manual �� �ٲ۴�. �׸���
                // ���� Component �� Animator �� �߰��ϴ� ������ �� ���� �ִ�
                // �̷��� Animator Component �� �߰��ϸ�, Photon Animator View Component �� �ڵ����� �߰��ȴ�
                // �׸��� �츮�� Animator �� �߰��ص� BlendVertical, BlendHorizontal �� �ڵ����� �߰��Ѵ�

                // ����, �̷��� Prefab �� ������ �����ϸ�, ���� Scene ���� �ش� object �� �����ϴ� ������
                // ����� �� �����Ƿ� �ٽ� �������ָ� �ȴ�.

            }
        }
        
        // Weapon Spawn
        {
            // �ϴ� Spawn �Ǵ� weapon �� ���� ù��° weapon �� �����´�
            // ã�� ��� slot �� �߰��� ���̴�.
            testForWeapons = GameObject.Find("Weapon1_Pickup(Clone)");

            if (testForWeapons == null)
            {
                // ���̻� weapon �� ã�� ���� ���̹Ƿ�, spawn �� �ؾ��Ѵ�.
                var spawner = GameObject.Find("SpawnScript");

                // Scene �� �ִ� SpawnScript ���� SpawnCharacters �� ã�Ƽ� �Լ� ȣ��
                // �̸� ���ؼ� ���� ù��° character �� ���ؼ��� �ش� SpwanWeaponsStart �Լ��� ȣ���Ѵ�
                spawner.GetComponent<SpawnCharacters>().SpwanWeaponsStart();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) &&
this.gameObject.GetComponent<PhotonView>().IsMine == true)
        {
            //weaponNumber++;
            this.GetComponent<PhotonView>().RPC("Change",
           RpcTarget.AllBuffered);

            if (weaponNumber > weapons.Length - 1)
            {
                weaponIcon.GetComponent<Image>().sprite = weaponIcons[0];
                ammoAmtText.text = ammoAmts[0].ToString();
                weaponNumber = 0;
            }

            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].SetActive(false);
            }

            weapons[weaponNumber].SetActive(true);
            weaponIcon.GetComponent<Image>().sprite = weaponIcons[weaponNumber];
            ammoAmtText.text = ammoAmts[weaponNumber].ToString();
        }
    }

    [PunRPC]
    public void Change()
    {
        weaponNumber++;
        if (weaponNumber > weapons.Length - 1)
        {
            weaponNumber = 0;
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[weaponNumber].SetActive(true);
    }
}
