using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float camSensitivity = 4.0f;
    public Transform player;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start() {
        offset = new Vector3(player.position.x, player.position.y + 8.0f, player.position.z + 7.0f);
    }

    // Update is called once per frame
    void LateUpdate(){
        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * camSensitivity, Vector3.up) * offset;
        transform.position = player.position + offset;
        transform.LookAt(player.position);
    }
}
