using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetUp : MonoBehaviour, IPunInstantiateMagicCallback
{
    // void Start()
    void OnEnable()
    {

        // Scene 상에 Instantiate 되어 있는 여러 플레이어들 중에서 내가  조종하는 플레이어라면 
        if (this.gameObject.GetComponent<PhotonView>().IsMine == false)
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
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        // info.Sender: 이 오브젝트를 생성한 플레이어
        // info.photonView: 이 오브젝트의 PhotonView
        // 예시: Player의 TagObject에 이 오브젝트를 저장
        info.Sender.TagObject = this.gameObject;

        // 추가로 필요한 초기화 코드 작성
        Debug.Log($"플레이어가 오브젝트 풀로 오브젝트를 생성했습니다.");
    }
}
