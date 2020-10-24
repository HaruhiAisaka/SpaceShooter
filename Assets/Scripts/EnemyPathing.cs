using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    WaveConfig waveConfig;
    List<Transform> waypoints;
    float moveSpeed;
    int waypointIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetPathWavepoints();
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveThenDestroy();
    }

    public void SetWaveConfigs(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
        this.moveSpeed = waveConfig.GetMoveSpeed();
    }
    
    private void MoveThenDestroy(){
        if(waypointIndex < waypoints.Count){
            var targetPosition = waypoints[waypointIndex]
                .transform.position;
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);
            if (transform.position == targetPosition) waypointIndex ++;
        }
        else Destroy(gameObject);
    }

    
}
