﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathlyEnemy : Triangle
{
    private GameObject player;
    private float timeFollow = 5.0f;
    private float timeWait = 0.8f;
    private float timeChangePos = 0.5f;

    private Box[] boxs;

    // Start is called before the first frame update
    void Start()
    {
        boxs = FindObjectsOfType<Box>();
        seek_force = Random.Range(0.1f, 0.5f);
        player = FindObjectOfType<Player>().gameObject;
        randomTarget();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        chasing();
        movement();
    }

    private void movement()
    {
        Vector2 steering = Vector2.zero;

        steering = steering + seek(target);
        steering = steering + avoidance(boxs);

        steering = Vector2.ClampMagnitude(steering, seek_force);

        velocity = Vector2.ClampMagnitude(velocity + steering, speed);

        lookAt(velocity);
        body.velocity = velocity;
    }


    private void chasing()
    {
        if(player == null)
        {
            return;
        }
        if (timeFollow > 0)
        {
            timeFollow -= Time.deltaTime;
            timeChangePos -= Time.deltaTime;
            if (timeChangePos < 0)
            {
                timeChangePos = Random.Range(0.1f, 0.5f);
                target = player.transform.position;
            }
            if (timeFollow <= 0 || Vector2.Distance(transform.position, target) < 1f)
            {
                timeFollow = 0;
                Vector2 direction = target - (Vector2)transform.position;
                target = direction.normalized * 20.0f;
            }
        }

        if (timeFollow <= 0)
        {

            timeWait -= Time.deltaTime;
            if (timeWait < 0)
            {
                timeWait = Random.Range(0.6f, 2.0f);
                timeFollow = 5.0f;
            }
        }
    }
}
