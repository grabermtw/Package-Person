using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PackageCourier : MonoBehaviour
{
    public Transform carryParent;
    private Movement movement;
    public TextMeshProUGUI inGameText;
    private Rigidbody rb;
    private bool carrying = false;
    private Animator anim;
    private BoxControl package;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        inGameText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action()
    {
        if (package != null)
        {
            if (!carrying) // pickup
            {
                package.Pickup();
                anim.SetLayerWeight(1,1);
                package.transform.SetParent(carryParent);
                package.transform.localPosition = new Vector3(0, 0, 0);
                //package.transform.localRotation = Quaternion.Euler(0, 0, 0);
                carrying = true;
                inGameText.enabled = false;
            }
            else // drop kick
            {
                if (movement.Grounded)
                    anim.SetTrigger("Kick");
                package.transform.SetParent(null);
                package.Toss(rb.velocity + carryParent.forward * 2.5f);
                anim.SetLayerWeight(1,0);
                carrying = false;
                package = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BoxControl newBoxControl;
        try {
            newBoxControl = other.transform.parent.parent.GetComponent<BoxControl>();
        } catch 
        {
            newBoxControl = other.GetComponent<BoxControl>();
        }
        if (newBoxControl != null && !carrying)
        {
            inGameText.enabled = true;
            inGameText.text = "Press E to Pick Up!";
            package = newBoxControl;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BoxControl newBoxControl;
        try {
            newBoxControl = other.transform.parent.parent.GetComponent<BoxControl>();
        } catch 
        {
            newBoxControl = other.GetComponent<BoxControl>();
        }
        if (newBoxControl != null)
        {
            inGameText.enabled = false;
            package = null;
        }
    }
}
