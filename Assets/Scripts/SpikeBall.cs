using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        if (rb.velocity.y > 0)
        {
            rb.AddForce(new Vector2(0, 6f * Time.deltaTime), ForceMode2D.Impulse);
        }
    }
}
