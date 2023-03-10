using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{

    //constants
    private const float CHASE_THRESH = 4; //threshold for chasing vs close patrolling
    private const float MAX_WAYPOINT_DIST = 6; //for generating the graph,  maximum dist between two waypoints
    private const float FAR_PATROL_THRESH = 7; //threshold for far patrolling vs close patrolling
    private const float AT_POINT_THRESH = 0.1f;
    private const float ENEMY_SPEED_CLOSE = 4;
    private const float ENEMY_SPEED_FAR = 6; //goes faster when not close to the player
    private const float ENEMY_SPEED_CHASE = 1.5f;
    private const float FAR_TELEPORT_THRESH = 7;
    private const float TELEPORT_TIMER = 10;
    private enum State
    {
        START,
        FAR_PATROL,
        CLOSE_PATROL,
        CHASING
    }
    
    //public variables
    public Transform[] waypoints;
    public LayerMask wallMask;

    //vars
    private float[,] distGraph;
    private State currentState = State.START;
    private int nextWaypoint;
    private Vector3 lastSeenPlayer;
    private float timeSincePlayerSeenOrTeleported = 0; //for teleporting
    private float lastTimePlayerSeenOrTeleported = 0; //for teleporting
    private AudioSource monsterAudio;
    [SerializeField] private AudioClip jumpScare;

    // Start is called before the first frame update
    void Start()
    {
        monsterAudio = GetComponent<AudioSource>();
        GenerateGraph();
    } //Start

    // Update is called once per frame
    void Update()
    {
        RecalculateState();
        CalculateAI();
    } //Update

    void GenerateGraph()
    {
        distGraph = new float[waypoints.Length, waypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
        {
            for(int j = 0; j < waypoints.Length; j++)
            {
                if(i != j && !Physics2D.Raycast(waypoints[i].position, (waypoints[j].position-waypoints[i].position), Vector3.Distance(waypoints[i].position, waypoints[j].position), wallMask))
                {
                    distGraph[i, j] = (waypoints[i].position - waypoints[j].position).magnitude;

                    if(distGraph[i, j] > MAX_WAYPOINT_DIST)
                        distGraph[i, j] = float.MaxValue;
                }
                else
                {
                    distGraph[i, j] = float.MaxValue;
                }
            }
        }
    } //GenerateGraph

    void CalculateAI() 
    {
        float distFromPlayer = (PlayerHandler._PlayerHandlerInstance.transform.position - transform.position).magnitude;

        if(currentState.Equals(State.FAR_PATROL)) //FAR PATROL
        {
            if (timeSincePlayerSeenOrTeleported >= TELEPORT_TIMER)
            {
                TeleportToRandomWaypointAwayFromPlayer();
                lastTimePlayerSeenOrTeleported = Time.time;
                //maybe need to the "timesinceplayerseenorteleported" calculation here, check this first if teleporting around randomly
                //its at the bottom of the update method
            }


            float distToNextWaypoint = (waypoints[nextWaypoint].position - transform.position).magnitude;

            if(distToNextWaypoint < AT_POINT_THRESH)
            {
                GenerateNextWaypoint(State.FAR_PATROL);
            }

            MoveToNextWaypoint(ENEMY_SPEED_FAR);
        }
        else if (currentState.Equals(State.CLOSE_PATROL)) //CLOSE PATROL
        {
            float distToNextWaypoint = (waypoints[nextWaypoint].position - transform.position).magnitude;

            if (distToNextWaypoint < AT_POINT_THRESH)
            {
                GenerateNextWaypoint(State.CLOSE_PATROL);
            }

            MoveToNextWaypoint(ENEMY_SPEED_CLOSE);
        }
        else if (currentState.Equals(State.CHASING)) //CHASING
        {
            if (GetDistFromPlayer() <= CHASE_THRESH)
            {
                lastSeenPlayer = PlayerHandler._PlayerHandlerInstance.transform.position;
                lastTimePlayerSeenOrTeleported = Time.time;
            }

            ChaseLastSeenPlayerLocation();

        }
        else
        {
            //shit broken if this happens
        }

        timeSincePlayerSeenOrTeleported = Time.time - lastTimePlayerSeenOrTeleported;
    } //CalculateAI

    void RecalculateState()
    {
        if (currentState.Equals(State.START))
        {
            currentState = State.FAR_PATROL;
            nextWaypoint = GetWaypointClosestToSelf();
        }
        else if (currentState.Equals(State.FAR_PATROL))
        {
            if(GetDistFromPlayer() < FAR_PATROL_THRESH)
            {
                currentState = State.CLOSE_PATROL;
            }
        }
        else if (currentState.Equals(State.CLOSE_PATROL))
        {
            if(GetDistFromPlayer() >= FAR_PATROL_THRESH)
            {
                currentState = State.FAR_PATROL;
            }
            else if(GetDistFromPlayer() < CHASE_THRESH && !Physics2D.Raycast(transform.position, (PlayerHandler._PlayerHandlerInstance.transform.position - transform.position), GetDistFromPlayer(), wallMask))
            {
                currentState = State.CHASING;
                monsterAudio.PlayOneShot(jumpScare);
            }
        }
        else if (currentState.Equals(State.CHASING))
        {
            if(GetDistFromPoint(lastSeenPlayer) < AT_POINT_THRESH)
            {
                //TODO: add teleport for here i think
                TeleportToRandomWaypointAwayFromPlayer();
                currentState = State.FAR_PATROL;
                //currentState = State.CLOSE_PATROL;
            }
        }
    }

    void GenerateNextWaypoint(State state)
    {
        if (state.Equals(State.FAR_PATROL))
        {
            //nextWaypoint = NextWaypointToPlayer(nextWaypoint); TODO: Implment pathfinding later if there's time, random pathing works for now

            nextWaypoint = GenerateRandomAdjacentWaypoint(nextWaypoint);
        }
        else if (state.Equals(State.CLOSE_PATROL))
        {
            nextWaypoint = GenerateRandomAdjacentWaypoint(nextWaypoint);
        }
    } //GenerateNextWaypoint

    void MoveToNextWaypoint(float speed)
    {
        Vector3 dir = waypoints[nextWaypoint].transform.position - this.transform.position;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;
    } //MoveToNextWaypoint

    int GenerateRandomAdjacentWaypoint(int currentWp)
    {
        List<int> candidates = new List<int>();

        for(int i = 0; i < waypoints.Length; i++)
        {
            if (distGraph[currentWp, i] < float.MaxValue)
            {
                //Debug.Log("added waypoint" + i + " to the candidates of where to go next!");
                candidates.Add(i);
            }
        }

        //if not in range of any points, just go back to current point I guess. would be a level flaw i think
        if(candidates.Count == 0)
        {
            return currentWp;
        }

        return candidates[Random.Range(0, candidates.Count)];

    } //GenerateRandomAdjacentWaypoint

    /*int NextWaypointToPlayer(int currentWp) 
    {
        int target = GetWaypointClosestToPlayer();

        List<int> path = new List<int>();

        float[] dists = new float[waypoints.Length];
        bool[] known = new bool[waypoints.Length];

        for(int i = 0; i < dists.Length; i++)
        {
            dists[i] = float.MaxValue;
        }

        for(int i = 0; i < known.Length; i++)
        {
            known[i] = false;
        }

        dists[currentWp] = 0;

        //dijsktras
        int u = currentWp;
        known[u] = true;
        for(int v = 0; v < waypoints.Length; v++)
        {
            if(distGraph[u, v] < float.MaxValue && !known[u])
            {
                known[u] = true;
            }
        }



    } //NextWaypointToPlayer
    */

    int GetWaypointClosestToPlayer() //will be used for pathfinding likely, don't need rn though.
    {
        return GetWaypointClosestToPoint(PlayerHandler._PlayerHandlerInstance.transform);
    } //GetWaypointClosestToPlayer

    int GetWaypointClosestToSelf()
    {
        return GetWaypointClosestToPoint(this.transform);
    } //GetWayPointClosestToSelf

    int GetWaypointClosestToPoint(Transform point)
    {
        int result = 0;
        float minDist = float.MaxValue;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float dist = (point.position - waypoints[i].position).magnitude;

            if (dist < minDist && !Physics2D.Raycast(point.position, waypoints[i].position, dist, wallMask))
            {
                minDist = dist;
                result = i;
            }
        }

        return result;
    } //GetWaypointClosestToPoint

    float GetDistFromPlayer()
    {
        return (this.transform.position - PlayerHandler._PlayerHandlerInstance.transform.position).magnitude;
    }

    float GetDistFromPoint(Transform point)
    {
        return (this.transform.position - point.position).magnitude;
    }

    float GetDistFromPoint(Vector3 point)
    {
        return (this.transform.position - point).magnitude;
    }

    //moves in the direction of the last known location of the player
    void ChaseLastSeenPlayerLocation()
    {
        Vector3 dir = lastSeenPlayer - this.transform.position;
        dir.Normalize();

        transform.position += dir * ENEMY_SPEED_CHASE * Time.deltaTime;
    }

    void TeleportToRandomWaypoint() // OBSOLETE
    {
        int wp = Random.Range(0, waypoints.Length);
        this.transform.position = waypoints[wp].position;
        nextWaypoint = wp;
    }

    void TeleportToRandomWaypointAwayFromPlayer()
    {
        float destinationDistFromPlayer = 0;
        int wp;
        int timeOut = 0;

        do
        {
            wp = Random.Range(0, waypoints.Length);
            destinationDistFromPlayer = Vector3.Distance(PlayerHandler._PlayerHandlerInstance.transform.position, waypoints[wp].position);
            timeOut++;
            if (timeOut > 300) //if we do too many things or its broken, don't go infinite
                break;

        } while (destinationDistFromPlayer < FAR_TELEPORT_THRESH);

        this.transform.position = waypoints[wp].position;
        nextWaypoint = wp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHandler._PlayerHandlerInstance.PlayerIsDead();
            TeleportToRandomWaypointAwayFromPlayer();
            currentState = State.FAR_PATROL;

        }
    }
}
