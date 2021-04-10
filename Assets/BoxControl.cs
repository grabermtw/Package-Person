using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxControl : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // called when the box is picked up
    public void Pickup()
    {
        rb.isKinematic = true;
        // set it to the "IgnorePlayer" layer
        gameObject.layer = 6;
    }

    // called when the Package Person tosses the box
    public void Toss(Vector3 force)
    {
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
        // set it back top the Non-Carried Package layer
        gameObject.layer = 8;
    }
}
