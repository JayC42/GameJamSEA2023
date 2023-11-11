using UnityEngine;

public class AIFollower : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float targetPosition;
    [SerializeField]
    private Transform playerTransform;
    Rigidbody2D rb;
    Animator anim;
    Transform target; 
    void Start()
    {
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
    }
    void Update()
    {
        TargetFollow(); 
    }
    void TargetFollow()
    {
        if (Vector2.Distance(transform.position, target.position) > targetPosition)
        {
            //anim.SetBool("Running", true);
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            //anim.SetBool("Running", false);
        }
    }
}
