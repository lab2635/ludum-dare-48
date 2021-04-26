using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PolygonCollider2D))]
public class Spawner : MonoBehaviour
{
    private PolygonCollider2D spawnArea;
    public GameObject prefab;
    public int spawnCount;
    public float margin = 1f;
    
    void Start()
    {
        spawnArea = GetComponent<PolygonCollider2D>();

        for (var i = 0; i < spawnCount; i++)
        {
            if (!SpawnInArea())
            {
                Debug.Log("failed to spawn fish");
            }
        }
    }

    public bool Overlaps(Vector2 point)
    {
        return spawnArea.OverlapPoint(point);
    }

    public bool IntersectsWith(Vector2 dir, Vector2 origin, out Vector2 intersectionPoint)
    {
        var collision = Physics2D.Raycast(origin, dir, 100f);
        
        if (collision.collider == null)
        {
            intersectionPoint = Vector2.zero;
            return false;
        }

        intersectionPoint.x = collision.point.x;
        intersectionPoint.y = collision.point.y;
        return true;
    }
    
    private void OnDrawGizmos()
    {
        if (spawnArea == null) return;
        var bounds = spawnArea.bounds;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(bounds.center, bounds.extents * 2f);
    }

    bool IsValidSpawnPoint(Vector2 point)
    {
        if (!spawnArea.OverlapPoint(point)) return false;
        return Physics.CheckSphere(point, 0.5f);
    }

    bool SpawnInArea()
    {
        if (prefab == null) return false;

        var bounds = spawnArea.bounds;
        var attempt = 0;
        var maxAttempts = 1000;

        while (attempt < maxAttempts)
        {
            var point = Vector2.zero;
            point.x = Random.Range(bounds.min.x, bounds.max.x);
            point.y = Random.Range(bounds.min.y, bounds.max.y);
            
            if (IsValidSpawnPoint(point))
            {
                var position = new Vector3(point.x, point.y, 0f);
                var rotation = Quaternion.identity;
                var instance = Instantiate(prefab, position, rotation);
                var behavior = instance.GetComponent<BasicFishMovement>();
                SetSpawnArea(behavior);
                return true;
            }

            attempt++;
        }
        
        return false;
    }

    private void SetSpawnArea(BasicFishMovement behavior)
    {
        if (behavior == null) return;
        behavior.spawnArea = this;
    }
}
