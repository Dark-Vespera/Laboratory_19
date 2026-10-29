using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAI : MonoBehaviour {
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
    bool CanSeePlayer() {

    float distance = Vector3.Distance(transform.position, player.position);
    if (distance > viewDistance) {
        return false; // Слишком далеко
    }

    Vector3 eyesPos = eyes != null ? eyes.position : transform.position + Vector3.up * 1.5f;
    Vector3 targetPos = player.position + Vector3.up * 1.5f;
    Vector3 dir = (targetPos - eyesPos).normalized;

    if (Physics.Raycast(eyesPos, dir, out RaycastHit hit, distance, visionMask)) {
        if (hit.transform == player || hit.transform.IsChildOf(player))
        {
            return true;
        }
        else
        {
            return false; 
        }
    }
    return true;
    }

    void Patrol() {
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

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {

        Debug.Log("Игрок умер");

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}