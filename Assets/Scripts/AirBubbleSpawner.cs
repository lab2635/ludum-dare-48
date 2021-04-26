using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBubbleSpawner : MonoBehaviour
{
    public float minSpawnTime;
    public float maxSpawnTime;
    public GameObject spawnLocation;
    public GameObject airBubblePrefab;
    public float minBubbleSize;
    public float maxBubbleSize;
    public float minLifetime;
    public float maxLifetime;

    private float bubbleTimeElapsed;
    private float nextBubbleTime;

    // Start is called before the first frame update
    void Start()
    {
        bubbleTimeElapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (bubbleTimeElapsed > nextBubbleTime)
        {
            float size = Random.Range(minBubbleSize, maxBubbleSize);
            GameObject bubble = GameObject.Instantiate(airBubblePrefab, spawnLocation.transform.position, spawnLocation.transform.rotation);
            bubble.transform.localScale = new Vector3(size, size, size);

            float lifetime = Random.Range(minLifetime, maxLifetime);
            ObstacleCollider bubbleObstacle = bubble.GetComponent<ObstacleCollider>();
            bubbleObstacle.lifetime = lifetime;

            bubbleTimeElapsed = 0;
            nextBubbleTime = Random.Range(minSpawnTime, maxSpawnTime);
        }

        bubbleTimeElapsed += Time.deltaTime;
    }
}
