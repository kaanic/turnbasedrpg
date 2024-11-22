using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private int stepsInGrass;

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private bool movingInGrass;
    private float stepTimer;

    private const string WALKING_PARAM = "isWalking";
    private const float timePerStep = 0.5f;

    float x, z; // z is second dimention for 2.5D, y infers height.


    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        x = playerControls.Player.Move.ReadValue<Vector2>().x;
        z = playerControls.Player.Move.ReadValue<Vector2>().y;

        // Debug.Log(x + "," + z);

        movement = new Vector3(x, 0, z).normalized;

        anim.SetBool(WALKING_PARAM, movement != Vector3.zero); // Vector3.zero = Vector3(0, 0, 0)

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
            if(stepTimer > timePerStep)
            {
                stepsInGrass++;
                stepTimer = 0;
            }
        }
    }
}
