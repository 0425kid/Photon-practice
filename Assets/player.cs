using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class player : MonoBehaviour
{
    public PhotonView PV;


    public float walkSpeed = 5.0f;
    public float runSpeed = 10.0f;
    public float jumpPower = 5.0f;
    private float rotX;
    private float rotY;
    [SerializeField] float sensitivity;


    public GameObject followCam;

    //�ִϸ��̼� ó��
    private Animator animator;
    public bool isMoving;
    public bool isRunning;

    public bool isLocalPlayer = false;
    public bool isJumping = false;

    //UI ó��
    public Text PlayerName;
    public string playername;

    public Rigidbody rigid;

    public GameObject cameras;
    public GameObject NameCanvas;

    private void Start()
    {
        try
        {
            animator = GetComponent<Animator>();
        }
        catch
        {
            Debug.Log("There is no animator");
        }

        if (PV.IsMine) isLocalPlayer = true;
        if (isLocalPlayer)
        {
            Debug.Log("�����̱���!");
            SetName(playername);
            cameras.SetActive(true);
            gameObject.tag = "LocalPlayer";
            NameCanvas.SetActive(false);
        }
        else
        {
            Debug.Log("������ �ƴϱ���!");
        }
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        isMoving = horizontalInput != 0 || verticalInput != 0;
        isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 movement = new Vector3(horizontalInput, 0.0f, verticalInput) * (isRunning ? runSpeed : walkSpeed) * Time.deltaTime;


        //if (!CheckWallCollision(movement))
        //{
        //    transform.Translate(movement);
        //}

        transform.Translate(movement);
        Jump();
        Turn();
        if (animator) SetAnimation();
    }

    void Turn()
    {
        if (Input.GetMouseButton(0))
        {
            rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;


            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            transform.rotation = rot;
        }
    }

    void SetAnimation()
    {
        if (isMoving)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (isRunning)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
    private void SetName(string name)
    {
        Debug.Log(name + "���� �����ϰڽ��ϴ�!");
        PV.RPC(nameof(SetNameRPC), RpcTarget.AllBuffered, name);
    }

    [PunRPC]
    public void SetNameRPC(string name)
    {
        PlayerName = GetComponentInChildren<Text>();
        PlayerName.text = name;

        if (isLocalPlayer)
        {
            Debug.Log("Local");
            //PlayerName.enabled = false;
        }
        else
        {
            Debug.Log("Not local");
        }
    }

    void Jump()
    {
        //�����̽� Ű�� ������ ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //�ٴڿ� ������ ������ ����
            if (!isJumping)
            {
                //print("���� ���� !");
                isJumping = true;
                rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            //���߿� ���ִ� �����̸� �������� ���ϵ��� ����
            else
            {
                //print("���� �Ұ��� !");
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�ٴڿ� ������
        if (collision.gameObject.CompareTag("Ground"))
        {
            //������ ������ ���·� ����
            isJumping = false;
        }
    }

    //bool CheckWallCollision(Vector3 movement)
    //{
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position, movement.normalized, out hit, movement.magnitude))
    //    {
    //        if (hit.collider.tag == "Wall")
    //        {
    //            // Adjust movement to stop before the wall
    //            transform.Translate(movement.normalized * (hit.distance - 0.1f));
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
