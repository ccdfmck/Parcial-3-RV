using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class monsterChase : MonoBehaviour
{
    public enum MonsterState { Patrol, Suspicious, Chase }
    public MonsterState currentState = MonsterState.Patrol;

    public NavMeshAgent monster;
    public Transform playTrans;
    public float stopDistance = 1.5f;

    // --- Patrulla ---
    public Transform[] waypoints;
    public float patrolSpeed = 3.5f;
    public float waypointTolerance = 1f;
    private int currentWaypointIndex = 0;

    // --- Detección (vista) ---
    public float viewDistance = 15f;
    [Range(0, 360)] public float viewAngle = 60f;
    public LayerMask obstacleMask; // capa de paredes/objetos que bloquean visión (NO incluir al jugador)

    // --- Sospecha ---
    public float suspiciousSpeed = 4.5f;
    public float suspiciousDuration = 4f;
    private float suspiciousTimer;
    private Vector3 lastKnownPlayerPos;

    // --- Persecución ---
    public float chaseSpeed = 6f;

    // Sonido de cercanía (ahora solo suena en estado Chase)
    public AudioSource closeSound;
    public float triggerDistance = 6f;

    // Growl / gruñido a distancia
    public AudioSource growlSound;
    public float growlMinInterval = 5f;
    public float growlMaxInterval = 6f;
    private float growlTimer;

    // Cámara
    public Transform playerCamera;
    public float shakeAmount = 0.05f;
    public float shakeSpeed = 20f;
    private Vector3 originalCameraPosition;

    void Start()
    {
        originalCameraPosition = playerCamera.localPosition;
        ResetGrowlTimer();
        monster.stoppingDistance = stopDistance;
    }

    void Update()
    {
        switch (currentState)
        {
            case MonsterState.Patrol:
                PatrolBehaviour();
                if (CanSeePlayer()) EnterChase();
                break;

            case MonsterState.Suspicious:
                SuspiciousBehaviour();
                if (CanSeePlayer()) EnterChase();
                break;

            case MonsterState.Chase:
                ChaseBehaviour();
                if (!CanSeePlayer()) EnterSuspicious();
                break;
        }

        HandleProximityEffects();
        HandleGrowl();
    }

    // ---------------- Comportamientos por estado ----------------

    void PatrolBehaviour()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        monster.speed = patrolSpeed;
        monster.SetDestination(waypoints[currentWaypointIndex].position);

        if (!monster.pathPending && monster.remainingDistance <= waypointTolerance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void SuspiciousBehaviour()
    {
        monster.speed = suspiciousSpeed;
        monster.SetDestination(lastKnownPlayerPos);

        if (!monster.pathPending && monster.remainingDistance <= waypointTolerance)
        {
            suspiciousTimer -= Time.deltaTime;
            if (suspiciousTimer <= 0f)
            {
                EnterPatrol();
            }
        }
    }

    void ChaseBehaviour()
    {
        monster.speed = chaseSpeed;
        monster.SetDestination(playTrans.position);
        lastKnownPlayerPos = playTrans.position;
    }

    // ---------------- Transiciones ----------------

    void EnterPatrol()
    {
        currentState = MonsterState.Patrol;
    }

    void EnterSuspicious()
    {
        currentState = MonsterState.Suspicious;
        suspiciousTimer = suspiciousDuration;
        // lastKnownPlayerPos ya quedó guardada en ChaseBehaviour antes de perder visión
        monster.SetDestination(lastKnownPlayerPos);
    }

    void EnterChase()
    {
        currentState = MonsterState.Chase;
    }

    // ---------------- Detección ----------------

    bool CanSeePlayer()
    {
        Vector3 dirToPlayer = playTrans.position - transform.position;
        float distToPlayer = dirToPlayer.magnitude;

        if (distToPlayer > viewDistance) return false;

        float angle = Vector3.Angle(transform.forward, dirToPlayer);
        if (angle > viewAngle / 2f) return false;

        // Si algo del obstacleMask bloquea el rayo antes de llegar al jugador, no lo ve
        if (Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, distToPlayer, obstacleMask))
        {
            return false;
        }

        return true;
    }

    // ---------------- Efectos (sonido cercanía + shake) ----------------

    void HandleProximityEffects()
    {
        float distance = Vector3.Distance(transform.position, playTrans.position);

        // Solo dan miedo de verdad cuando el monstruo va en Chase
        bool shouldTrigger = currentState == MonsterState.Chase && distance <= triggerDistance;

        if (shouldTrigger)
        {
            if (!closeSound.isPlaying) closeSound.Play();
            ShakeCamera();
        }
        else
        {
            if (closeSound.isPlaying) closeSound.Stop();
            playerCamera.localPosition = originalCameraPosition;
        }
    }

    void HandleGrowl()
    {
        growlTimer -= Time.deltaTime;
        if (growlTimer <= 0f)
        {
            if (growlSound != null && !growlSound.isPlaying)
            {
                growlSound.Play();
            }
            ResetGrowlTimer();
        }
    }

    void ResetGrowlTimer()
    {
        // Gruñe más seguido si está sospechando o persiguiendo -> más tensión
        if (currentState == MonsterState.Patrol)
            growlTimer = Random.Range(growlMinInterval, growlMaxInterval);
        else
            growlTimer = Random.Range(growlMinInterval * 0.4f, growlMaxInterval * 0.5f);
    }

    void ShakeCamera()
    {
        float x = Random.Range(-1f, 1f) * shakeAmount;
        float y = Random.Range(-1f, 1f) * shakeAmount;
        playerCamera.localPosition = originalCameraPosition + new Vector3(x, y, 0);
    }
}