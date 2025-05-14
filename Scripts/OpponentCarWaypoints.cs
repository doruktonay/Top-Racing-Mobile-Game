using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentCarWaypoints : MonoBehaviour
{
    [Header("Opponent Car")]
    public OpponentCar OpponentCar;
    public WayPoint currentWayPoint;

    private void Awake()
    {
        OpponentCar = GetComponent<OpponentCar>();
    }

    private void Start()
    {
        OpponentCar.LocateDestination(currentWayPoint.GetPosition());
    }

    private void Update()
    {
        if(OpponentCar.destinationReached)
        {
            currentWayPoint = currentWayPoint.nextWaypoint;
            OpponentCar.LocateDestination(currentWayPoint.GetPosition());
        }
    }
}
