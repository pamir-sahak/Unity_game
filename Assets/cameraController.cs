using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    // variable to get playar's tranform
    public Transform player;
    
    private void Update()
    {
        // making camera follow the player    
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}
