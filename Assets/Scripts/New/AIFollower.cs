using UnityEngine;

public class AIFollower : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float targetPosition;
    [SerializeField]
    private float heightOffset = 2f;
    Rigidbody2D rb;
    Animator anim;
    Transform target;

    public static bool introComplete; 
    void Start()
    {
        introComplete = false; 
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); 
    }
    void Update()
    {
        if (introComplete)
        {
            TargetFollow(); 
        }
    }
    void TargetFollow()
    {
        // Calculate the target position with a height offset
        Vector2 targetPositionWithHeight = new Vector2(target.position.x, target.position.y + heightOffset);

        if (Vector2.Distance(transform.position, targetPositionWithHeight) > targetPosition)
        {
            // Move towards the target position with height offset
            transform.position = Vector2.MoveTowards(transform.position, targetPositionWithHeight, speed * Time.deltaTime);
        }
    }
}
