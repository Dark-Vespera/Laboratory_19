using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAI : MonoBehaviour
{
    [Header("Игрок")]
    public Transform player;

    [Header("Патруль")]
    public Transform[] patrolPoints;

    [Header("Зрение")]
    public Transform eyes; // Точка, откуда монстр смотрит
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    public LayerMask visionMask;

    [Header("Скорость")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    private NavMeshAgent agent;
    private int currentPoint = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (eyes == null) {
            eyes = transform;
        }
        if (patrolPoints.Length > 0) {
            agent.speed = patrolSpeed;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void Update()
    {
        if (player == null) {
            return;
        }

        if (CanSeePlayer()) {
            agent.speed = chaseSpeed;
            agent.SetDestination(player.position); // Бежит за игроком
        }
        else {
            agent.speed = patrolSpeed; 
            Patrol(); // Патрулирует
        }
    }

    // Видит ли монстр игрока?
    bool CanSeePlayer()
    {
        Vector3 targetPoint = player.position + Vector3.up;
        Vector3 directionToPlayer = targetPoint - eyes.position;

        if (directionToPlayer.magnitude > viewDistance) {
            return false;
        }

        // Игрок перед монстром?
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle > viewAngle / 2f) {
            return false;
        }

        // Игрок за стеной?
        if (Physics.Raycast(
            eyes.position,
            directionToPlayer.normalized,
            out RaycastHit hit,
            viewDistance,
            visionMask)) {
            return hit.transform == player || hit.transform.IsChildOf(player);
        }
        return false;
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) {
            return;
        }

        // Переход к следующей точки маршрута
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance) {
            currentPoint++;
            if (currentPoint >= patrolPoints.Length) {
                currentPoint = 0;
            }
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }
}
