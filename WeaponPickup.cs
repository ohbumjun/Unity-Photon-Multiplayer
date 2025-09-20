using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponPickup : MonoBehaviour
{
    private AudioSource audioPlayer;
    public float respawnTime = 5.0f;
    public int weaponType = 1; // weapon2 : 2

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        각각의 Tag 항목은 Unity에서 게임 오브젝트의 **"역할, 용도, 범주"를 명확히 구분하기 위한 식별자(레이블)**로 사용됩니다.  
        Tag는 단순 문자열이기 때문에, 개발자가 원하는 기준으로 직접 정의하여 사용합니다.[1][2][3]

        ### 이미지 내 Tag 항목 설명

        - **Untagged**: 태그가 지정되지 않은 오브젝트(기본값)
        - **Respawn**: 플레이어나 오브젝트 등의 부활 위치(Spawn Point)로 사용될 오브젝트에 부여
        - **Finish**: 게임 스테이지/레벨 완료 지점(예: 골인 지점 등) 오브젝트에 부여
        - **EditorOnly**: 에디터(개발환경)에서는 보이지만, 실제 빌드(게임 실행)에선 제거할 대상을 명시[1]
        - **MainCamera**: 주 카메라 역할 오브젝트에 부여, 코드에서 "메인 카메라" 찾을 때 활용
        - **Player**: 플레이어 캐릭터, 주인공 등 Player로 인식해야 하는 오브젝트에 부여
        - **GameController**: 전체 게임 흐름(점수, 시작, 종료 등 관리) 담당 오브젝트에 부여

        ***

        ### Tag의 역할과 활용

        - **의미**:  
          Tag는 오브젝트에 "특정 역할"이나 "종류"를 정의하는 **식별 마커**입니다.  
          한 오브젝트에 하나의 태그만 할당할 수 있습니다.[2][1]
        - **활용 예시**:  
          - 충돌(Trigger/Collision) 처리에서 특정 태그만 검색(예: if (other.CompareTag("Player")) { ... })
          - 적, 아이템, 목표지점, 특수 포인트 등 "동일 기능 객체" 빠른 분류 및 처리
          - 오브젝트 찾기(GameObject.FindGameObjectsWithTag)
          - 씬 또는 스크립트에서 효율적인 상태 분기, 이벤트 분리 등[3][2]

        ***

        ### Tag와 Name의 차이

        | 구분     | Tag                                                        | Name                              |
        |----------|------------------------------------------------------------|-----------------------------------|
        | 개념     | 오브젝트의 역할/종류 분류 식별자                          | 개별 오브젝트를 구분하는 "고유 이름"|
        | 중복     | 여러 오브젝트가 동일 태그 가질 수 있음                     | 한 오브젝트만의 고유 문자열        |
        | 활용     | 대량 검색·종류 구분·Trigger 등                            | 특정 오브젝트 직접 참조·관리용      |
        | 예시     | "Player", "Enemy", "Respawn", "Finish", ...               | "YBot", "GameManager", ...        |

        **정리:**  
        - Tag는 **의미 기반 그룹화, 빠른 식별, 용도별 스크립트 분기 처리 등에 사용**하며,  
        - Name은 **특정 오브젝트만을 직접 식별**할 때 사용합니다.  
        즉, 여러 Player 오브젝트가 있을 때 모두 같은 "Player" Tag를 가져도 각각의 Name은 다를 수 있습니다.
        */

        // Prefab YBot 을 Player 로 세팅한 상황
        if (other.CompareTag("Player"))
        {
            /*
            ## RPC(Remote Procedure Call)란?

            - 네트워크에 연결된 여러 클라이언트(플레이어) 중 **원격 클라이언트에서 특정 메서드를 실행시키는 기능**입니다.
            - 예를 들어, 한 플레이어가 공격 버튼을 누르면, 이 RPC 호출로 같은 메서드가 다른 사용자 게임에서도 즉시 실행됩니다.

            ## 사용 방식

            1. **방법에 [PunRPC] 속성 붙이기**

            [PunRPC]
            void ChatMessage(string a, string b)
            {
                Debug.Log(string.Format("ChatMessage {0} {1}", a, b));
            }

            - [PunRPC]를 method 위에 붙이면, 이 함수는 네트워크 RPC 호출이 가능해집니다.

            2. **PhotonView를 통한 원격 실행**

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("ChatMessage", RpcTarget.All, "jup", "and jup.");

            - PhotonView: 모든 네트워크 객체에 반드시 달려 있어야 하는 컴포넌트(식별자/동기화 담당)
            - photonView.RPC(): "ChatMessage"라는 메서드를 네트워크 상에서 지정된 대상들(RpcTarget.All = 모든 룸 유저)에게 실행하게 됨
            - 추가 전달("jup", "and jup.") 등 파라미터도 동기화되어 호출 대상 함수로 들어감

            #### **MonoBehaviourPun 상속 시**
            - 따로 PhotonView를 Get하지 않고, 바로 this.photonView.RPC()처럼 간단히 쓸 수 있음

            ## 핵심 요약

            - **원격 동기화**: 내 게임에서만 일어나는 코드가 아니라, [PunRPC]로 지정된 함수라면 채팅, 공격, 효과 등 어떤 이벤트도 네트워크로 다른 플레이어와 함께 실행시킬 수 있음
            - **방법**: 함수에 [PunRPC] 속성 → photonView.RPC("함수명", 타겟, 파라미터...)
            - **종류**: RpcTarget.All(모두), Others(나 빼고), MasterClient(방장), 등 다양한 타겟팅 가능
            */


            // Weapon1_Pickup 의 Mesh Renderer 랑 Box Collider 를 끈다
            // 현재 함수가 실행되는 시점에 이 component 들을 enable 끌것이다.
            // 하지만, 단순히 이렇게만 하면 이것은 내 컴퓨터에서만 동작하게 되는 것이다
            // 나는 현재 여기 함수 내에서 Pick up audio 가 실행되고, 이것이 내 컴퓨터에서만
            // 실행되게 하는 것이 아니라, 모든 컴퓨터에서 실행되게 하고 싶다. 
            // 이를 위해 RPC 를 사용할 것이다
            this.GetComponent<PhotonView>().RPC("PlayPickupAudio", RpcTarget.All);
            this.GetComponent<PhotonView>().RPC("TurnOff", RpcTarget.All);
        }
    }

    [PunRPC]
    void PlayPickupAudio()
    {
        // 자. 그러면 이 함수가 WeaponPickup Script 가 붙은 곳에서 모두 실행이 된다
        // (당연히 PhotonView Component 가 있어야 한다)

        // 이때 해당 Script 는 Weapon 에들만 붙어있고 Prefab YBot 에는 없다
        // 따라서 Weapon 에서만 실행될 것이다.
        audioPlayer.Play();
    }

    [PunRPC]
    void TurnOff()
    {
        if (weaponType == 1)
        {
            this.transform.gameObject.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            // Weapon2 의 경우에는 아래 엄청 많은 sub game object 들이 있었다.
            // 이것을 일단 gun 이라는 이름의 game object 로 묶어두고
            // mesh renderer 를 끄는게 아니라 아예 해당 gun 을 active false 로 만들어버려서 동일한 효과 주기
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(WaitToRespawn());
    }

    IEnumerator WaitToRespawn()
    {
        // 5초 기다렸다가
        yield return new WaitForSeconds(respawnTime);

        // 다시 Mesh Renderer 와 Box Collider 를 켠다
        // 즉, 예를 들어, Player1 이 이 Weapon 을 먹으면 5초 이후에 다시 Weapon 이 나타나게 된다
        // 그러면 그때가서 비로소 Player2 가 이 Weapon 을 먹을 수 있게 된다
        // 그리고 이렇게 하는 코드를 모든 컴퓨터에서 실행되게 하기 위해서
        // RPC 를 사용한다
        this.GetComponent<PhotonView>().RPC("TurnOn", RpcTarget.All);
    }

    [PunRPC]
    void TurnOn()
    {
        if (weaponType == 1)
        {
            this.transform.gameObject.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            // Weapon2 의 경우에는 아래 엄청 많은 sub game object 들이 있었다.
            // 이것을 일단 gun 이라는 이름의 game object 로 묶어두고
            // mesh renderer 를 끄는게 아니라 아예 해당 gun 을 active false 로 만들어버려서 동일한 효과 주기
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        this.transform.gameObject.GetComponent<Collider>().enabled = true;
    }
}
