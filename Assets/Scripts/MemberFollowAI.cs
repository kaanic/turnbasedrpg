using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberFollowAI : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private int speed;

    private float followDistance;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    private const string WALKING_PARAMETER = "IsWalking";

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        followTarget = GameObject.FindFirstObjectByType<PlayerController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, followTarget.position) > followDistance)
        {
            anim.SetBool(WALKING_PARAMETER, true);

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, followTarget.position, step);

            if (followTarget.position.x - transform.position.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            anim.SetBool(WALKING_PARAMETER, false);
        }
    }

    public void SetFollowDistance(float newFollowDistance)
    {
        followDistance = newFollowDistance;
    }
}
