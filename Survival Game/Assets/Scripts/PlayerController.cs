using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float walkSpeed; //이스크립트내에서만변경가능,inspector창에서확인가능

    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    private bool isRun; //뛰고있는지여부저장변수
    private bool isGround = true;//땅에 있는지 여부,t일때만점프할수있게
    private bool isCrouch;//앉아있는지여부

    [SerializeField]
    private float crouchPosY;//앉았을 때 얼마나 앉을지 결정하는 변수
    private float originPosY;//숙였다가 돌아오기 위한 값 저장
    private float applyCouchPosY; //위 두 개의 값을 여기에 넣어서 이 변수를 사용

    private CapsuleCollider capsuleCollider; //콜라이더가땅에붙어있는지

    [SerializeField]
    private float lookSensitivity; //카메라의민감도

    [SerializeField]
    private float cameraRotationLimit; //고개를들때각도제한
    private float currentCameraRotationX=0f;//정면을바라보고있게

    //필요한컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid; //플레이어의몸체
    private GunController theGunController;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        theGunController = FindObjectOfType<GunController>();

        //초기화
        originPosY = theCamera.transform.localPosition.y;
        applyCouchPosY = originPosY;//기본서있는상태
    }

    // Update is called once per frame 1초에 60회 실행
    void Update()
    {
        IsGround();
        TryJump();
        TryRun(); //뛰,걷 판단하고->Move실행해야됨
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
    }
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }
    private void Crouch()
    {
        isCrouch = !isCrouch;

        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCouchPosY = originPosY;
        }
        StartCoroutine(CrouchCoroutine());
    }
    IEnumerator CrouchCoroutine() //코루틴함수, 병렬처리를위해만들어진개념,빠르게왔다갔다하는식으로
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY!=applyCouchPosY)//원하느y값이될때까지
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCouchPosY, 0.3f);//보간함수사용해서자연스럽게
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null; //한프레임동안대기
        }
        theCamera.transform.localPosition = new Vector3(0, applyCouchPosY, 0);
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f); //바닥에닿아있으면t를반환함
    }
    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    private void Jump()
    {
        if (isCrouch)
            Crouch(); //앉아있을때점프하면일어나게

        myRigid.velocity = transform.up * jumpForce;//갑자기velocity를변경해주는방법으로점프구현
    }
    private void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
    private void Running()
    {
        if (isCrouch)//앉아있을때달리기시도
            Crouch();

        theGunController.CancleFineSight();

        isRun = true;
        applySpeed = runSpeed;
    }
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");//오른쪽방향키1,왼쪽-1,안누르면0 /X가 좌우 Z가 위아래
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;//transform의right값(1,0,0)을 쓴다
        Vector3 _moveVertical = transform.forward * _moveDirZ;//(0,0,1)에다가

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime); //움직임구현

    }
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y"); //위아래로마우스움직여서고개를들기
        float _cameraRotationX = _xRotation * lookSensitivity;//마우스를올렸다고45도까지확올라가면안되니까
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        //current값이 -45도~45도사이이게 가두기Clamp, 초과하면 45로 되고...

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation() //좌우캐릭터회전
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
