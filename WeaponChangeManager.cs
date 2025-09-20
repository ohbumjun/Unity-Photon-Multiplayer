using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

/*
 * >> Weapon Prefab 설정 방법
 * - 당연히 해당 Weapon 존재가 Network 상에 동기화되어야 하므로, PhotonView Component 이 있어야 한다
 * - 그리고 Box Collider Component 도 있어야 한다. Is Trigger 옵션을 true 로 설정한다
 * - Rigidbody Component 도 있어야 한다. Is Kinematic 옵션을 true 로 설정한다.
 *   그래야만 중력 영향을 안받고 Scene 상에 떠있게 된다
 */

// 해당 script 는 당연히 prefab 에 붙어있다.
public class WeaponChangeManager : MonoBehaviour
{
    // 카메라 세팅
    private GameObject camObject;
    private CinemachineVirtualCamera cam;

    // Player 가 가지는 여러 weapon
    // YBot Prefab 에 붙어있는 GameObject 들을 여기에 세팅한다.
    public GameObject[] weapons;

    // Weapon Spawn 세팅
    public GameObject testForWeapons;

    // 오른쪽 아래 Armor Weapon UI 관련 내용들 
    private Image weaponIcon;
    private Text ammoAmtText;
    public Sprite[] weaponIcons; // 실제 Resources 폴더 상에서 Weapon Sprite 들을 Editor 에서 세팅
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
            // 아래 코드는 원래 weapon change 에 있어야 하는데 귀찮아서 여기에 넣음
            camObject = GameObject.Find("PlayerCam");

            // Scene 상에 Instantiate 되어 있는 여러 플레이어들 중에서 내가  조종하는 플레이어라면 
            if (this.gameObject.GetComponent<PhotonView>().IsMine == true)
            {
                cam = camObject.GetComponent<CinemachineVirtualCamera>();
                cam.Follow = this.gameObject.transform;
                cam.LookAt = this.gameObject.transform;
            }
            else
            {
                // 내가 조종하는 플레이어가 아니라면, 즉 다른 사람이 조종하는 플레이어라면
                // enable false 로 만들어서 다른 사람 거를 control 하지 못하게 한다
                enabled = false;

                // 그렇다면 나의 movement 가 실시간 동기화되어서 다른 사람들도 볼 수 있게 하려면
                // 어떻게 해야 할까 ?
                // PhotonView Component 을 일단 추가한다. 해당 Component 가
                // 현재 오브젝트의 움직임을 네트워크 상에 동기화 시킨다.
                // 물론 내가 server 측에 정보를 전달하고자 하는 모든 object 에 PhotonView Component 이 있어야 한다
                // ex) Rig 정보도 update 되면 해당 정보도 동기화된다.
                // 그리고 이렇게 추가된 Photon Transform View Component 은 Photon View Component 의
                // Observed Components 리스트에 자동으로 추가된다

                // 이후, Obeservable 의 Observable Search 옵션을 Manual 로 바꾼다. 그리고
                // 같은 Component 의 Animator 도 추가하는 식으로 할 수도 있다
                // 이렇게 Animator Component 도 추가하면, Photon Animator View Component 가 자동으로 추가된다
                // 그리고 우리가 Animator 에 추가해둔 BlendVertical, BlendHorizontal 도 자동으로 추가한다

                // 한편, 이렇게 Prefab 의 내용을 수정하면, 기존 Scene 에서 해당 object 를 참조하던 정보가
                // 사라질 수 있으므로 다시 연결해주면 된다.

            }
        }
        
        // Weapon Spawn
        {
            // 일단 Spawn 되는 weapon 중 가장 첫번째 weapon 을 가져온다
            // 찾을 경우 slot 에 추가될 것이다.
            testForWeapons = GameObject.Find("Weapon1_Pickup(Clone)");

            if (testForWeapons == null)
            {
                // 더이상 weapon 을 찾지 못한 것이므로, spawn 을 해야한다.
                var spawner = GameObject.Find("SpawnScript");

                // Scene 에 있는 SpawnScript 안의 SpawnCharacters 를 찾아서 함수 호출
                // 이를 통해서 가장 첫번째 character 에 대해서만 해당 SpwanWeaponsStart 함수를 호출한다
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
