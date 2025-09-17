using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    private int currentTarget = 0;
    private Vector2[] pathPoints;
    private Animator animator;
    private AudioSource moveSFX;
    private bool canMove = false;
    [SerializeField] public AudioClip moveClip;  
    
    void Start()
    {
        animator = GetComponent<Animator>();
        moveSFX = GetComponent<AudioSource>();
        pathPoints = new Vector2[]
        {
            new Vector2(1f, -1f),
            new Vector2(6f, -1f),
            new Vector2(6f, -5f),
            new Vector2(1f, -5f)
        };
        transform.position = pathPoints[0];
        StartCoroutine(WaitStart(3f));
    }
    
    void Update()
    {
        if (!canMove) return;
        Vector2 target = pathPoints[currentTarget];
        Vector2 dir = (target - (Vector2)transform.position).normalized;
        transform.position += (Vector3)dir*moveSpeed*Time.deltaTime;
        
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0) animator.Play("player-right");
            else animator.Play("player-left");
        }
        else
        {
            if (dir.y > 0) animator.Play("player-up");
            else animator.Play("player-down");
        }
        
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            currentTarget = (currentTarget + 1) % pathPoints.Length;
        }
        
        if (dir.magnitude > 0.01f)
        {
            if (!moveSFX.isPlaying)
            {
                moveSFX.clip = moveClip;
                moveSFX.loop = true;
                moveSFX.Play();
            }
        }
        else
        {
            if (moveSFX.isPlaying)
            {
                moveSFX.Stop();
            }
        }
    }
    
    IEnumerator WaitStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        canMove = true;
    }
}
