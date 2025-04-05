using System.Collections;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    public float speed;
    public Transform[] patrolPoints;
    public float waitTime;
    int currentPointIndex;
    bool once;

    void Update()
    {
        // Move toward the current patrol point
        if (Vector2.Distance(transform.position, patrolPoints[currentPointIndex].position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPointIndex].position, speed * Time.deltaTime);
        }
        else
        {
            // Start waiting only once when arriving
            if (!once)
            {
                once = true;
                StartCoroutine(Wait());
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);

        // Move to next patrol point
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        once = false;
    }
}
