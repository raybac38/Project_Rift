using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerMainControler : NetworkBehaviour 
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float speed = 10f;
    void Start()
    {
        print("Joueur a spawner");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 m_Input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.MovePosition(m_Input * Time.deltaTime * speed + transform.position);
    }
}
