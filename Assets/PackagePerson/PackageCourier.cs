using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PackageCourier : MonoBehaviour
{
    public Transform carryParent;
    private Movement movement;
    public TextMeshProUGUI inGameText;
    public float carryingRunSpeed = 4.5f;
    public float runSpeed = 6f;
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

    public float GetRunSpeed()
    {
        if (carrying)
            return carryingRunSpeed;
        else
            return runSpeed;
    }

    // Called by a package deposit if you run into the deposit with the package.
    public void DeliveredPackage()
    {
        if (carrying)
        {
            carrying = false;
            anim.SetLayerWeight(1,0);
            package = null;
        }
        else
        {
            if (package == null)
            {
                inGameText.enabled = false;
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
