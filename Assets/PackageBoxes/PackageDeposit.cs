using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageDeposit : MonoBehaviour
{
    private float rotSpeed = 20;
    private float floatSpeed = 2.5f;
    private float floatAmplitude = .1f;
    private float boostFactor = 20;

    private PackageCourier playerCourier;
    private GameplayManager manager;
    private Renderer rend;
    private bool open;

    public Material openForDelivery;
    public Material closedForDelivery;
    public float coolDownTime = 15;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        playerCourier = (PackageCourier)FindObjectOfType(typeof(PackageCourier));
        manager = (GameplayManager)FindObjectOfType(typeof(GameplayManager));
        StartCoroutine(AmbientMotion());
        rend.material = openForDelivery;
        open = true;
    }

    private IEnumerator AmbientMotion()
    {
        while (true)
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
            transform.Translate(Vector3.up * Mathf.Cos(Time.time * floatSpeed) * Time.fixedDeltaTime * floatAmplitude);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (open && other.gameObject.CompareTag("Package") && other.GetComponent<BoxControl>().IsDeliverable)
        {
            Destroy(other.gameObject);
            playerCourier.DeliveredPackage();
            manager.IncrementScore();
            StartCoroutine(TemporaryClosure());
        }
    }

    private IEnumerator TemporaryClosure()
    {
        rend.material = closedForDelivery;
        open = false;
        rotSpeed = rotSpeed * boostFactor;
        yield return new WaitForSeconds(coolDownTime);
        rotSpeed = rotSpeed / boostFactor;
        rend.material = openForDelivery;
        open = true;
    }
}
