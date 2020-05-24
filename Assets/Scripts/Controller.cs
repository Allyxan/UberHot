using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class Controller : MonoBehaviour
{
    public static bool bb = false;
    bool testAnim = false;
    public static Collider otherEnemy;
    public LayerMask Enemy;
    public static Controller Instance { get; protected set; }

    public Camera MainCamera;
    public Camera WeaponCamera;
    public Transform CameraPosition;
    public Transform WeaponPosition;

    [Header("Control Settings")]
    public float MouseSensitivity = 100.0f;
    public float PlayerSpeed = 5.0f;
    public float RunningSpeed = 7.0f;
    public float JumpSpeed = 5.0f;

    [Header("Audio")]
    public AudioClip JumpingAudioClip;
    public AudioClip LandingAudioClip;

    float m_VerticalSpeed = 0.0f;
    bool m_IsPaused = false;
    int m_CurrentWeapon;

    float m_VerticalAngle, m_HorizontalAngle;
    public float Speed { get; private set; } = 0.0f;

    public bool LockControl { get; set; }
    public bool CanPause { get; set; } = true;

    public bool Grounded => m_Grounded;

    CharacterController m_CharacterController;

    bool m_Grounded;
    float m_GroundedTimer;
    float m_SpeedAtJump = 0.0f;

    int enemys;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemys = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_IsPaused = false;
        m_Grounded = true;

        MainCamera.transform.SetParent(CameraPosition, false);
        MainCamera.transform.localPosition = Vector3.zero;
        MainCamera.transform.localRotation = Quaternion.identity;
        m_CharacterController = GetComponent<CharacterController>();

        m_VerticalAngle = 0.0f;
        m_HorizontalAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        bool wasGrounded = m_Grounded;
        bool loosedGrounding = false;
        //we define our own grounded and not use the Character controller one as the character controller can flicker
        //between grounded/not grounded on small step and the like. So we actually make the controller "not grounded" only
        //if the character controller reported not being grounded for at least .5 second;
        if (!m_CharacterController.isGrounded)
        {
            if (m_Grounded)
            {
                m_GroundedTimer += Time.deltaTime;
                if (m_GroundedTimer >= 0.5f)
                {
                    loosedGrounding = true;
                    m_Grounded = false;
                }
            }
        }
        else
        {
            m_GroundedTimer = 0.0f;
            m_Grounded = true;
        }

        Speed = 0;
        Vector3 move = Vector3.zero;
        if (!m_IsPaused && !LockControl)
        {

            if (Input.GetKeyDown(KeyCode.K))
            {
                testAnim = true;
                StartCoroutine(PlaySecretKey());
            }


            if ((BreakResDoorScript.breakingdoor == false) & (BreakAdmDoorScript.breakingdoor == false))
            {
                if (testAnim == false)
                {
                    if (m_Grounded && Input.GetButtonDown("Jump"))
                    {
                        m_VerticalSpeed = JumpSpeed;
                        m_Grounded = false;
                        loosedGrounding = true;
                        //FootstepPlayer.PlayClip(JumpingAudioCLip, 0.8f,1.1f);
                    }

                    bool running = Input.GetKey(KeyCode.LeftShift);
                    float actualSpeed = running ? RunningSpeed : PlayerSpeed;

                    if (loosedGrounding)
                    {
                        m_SpeedAtJump = actualSpeed;
                    }

                    // Move around with WASD

                    move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                    if (move.sqrMagnitude > 1.0f)
                        move.Normalize();

                    float usedSpeed = m_Grounded ? actualSpeed : m_SpeedAtJump;

                    move = move * usedSpeed * Time.deltaTime;

                    move = transform.TransformDirection(move);
                    m_CharacterController.Move(move);
                }
                // Turn player
                float turnPlayer = Input.GetAxis("Mouse X") * MouseSensitivity;
                m_HorizontalAngle = m_HorizontalAngle + turnPlayer;

                if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
                if (m_HorizontalAngle < 0) m_HorizontalAngle += 360.0f;

                Vector3 currentAngles = transform.localEulerAngles;
                currentAngles.y = m_HorizontalAngle;
                transform.localEulerAngles = currentAngles;

                // Camera look up/down
                var turnCam = -Input.GetAxis("Mouse Y");
                turnCam = turnCam * MouseSensitivity;
                m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle, -89.0f, 89.0f);
                currentAngles = CameraPosition.transform.localEulerAngles;
                currentAngles.x = m_VerticalAngle;
                CameraPosition.transform.localEulerAngles = currentAngles;


                Speed = move.magnitude / (PlayerSpeed * Time.deltaTime);

            }

        }

        // Fall down / gravity
        m_VerticalSpeed = m_VerticalSpeed - 10.0f * Time.deltaTime;
        if (m_VerticalSpeed < -15.0f)
            m_VerticalSpeed = -15.0f; // max fall speed
        var verticalMove = new Vector3(0, m_VerticalSpeed * Time.deltaTime, 0);
        var flag = m_CharacterController.Move(verticalMove);
        if ((flag & CollisionFlags.Below) != 0)
            m_VerticalSpeed = 0;
    }

    IEnumerator PlaySecretKey()
    {
        yield return new WaitForSecondsRealtime(5f);
        testAnim = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            bb = true;
            GunScript.bb = true;
            HandScript.bb = true;
            otherEnemy = other;
            enemys++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Enemy") || (other.tag == "DeadEnemy"))
        {
            enemys--;
            bb = false;
            GunScript.bb = false;
            HandScript.bb = false;
            otherEnemy = null;

        }

        if (enemys == 0)
        {

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            bb = true;
            GunScript.bb = true;
            HandScript.bb = true;
            otherEnemy = other;

        }
    }

    public void BBB()
    {
        GunScript.bb1 = true;
        HandScript.bb1 = true;
        StartCoroutine(BB());
    }
    IEnumerator BB()
    {
        //0.95
        yield return new WaitForSeconds(0.136f);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 5f, Enemy))
        {
            Debug.Log(hit.transform);
            BodyPartScript bp = hit.transform.GetComponent<BodyPartScript>();
            //Debug.Log(other.transform.parent.parent);
            //Debug.Log(bp.enemy.lifes);
            bp.enemy.lifes--;
            bp.ChangeMaterial(bp.enemy.lifes);
            if (bp.enemy.lifes < 1)
            {
                Instantiate(SuperHotScript.instance.hitParticlePrefab, transform.position, transform.rotation);
                bp.HidePartAndReplace();
                bp.enemy.Ragdoll();
            }
        }
    }
    /*

    public void DisplayCursor(bool display)
    {
        m_IsPaused = display;
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
    }*/
}
