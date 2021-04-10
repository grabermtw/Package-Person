using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    public float roamingRadius = 15;
    public float roamingTimerMax = 15f;
    public float roamingTimerMin = 3f;
    public float detectionRadius = 15f;
    public Transform packagePerson;
    public GameManager manager;
    public AudioClip[] voiceLines;

    
    private AudioSource audioSource;
    private Animator anim;
    private NavMeshAgent nav;
    private Transform target;
    private bool isRoaming;
    private bool isHunting;
    
    private void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        
        StartCoroutine(BehaviorControl());
        StartCoroutine(Speak());

        // Calculate the base offset adjustment using the capsule collider
        CapsuleCollider capsule = GetComponent<CapsuleCollider>();
        nav.baseOffset = - (capsule.center.y - capsule.height / 2) - 0.05f;
    }

    IEnumerator Speak()
    {
        if (voiceLines.Length != 0)
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(4, 15));
                audioSource.PlayOneShot(voiceLines[Random.Range(0, voiceLines.Length)]);
                yield return null;
            }
        }

    }
 
    // Update is called once per frame
    void Update ()
    {
        //timer += Time.deltaTime;
        
        // Calculate angular velocity
        Vector3 s = transform.InverseTransformDirection(nav.velocity).normalized;
        float turn = s.x;

        // let the animator know what's going on
        anim.SetFloat("Speed", nav.velocity.magnitude);
        anim.SetFloat("Turn", turn * 2);
        anim.SetLayerWeight(1, 1 - Mathf.Clamp(Vector3.Distance(packagePerson.position, transform.position) - 3.5f, 0, 1));
    }

    IEnumerator BehaviorControl()
    {
        while (true)
        {
            // roam until we're in range.
            StartCoroutine(Roaming());
            yield return new WaitUntil(() => Vector3.Distance(packagePerson.position, transform.position) < detectionRadius);
            // stop roaming
            isRoaming = false;
            // hunt until we're out of range
            StartCoroutine(Hunting());
            yield return new WaitUntil(() => Vector3.Distance(packagePerson.position, transform.position) > detectionRadius);
            isHunting = false;
        }
    }

    IEnumerator Roaming()
    {
        // don't start roaming if we're already roaming
        if (isRoaming)
            yield break;
        isRoaming = true;
        while (isRoaming)
        {
            // get a new destination to roam to
            Vector3 newPos = GetNewDestination(transform.position, roamingRadius, -1);
            nav.SetDestination(newPos);
            // roam toward that destination
            yield return new WaitForSeconds(Random.Range(roamingTimerMin, roamingTimerMax));
        }
    }

    IEnumerator Hunting()
    {
        if (isHunting)
            yield break;
        isHunting = true;
        while (isHunting)
        {
            nav.SetDestination(packagePerson.position);
            yield return null;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            manager.EndGame();
        }
    }


    public static Vector3 GetNewDestination(Vector3 origin, float radius, int layermask) {
        // Get a random new destination
        Vector3 newDirection = Random.insideUnitSphere * radius;

        newDirection += origin;
        
        NavMeshHit navHit;
 
        NavMesh.SamplePosition(newDirection, out navHit, radius, layermask);
    
        return navHit.position;
    }
}
