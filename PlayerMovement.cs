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

    // Update: �� �����Ӹ��� ȣ��˴ϴ�.��, ȭ�鿡 ���ο� �������� ��Ÿ�� ������ �� ���� ����ǰ�,
    // ������ �ӵ��� ������ �� �־ ���� ������ �������� �ʽ��ϴ�.
    // FixedUpdate: ȭ�� �����Ӱ� �����ϰ�, ������ ������ �ð� ����(�⺻�� 0.02��)�� ����
    // �ݺ������� ����˴ϴ�.�ַ� ���������� ����ȭ�Ǿ� ������ �ֱ�� ó���ؾ� �ϴ� �۾��� �����մϴ�. ex) Rigid Body
    void FixedUpdate()
    {
        // Movement
        {
            // ���� ������ normalized
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        
            // Rotation : Mouse �¿�� ȸ��
            Vector3 rotateY = new Vector3(0, rotateSpeed * Time.deltaTime * Input.GetAxis("Mouse X"), 0);
        
            if (movement != Vector3.zero) // �������� �������� ���� ���� �����̰� �ϱ�
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
            // Input Ű�� ���� Animator �� BlenVertical, BlendHorizontal Value �� �����Ѵ�.
            anim.SetFloat("BlendVertical", Input.GetAxis("Vertical"));
            anim.SetFloat("BlendHorizontal", Input.GetAxis("Horizontal"));
        }
    }

    // FixedUpdate�� �����Ӹ��� ȣ����� �ʰ�, ������ �ð� �������� ����Ǳ� ������ 
    // �Է� ������ ��ĥ �� �ֽ��ϴ�.
    // �ݸ� Update�� �����Ӵ� 1ȸ ����ǹǷ� 
    // Ű �Է�(Ư�� GetButtonDown�� ���� ���� "����"�� �����ϴ� ���)�� �� ��Ȯ�ϰ� �νĵ˴ϴ�.
    // ���� ���, ����ڰ� ������ Ű�� ������ �� ��� FixedUpdate �ֱ�� ���� ������ 
    // �ٷ� �ݿ��� �� �ǰų� �Է��� ������ �� �־�, ��Ȯ�� �Է� ó���� ���� Update���� �����մϴ�
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (canJump)
            {
                canJump = false;

                rb.AddForce(Vector3.up * jumpSpeed,  // Vector3.up�� (0,1,0) ����, �� ���� ���� ����

                    // �� �ɼ��� ������Ʈ�� ������ �����ϰ�, ��� �ӵ��� ��ȭ��ŵ�ϴ�.
                    // �������� ��(���� ��)���� ������ ����̸�, Force, Impulse, Acceleration ��
                    // �ٸ� ForceMode�� ������ VelocityChange�� "�ﰢ���� �ӵ� ��ȭ"�� ����մϴ�
                    // Rigidbody�� �÷��̾�� ���� �������� ���������� ���� ���� ĳ���Ͱ� �����ϰ� ����ϴ�.
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
