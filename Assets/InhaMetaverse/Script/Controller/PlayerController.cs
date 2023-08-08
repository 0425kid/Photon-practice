using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class PlayerController : MonoBehaviour
{
    public PhotonView PV;

    // �ִϸ��̼�
    public Animator animator;

    public Transform tpRoot;
    public Transform tpRig;
    public Transform fpRoot;
    public Transform fpRig;

    public Rigidbody rigid;

    // �̵� class �з��� �̿��� script
    //[HideInInspector] public Movement3D movement3d;

    private Vector3 _moveDir;
    private Vector3 _worldMove;
    private Vector3 _rotation;

    /*
     player ���� �ɼ�
     */
    private bool isCurrentFp;
    public bool isMoving;
    public bool isRunning;
    public bool isGrounded;

    /*
     �̵� �ɼ�
     */
    [Range(1f, 10f), Tooltip("�̵��ӵ�")]
    public float speed = 3f;
    [Range(1f, 3f), Tooltip("�޸��� �̵��ӵ� ���� ���")]
    public float runningCoef = 1.5f;
    [Range(1f, 10f), Tooltip("���� ����")]
    public float jumpForce = 5.5f;
    [Tooltip("�������� üũ�� ���̾� ����")]
    public LayerMask groundLayerMask = -1;

    private float _groundCheckRadius;

    public string email, socketId;
    public bool isOnline, isLocalPlayer;

    public Transform cameraToTarget;

    public float verticalSpeed = 3.0f;

    public float rotateSpeed = 150f;

    float h;

    float v;

    public void Set()
    {
        Debug.Log("����!");
        isLocalPlayer = PV.IsMine;
        //tpCamera.enabled = isLocalPlayer;

        GetComponentInChildren<Text>().text = name;

        //TryGetComponent(out rigid);
        //if (rigid != null)
        //{
        //    rigid.constraints = RigidbodyConstraints.FreezeRotation;
        //}
    }

    //public void Awake()
    //{
    //    isLocalPlayer = PV.IsMine;
    //}

    void Start()
    {
        Set();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsGround();
        SetValuesByKeyInput();   
    }

    private void SetValuesByKeyInput()
    {
        float h = 0f, v = 0f;

        if (Input.GetKey(KeyCode.W)) v += 1.0f;
        if (Input.GetKey(KeyCode.S)) v -= 1.0f;
        if (Input.GetKey(KeyCode.A)) h -= 1.0f;
        if (Input.GetKey(KeyCode.D)) h += 1.0f;

        isMoving = h != 0 || v != 0;
        isRunning = Input.GetKey(KeyCode.LeftShift);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        _moveDir = new Vector3(h, 0f, v).normalized;

        Move();

    }

    // �̵� ���� ���� �ʱ�ȭ 
    private void SendMoveInfo(float horizontal, float vertical)
    {
        _moveDir = new Vector3(horizontal, 0f, vertical).normalized;

        if (isCurrentFp)
        {
            _worldMove = fpRoot.TransformDirection(_moveDir);
        }
        else
        {
            _worldMove = tpRoot.TransformDirection(_moveDir);
        }

        Move();
    }

    /*player �̵� ���� �Լ�*/
    private void Move()
    {


        // �ִϸ��̼� ���� ����
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

    private void Jump()
    {
        if (!isGrounded) return;

        // �ϰ� �� ���� �� �ӵ��� �ջ���� �ʵ��� �ӵ� �ʱ�ȭ
        rigid.velocity = Vector3.zero;

        rigid.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    private void CheckIsGround()
    {
        Vector3 ro = transform.position + Vector3.up;
        Vector3 rd = Vector3.down;
        Ray ray = new Ray(ro, rd);

        const float rayDist = 500f;
        const float threshold = 0.01f;

        bool cast =
            Physics.SphereCast(ray, _groundCheckRadius, out var hit, rayDist, groundLayerMask);

        float _distFromGround = cast ? (hit.distance - 1f + _groundCheckRadius) : float.MaxValue;
        isGrounded = _distFromGround <= _groundCheckRadius + threshold;
    }

    public void SetName(string name)
    {
        GetComponentInChildren<Text>().text = name;
    }
}