using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMask : MonoBehaviour
{
    [Range(0.05f, 2.5f)]
    public float flickTime;

    [Range(0.02f, 0.09f)]
    public float addSize;

    [Range(1f, 5f)]
    public float moveSpeed;

    private float timer = 0;
    private bool bigger = true;

    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("NPC").GetComponent<Transform>();
    }

    void Update()
    {
        if (timer > flickTime)
        {
            float newSize = bigger ? (transform.localScale.x + addSize) : (transform.localScale.x - addSize);
            transform.localScale = new Vector3(newSize, newSize, transform.localScale.z);
            timer = 0;
            bigger = !bigger;
        }

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, 20 * Time.deltaTime);

        timer += Time.deltaTime;
    }
}