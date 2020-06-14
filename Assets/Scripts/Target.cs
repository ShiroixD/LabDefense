using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public float damage = 10f;
    public float attackDelay = 0.5f;
    public float dieDelay = 3f;
    public Destructible destructible;
    public float lookRadius = 10f;

    private Transform target;
    private NavMeshAgent agent;
    private bool isAttacking;

    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent != null)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);
            }

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
                if (!isAttacking)
                {
                    isAttacking = true;
                    StartCoroutine(Attack(attackDelay));
                }
            }
            else
            {
                isAttacking = false;
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            isAttacking = false;
            if (destructible != null)
            {
                destructible.Destroy();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


    IEnumerator Attack(float time)
    {
        Player player = target.GetComponent<Player>();
        while (isAttacking)
        {
            yield return new WaitForSeconds(time);
            player.TakeDamage((int)damage);
        }
    }
}
