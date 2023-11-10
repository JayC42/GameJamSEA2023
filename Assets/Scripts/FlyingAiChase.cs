using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingAiChase : MonoBehaviour
{
    public GameObject player;
    private float speed = 3f;

    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;

        distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < 14)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }

        if (player.transform.position.x > transform.position.x)
        {
            scale.x = Mathf.Abs(scale.x);
        }
        else
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }

        transform.localScale = scale;
    }
}
