using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.SceneManagement;
using Unity.Properties;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private int stepsInGrass;
    [SerializeField] private int minStepsToEncounter;
    [SerializeField] private int maxStepsToEncounter;

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private bool movingInGrass;
    private float stepTimer;
    private int stepsToEncounter;
    private PartyManager partyManager;

    private const string WALKING_PARAMETER = "isWalking";
    private const string BATTLE_SCENE = "BattleScene";
    private const float TIME_PER_STEP = 0.5f;

    float x, z; // z is second dimention for 2.5D, y infers height.


    private void Awake()
    {
        playerControls = new PlayerControls();
        CalcStepToNextEncounter();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        partyManager = GameObject.FindFirstObjectByType<PartyManager>();

        if (partyManager.GetPosition() != Vector3.zero)
        {
            transform.position = partyManager.GetPosition();
        }
    }

    void Update()
    {
        x = playerControls.Player.Move.ReadValue<Vector2>().x;
        z = playerControls.Player.Move.ReadValue<Vector2>().y;

        // Debug.Log(x + "," + z);

        movement = new Vector3(x, 0, z).normalized;

        anim.SetBool(WALKING_PARAMETER, movement != Vector3.zero); // Vector3.zero = Vector3(0, 0, 0)

        // flip the character
        if (x != 0 && x < 0)
        {
            playerSprite.flipX = true;
        }
        if (x != 0 && x > 0)
        {
            playerSprite.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
        movingInGrass = colliders.Length != 0 && movement != Vector3.zero;

        if (movingInGrass)
        {
            stepTimer += Time.fixedDeltaTime;
            if(stepTimer > TIME_PER_STEP)
            {
                stepsInGrass++;
                stepTimer = 0;

                if (stepsInGrass >= stepsToEncounter)
                {
                    partyManager.SetPosition(transform.position);
                    SceneManager.LoadScene(BATTLE_SCENE);
                }
            }
        }
    }

    private void CalcStepToNextEncounter() 
    {
        stepsToEncounter = Random.Range(minStepsToEncounter, maxStepsToEncounter);
    }

    public void SetOverworldVisuals(Animator animator, SpriteRenderer spriteRenderer)
    {
        anim = animator;
        playerSprite = spriteRenderer;
    }
}
