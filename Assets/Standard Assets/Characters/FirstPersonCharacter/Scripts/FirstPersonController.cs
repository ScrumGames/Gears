using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private bool m_IsMoving;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] private float m_CrouchSpeed;
        [SerializeField] private float m_ProneSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
        private bool playerStand;
        private bool playerCrouch;
        private bool playerProne;
        private bool isLean;
        private Vector3 originalPlayerLocalScale;
        private float playerHeight;
        private float auxPlayerHeightA;
        private float auxPlayerHeightB;
        private float t;
        private float t2;
        private float t3;
        private float mult;
        private float originalSpeed;
        private int auxLean;
        private float leanAuxA;
        private float leanAuxB;
        private float leanPosX;
        private float oldEulerAnglesZ;
        private bool backEulerAnglesZ;
        private ClimbingLadderController climbingController;
        private Vector3 climbingPosition;
        private bool climbingLadder;



        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
            playerStand = true;
            playerCrouch = false;
            playerProne = false;
            isLean = false;
            originalPlayerLocalScale = transform.localScale;
            playerHeight = 1.0f;
            auxPlayerHeightA = transform.localScale.y;
            auxPlayerHeightB = transform.localScale.y;
            t = 0.0f;
            t2 = 0.0f;
            t3 = 0.0f;
            leanAuxA = 0.0f;
            leanAuxB = 0.0f;
            leanPosX = 0.0f;
            oldEulerAnglesZ = 0.0f;
            backEulerAnglesZ = false;
            originalSpeed = m_WalkSpeed;
            auxLean = 0;
            climbingController = GetComponent<ClimbingLadderController>();
            climbingLadder = false;
        }


        // Update is called once per frame
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump && playerStand && !isLean)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0.0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0.0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
            m_IsMoving = (m_CharacterController.velocity.magnitude > 0);


            // o codigo abaixo implementa o crouch e prone
            if (Input.GetButtonDown("Crouch") && !isLean)
            {
                if (!playerCrouch)
                {
                    playerCrouch = true;
                    playerStand = false;
                    playerProne = false;
                    auxPlayerHeightA = playerHeight;
                    auxPlayerHeightB = originalPlayerLocalScale.y / 1.6f;
                    t = 0.0f;
                    mult = 4.0f;
                    m_WalkSpeed = m_CrouchSpeed;
                }
                else
                {
                    playerCrouch = false;
                    playerStand = true;
                    playerProne = false;
                    auxPlayerHeightA = playerHeight;
                    auxPlayerHeightB = originalPlayerLocalScale.y;
                    t = 0.0f;
                    mult = 4.0f;
                    m_WalkSpeed = originalSpeed;
                }
            }

            if (Input.GetButtonDown("Prone") && !isLean)
            {
                if (!playerProne)
                {
                    playerCrouch = false;
                    playerStand = false;
                    playerProne = true;
                    auxPlayerHeightA = playerHeight;
                    auxPlayerHeightB = originalPlayerLocalScale.y / 30;
                    t = 0.0f;
                    mult = 4.0f;
                    m_WalkSpeed = m_ProneSpeed;
                }
                else
                {
                    playerCrouch = false;
                    playerStand = true;
                    playerProne = false;
                    auxPlayerHeightA = playerHeight;
                    auxPlayerHeightB = originalPlayerLocalScale.y;
                    t = 0.0f;
                    mult = 4.0f;
                    m_WalkSpeed = originalSpeed;
                }
            }

            if(playerHeight != auxPlayerHeightB)
            {
                playerHeight = Mathf.Lerp(auxPlayerHeightA, auxPlayerHeightB, t);
                m_Camera.transform.localPosition = new Vector3(m_Camera.transform.localPosition.x, playerHeight, m_Camera.transform.localPosition.z);
                t += mult * Time.deltaTime;
            }


            //o codigo abaixo implementa o lean
            if (Input.GetButton("LeanRight"))
            {
                if ((playerStand || playerCrouch) && auxLean >= 0 && !m_IsMoving)
                {
                    if (!isLean)
                    {
                        leanAuxA = m_Camera.transform.localPosition.x;
                        m_OriginalCameraPosition = m_Camera.transform.localPosition;
                        leanAuxB = m_OriginalCameraPosition.x + 1.0f;
                        t2 = 0.0f;
                    }
                    isLean = true;
                    m_Camera.transform.eulerAngles = new Vector3(m_Camera.transform.eulerAngles.x, m_Camera.transform.eulerAngles.y, Mathf.Lerp(0.0f, -10.0f, t2));
                    oldEulerAnglesZ = m_Camera.transform.eulerAngles.z;
                    t2 += 8.0f * Time.deltaTime;
                    auxLean = 1;
                }
            }

            if (Input.GetButton("LeanLeft"))
            {
                if ((playerStand || playerCrouch) && auxLean <= 0 && !m_IsMoving)
                {
                    if (!isLean)
                    {
                        leanAuxA = m_Camera.transform.localPosition.x;
                        m_OriginalCameraPosition = m_Camera.transform.localPosition;
                        leanAuxB = m_OriginalCameraPosition.x + -1.0f;
                        t2 = 0.0f;
                    }
                    isLean = true;
                    m_Camera.transform.eulerAngles = new Vector3(m_Camera.transform.eulerAngles.x, m_Camera.transform.eulerAngles.y, Mathf.Lerp(0.0f, 10.0f, t2));
                    oldEulerAnglesZ = m_Camera.transform.eulerAngles.z;
                    t2 += 8.0f * Time.deltaTime;
                    auxLean = -1;
                }
            }

            if (Input.GetButtonUp("LeanLeft") || Input.GetButtonUp("LeanRight"))
            {
                if (!Input.GetButton("LeanLeft") && !Input.GetButton("LeanRight") && auxLean != 0.0f)
                {
                    isLean = false;
                    t2 = 0.0f;
                    leanAuxA = m_Camera.transform.localPosition.x;
                    leanAuxB = m_OriginalCameraPosition.x;
                    backEulerAnglesZ = true;
                }
            }

            if(backEulerAnglesZ && !isLean)
            {
                if(oldEulerAnglesZ > 300.0f)
                {
                    m_Camera.transform.eulerAngles = new Vector3(m_Camera.transform.eulerAngles.x, m_Camera.transform.eulerAngles.y, Mathf.Lerp(oldEulerAnglesZ, 360.0f, t2));
                }
                else
                {
                    m_Camera.transform.eulerAngles = new Vector3(m_Camera.transform.eulerAngles.x, m_Camera.transform.eulerAngles.y, Mathf.Lerp(oldEulerAnglesZ, 0.0f, t2));
                }

                t2 += 8.0f * Time.deltaTime;
                if(m_Camera.transform.eulerAngles.z == 0.0f)
                {
                    oldEulerAnglesZ = 0.0f;
                    backEulerAnglesZ = false;
                }
            }

            if (leanPosX != leanAuxB)
            {
                leanPosX = Mathf.Lerp(leanAuxA, leanAuxB, t3);
                m_Camera.transform.localPosition = new Vector3(leanPosX, m_Camera.transform.localPosition.y, m_Camera.transform.localPosition.z);
                t3 += 12.0f * Time.deltaTime;
            }
            else
            {
                t3 = 0.0f;
            }

            if(m_Camera.transform.localPosition.x == 0.0f)
            {
                auxLean = 0;
                isLean = false;
            }


            // o codigo abaixo move o jogador para a posicao de subir ou descer a escada
            if (climbingLadder)
            {
                float step = Time.deltaTime * 5.0f;
                transform.position = Vector3.MoveTowards(transform.position, climbingPosition, step);
                if (transform.position.Equals(climbingPosition))
                {
                    climbingLadder = false;
                    climbingController.isClimbing = true;
                    climbingController.pressedButton = false;
                }
            }



        }





        private void PlayLandingSound()
        {
            if (playerStand && (playerHeight == auxPlayerHeightB))
            {
                m_AudioSource.clip = m_LandSound;
                m_AudioSource.Play();
                m_NextStep = m_StepCycle + .5f;
            }
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded || !playerStand)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            //Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded && playerStand)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
               
            }
            //newCameraPosition = m_Camera.transform.localPosition;
            //newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            //m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            if(playerStand)
            {
                m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
            }
            
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;

            if (!isLean)
            {
                m_Input = new Vector2(horizontal, vertical);
            }

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        public void movePlayerToLadderDownstairs(Transform climbTransform)
        {
            climbingPosition = climbTransform.position;
            climbingLadder = true;
        }

        public void movePlayerToLadderUpstairs(Transform climbPosition)
        {

        }
    }
}
