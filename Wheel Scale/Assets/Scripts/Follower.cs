using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    private GameObject player;
    private Vector3 playerPos;
    private Vector3 distance;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        playerPos = player.transform.position;

        distance.z = playerPos.z - transform.position.z;
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - distance.z);
    }
}
