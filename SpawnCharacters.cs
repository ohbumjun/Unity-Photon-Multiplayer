using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacters : MonoBehaviour
{
    public GameObject character; // Prefab 으로 만든 Resource/Folder 의 YBot 으로 세팅한다 
    public Transform[] spawnPoints; // 이것도 내가 만들어둔 Transform Component 를 가진 SpanwPoint 들을 드래그 드롭해서 세팅

    public GameObject[] weapons; // 무기들 
    public Transform[] weaponSpawnPoints; // 무기 spawn 위치들

    public float weaponRespawnTime = 10.0f; // 무기 respawn 시간


    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            // Scene Load 와 Charactor Spawning 사이에 어느정도 간격을 두어야 한다 
            // player, sever lag 가 발생할 수 있다
            StartCoroutine(WaitToSpawn());
        }
        else
        {
            Debug.Log("Not Connected");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(1.0f); // 이 2초라는 시간은 임의로 설정한 것이다

        // Network 에 연결된 경우에만 Character 들을 Spawn 시킬 것이다.
        // Photon Network 상에 YBot Prefab 을 실제 Scene 에 Instantiate 시킬 것이다
        // 이때 우리가 Instantiate 시키고자 하는 Prefab 에는 Photon View Component 가 장착된 상태이다
        // 단, 일반 Instantiate 함수는 부족하다. 왜냐하면 그것은 그저 나의 local computer 에서만 instantiate
        // 시키게 되는 것이기 때문이다

        // 이때 character.name 을 찾는 과정은, Resource Folder 로 가서
        // 해당 name 과 정확히 일치하는 character 를 찾는 것이다

        // Photon Network 에서는 Character 가 생성될 때마다 count 가 올라간다. 해당 값을 사용한다
        Debug.Log($"CountOfPlayers : {PhotonNetwork.CountOfPlayers}");
        Debug.Log($"CurrentRoom.PlayerCount : {PhotonNetwork.CurrentRoom.PlayerCount}");

        // 아래 코드의 경우 Room 이 1개라면 의도한 대로 동작. 하지만 여러 Room 일 경우에는 Room 별로 CountOfPlayers 를 고려해야 한다.
        // CountOfPlayers 는 Server 전체의 Player 수를 나타내기 때문이다
        // PhotonNetwork.Instantiate(character.name,
        //     spawnPoints[PhotonNetwork.CountOfPlayers - 1].position,
        //     spawnPoints[PhotonNetwork.CountOfPlayers - 1].rotation);

        PhotonNetwork.Instantiate(character.name,
            spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].position,
            spawnPoints[PhotonNetwork.CurrentRoom.PlayerCount - 1].rotation);

        Debug.Log("Connected Instantiate");
    }

    public void SpwanWeaponsStart()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            GameObject weapon = PhotonNetwork.Instantiate(weapons[i].name,
                weaponSpawnPoints[i].position,
                weaponSpawnPoints[i].rotation);
        }
    }
}
