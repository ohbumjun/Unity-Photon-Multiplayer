using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeed = 5.5f;
    public float rotateSpeed = 150.0f;
    public float jumpSpeed = 20;
    private Rigidbody rb;
    private Animator anim;
    private bool canJump = true;

    void Start()
    {
        moveSpeed = 3.5f;
        rotateSpeed = 150.0f;
        jumpSpeed = 20;

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

    }

    // Update: 매 프레임마다 호출됩니다.즉, 화면에 새로운 프레임이 나타날 때마다 한 번씩 실행되고,
    // 프레임 속도가 변동될 수 있어서 실행 간격이 일정하지 않습니다.
    // FixedUpdate: 화면 프레임과 무관하게, 설정된 일정한 시간 간격(기본값 0.02초)에 맞춰
    // 반복적으로 실행됩니다.주로 물리엔진과 동기화되어 고정된 주기로 처리해야 하는 작업에 적합합니다. ex) Rigid Body
    void FixedUpdate()
    {
        // Movement
        {
            // 방향 정보를 normalized
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
            // Rotation : Mouse 좌우로 회전
            Vector3 rotateY = new Vector3(0, rotateSpeed * Time.deltaTime * Input.GetAxis("Mouse X"), 0);
        
            if (movement != Vector3.zero) // 실질적인 움직임이 있을 때만 움직이게 하기
            {
                // Move
                rb.MovePosition(rb.position + 
                    transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime +
                     transform.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
            }

            // Rotate
            rb.MoveRotation(rb.rotation * Quaternion.Euler(rotateY));
        }

        // Animation
        {
            // Input 키에 따라서 Animator 의 BlenVertical, BlendHorizontal Value 를 조절한다.
            anim.SetFloat("BlendVertical", Input.GetAxis("Vertical"));
            anim.SetFloat("BlendHorizontal", Input.GetAxis("Horizontal"));
        }
    }

    // FixedUpdate는 프레임마다 호출되지 않고, 고정된 시간 간격으로 실행되기 때문에 
    // 입력 순간을 놓칠 수 있습니다.
    // 반면 Update는 프레임당 1회 실행되므로 
    // 키 입력(특히 GetButtonDown과 같이 눌린 "순간"을 감지하는 경우)이 더 정확하게 인식됩니다.
    // 예를 들어, 사용자가 빠르게 키를 눌렀다 뗄 경우 FixedUpdate 주기와 맞지 않으면 
    // 바로 반영이 안 되거나 입력이 누락될 수 있어, 정확한 입력 처리를 위해 Update에서 감지합니다
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                canJump = false;

                rb.AddForce(Vector3.up * jumpSpeed,  // Vector3.up은 (0,1,0) 방향, 즉 위쪽 방향 벡터

                    // 이 옵션은 오브젝트의 질량을 무시하고, 즉시 속도를 변화시킵니다.
                    // 순간적인 힘(점프 등)에는 적합한 모드이며, Force, Impulse, Acceleration 등
                    // 다른 ForceMode도 있으나 VelocityChange는 "즉각적인 속도 변화"에 사용합니다
                    // Rigidbody인 플레이어에게 위쪽 방향으로 순간적으로 힘을 가해 캐릭터가 점프하게 만듭니다.
                    ForceMode.VelocityChange);

                Debug.Log("Jump !");

                StartCoroutine(JumpAgain());
            }
            else
            {
                Debug.Log("Cannot Jump !");
            }
        }
       
    }
    IEnumerator JumpAgain()
    {
        yield return new WaitForSeconds(1f); // after 1 second, execute code below
        canJump = true;
    }
}
