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
        ������ Tag �׸��� Unity���� ���� ������Ʈ�� **"����, �뵵, ����"�� ��Ȯ�� �����ϱ� ���� �ĺ���(���̺�)**�� ���˴ϴ�.  
        Tag�� �ܼ� ���ڿ��̱� ������, �����ڰ� ���ϴ� �������� ���� �����Ͽ� ����մϴ�.[1][2][3]

        ### �̹��� �� Tag �׸� ����

        - **Untagged**: �±װ� �������� ���� ������Ʈ(�⺻��)
        - **Respawn**: �÷��̾ ������Ʈ ���� ��Ȱ ��ġ(Spawn Point)�� ���� ������Ʈ�� �ο�
        - **Finish**: ���� ��������/���� �Ϸ� ����(��: ���� ���� ��) ������Ʈ�� �ο�
        - **EditorOnly**: ������(����ȯ��)������ ��������, ���� ����(���� ����)���� ������ ����� ���[1]
        - **MainCamera**: �� ī�޶� ���� ������Ʈ�� �ο�, �ڵ忡�� "���� ī�޶�" ã�� �� Ȱ��
        - **Player**: �÷��̾� ĳ����, ���ΰ� �� Player�� �ν��ؾ� �ϴ� ������Ʈ�� �ο�
        - **GameController**: ��ü ���� �帧(����, ����, ���� �� ����) ��� ������Ʈ�� �ο�

        ***

        ### Tag�� ���Ұ� Ȱ��

        - **�ǹ�**:  
          Tag�� ������Ʈ�� "Ư�� ����"�̳� "����"�� �����ϴ� **�ĺ� ��Ŀ**�Դϴ�.  
          �� ������Ʈ�� �ϳ��� �±׸� �Ҵ��� �� �ֽ��ϴ�.[2][1]
        - **Ȱ�� ����**:  
          - �浹(Trigger/Collision) ó������ Ư�� �±׸� �˻�(��: if (other.CompareTag("Player")) { ... })
          - ��, ������, ��ǥ����, Ư�� ����Ʈ �� "���� ��� ��ü" ���� �з� �� ó��
          - ������Ʈ ã��(GameObject.FindGameObjectsWithTag)
          - �� �Ǵ� ��ũ��Ʈ���� ȿ������ ���� �б�, �̺�Ʈ �и� ��[3][2]

        ***

        ### Tag�� Name�� ����

        | ����     | Tag                                                        | Name                              |
        |----------|------------------------------------------------------------|-----------------------------------|
        | ����     | ������Ʈ�� ����/���� �з� �ĺ���                          | ���� ������Ʈ�� �����ϴ� "���� �̸�"|
        | �ߺ�     | ���� ������Ʈ�� ���� �±� ���� �� ����                     | �� ������Ʈ���� ���� ���ڿ�        |
        | Ȱ��     | �뷮 �˻������� ���С�Trigger ��                            | Ư�� ������Ʈ ���� ������������      |
        | ����     | "Player", "Enemy", "Respawn", "Finish", ...               | "YBot", "GameManager", ...        |

        **����:**  
        - Tag�� **�ǹ� ��� �׷�ȭ, ���� �ĺ�, �뵵�� ��ũ��Ʈ �б� ó�� � ���**�ϸ�,  
        - Name�� **Ư�� ������Ʈ���� ���� �ĺ�**�� �� ����մϴ�.  
        ��, ���� Player ������Ʈ�� ���� �� ��� ���� "Player" Tag�� ������ ������ Name�� �ٸ� �� �ֽ��ϴ�.
        */

        // Prefab YBot �� Player �� ������ ��Ȳ
        if (other.CompareTag("Player"))
        {
            /*
            ## RPC(Remote Procedure Call)��?

            - ��Ʈ��ũ�� ����� ���� Ŭ���̾�Ʈ(�÷��̾�) �� **���� Ŭ���̾�Ʈ���� Ư�� �޼��带 �����Ű�� ���**�Դϴ�.
            - ���� ���, �� �÷��̾ ���� ��ư�� ������, �� RPC ȣ��� ���� �޼��尡 �ٸ� ����� ���ӿ����� ��� ����˴ϴ�.

            ## ��� ���

            1. **����� [PunRPC] �Ӽ� ���̱�**

            [PunRPC]
            void ChatMessage(string a, string b)
            {
                Debug.Log(string.Format("ChatMessage {0} {1}", a, b));
            }

            - [PunRPC]�� method ���� ���̸�, �� �Լ��� ��Ʈ��ũ RPC ȣ���� ���������ϴ�.

            2. **PhotonView�� ���� ���� ����**

            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("ChatMessage", RpcTarget.All, "jup", "and jup.");

            - PhotonView: ��� ��Ʈ��ũ ��ü�� �ݵ�� �޷� �־�� �ϴ� ������Ʈ(�ĺ���/����ȭ ���)
            - photonView.RPC(): "ChatMessage"��� �޼��带 ��Ʈ��ũ �󿡼� ������ ����(RpcTarget.All = ��� �� ����)���� �����ϰ� ��
            - �߰� ����("jup", "and jup.") �� �Ķ���͵� ����ȭ�Ǿ� ȣ�� ��� �Լ��� ��

            #### **MonoBehaviourPun ��� ��**
            - ���� PhotonView�� Get���� �ʰ�, �ٷ� this.photonView.RPC()ó�� ������ �� �� ����

            ## �ٽ� ���

            - **���� ����ȭ**: �� ���ӿ����� �Ͼ�� �ڵ尡 �ƴ϶�, [PunRPC]�� ������ �Լ���� ä��, ����, ȿ�� �� � �̺�Ʈ�� ��Ʈ��ũ�� �ٸ� �÷��̾�� �Բ� �����ų �� ����
            - **���**: �Լ��� [PunRPC] �Ӽ� �� photonView.RPC("�Լ���", Ÿ��, �Ķ����...)
            - **����**: RpcTarget.All(���), Others(�� ����), MasterClient(����), �� �پ��� Ÿ���� ����
            */


            // Weapon1_Pickup �� Mesh Renderer �� Box Collider �� ����
            // ���� �Լ��� ����Ǵ� ������ �� component ���� enable �����̴�.
            // ������, �ܼ��� �̷��Ը� �ϸ� �̰��� �� ��ǻ�Ϳ����� �����ϰ� �Ǵ� ���̴�
            // ���� ���� ���� �Լ� ������ Pick up audio �� ����ǰ�, �̰��� �� ��ǻ�Ϳ�����
            // ����ǰ� �ϴ� ���� �ƴ϶�, ��� ��ǻ�Ϳ��� ����ǰ� �ϰ� �ʹ�. 
            // �̸� ���� RPC �� ����� ���̴�
            this.GetComponent<PhotonView>().RPC("PlayPickupAudio", RpcTarget.All);
            this.GetComponent<PhotonView>().RPC("TurnOff", RpcTarget.All);
        }
    }

    [PunRPC]
    void PlayPickupAudio()
    {
        // ��. �׷��� �� �Լ��� WeaponPickup Script �� ���� ������ ��� ������ �ȴ�
        // (�翬�� PhotonView Component �� �־�� �Ѵ�)

        // �̶� �ش� Script �� Weapon ���鸸 �پ��ְ� Prefab YBot ���� ����
        // ���� Weapon ������ ����� ���̴�.
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
            // Weapon2 �� ��쿡�� �Ʒ� ��û ���� sub game object ���� �־���.
            // �̰��� �ϴ� gun �̶�� �̸��� game object �� ����ΰ�
            // mesh renderer �� ���°� �ƴ϶� �ƿ� �ش� gun �� active false �� ���������� ������ ȿ�� �ֱ�
            this.transform.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(WaitToRespawn());
    }

    IEnumerator WaitToRespawn()
    {
        // 5�� ��ٷȴٰ�
        yield return new WaitForSeconds(respawnTime);

        // �ٽ� Mesh Renderer �� Box Collider �� �Ҵ�
        // ��, ���� ���, Player1 �� �� Weapon �� ������ 5�� ���Ŀ� �ٽ� Weapon �� ��Ÿ���� �ȴ�
        // �׷��� �׶����� ��μ� Player2 �� �� Weapon �� ���� �� �ְ� �ȴ�
        // �׸��� �̷��� �ϴ� �ڵ带 ��� ��ǻ�Ϳ��� ����ǰ� �ϱ� ���ؼ�
        // RPC �� ����Ѵ�
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
            // Weapon2 �� ��쿡�� �Ʒ� ��û ���� sub game object ���� �־���.
            // �̰��� �ϴ� gun �̶�� �̸��� game object �� ����ΰ�
            // mesh renderer �� ���°� �ƴ϶� �ƿ� �ش� gun �� active false �� ���������� ������ ȿ�� �ֱ�
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        this.transform.gameObject.GetComponent<Collider>().enabled = true;
    }
}
