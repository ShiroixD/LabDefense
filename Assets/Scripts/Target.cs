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

    public Transform playerTransform;
    public Transform towerTransform;
    public Transform currentTargetTransform;
    private NavMeshAgent agent;
    private bool isAttacking;

    void Start()
    {
        playerTransform = PlayerManager.instance.player.transform;
        towerTransform = GameObject.FindWithTag("Tower").transform;
        currentTargetTransform = towerTransform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent != null)
        {
            float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

            if (distanceToPlayer <= lookRadius)
                currentTargetTransform = playerTransform;
            else
                currentTargetTransform = towerTransform;

            float distanceToTarget = Vector3.Distance(currentTargetTransform.position, transform.position);
            agent.SetDestination(currentTargetTransform.position);
            FaceTarget();
            Debug.Log("Distance to player: " + distanceToPlayer);
            Debug.Log("Distance to target: " + distanceToTarget);
            if (distanceToTarget <= 2 * agent.stoppingDistance)
            {
                if (currentTargetTransform.CompareTag("Player"))
                {
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        StartCoroutine(AttackPlayer(attackDelay));
                    }
                }
                else if (currentTargetTransform.CompareTag("Tower"))
                {
                    if (!isAttacking)
                    {
                        isAttacking = true;
                        StartCoroutine(AttackTower(attackDelay));
                    }
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
        Vector3 direction = (currentTargetTransform.position - transform.position).normalized;
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


    IEnumerator AttackPlayer(float time)
    {
        Player player = playerTransform.GetComponent<Player>();
        while (isAttacking)
        {
            yield return new WaitForSeconds(time);
            player.TakeDamage((int)damage);
        }
    }

    IEnumerator AttackTower(float time)
    {
        Tower tower = towerTransform.GetComponent<Tower>();
        while (isAttacking)
        {
            yield return new WaitForSeconds(time);
            tower.TakeDamage((int)damage);
        }
    }
}
