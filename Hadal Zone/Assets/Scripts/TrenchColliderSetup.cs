using UnityEngine;

public class TrenchColliderSetup : MonoBehaviour
{
    public GameObject[] trenchLineObjects;

    void Start()
    {
        foreach (GameObject line in trenchLineObjects)
        {
            // Get the SpriteRenderer to access the sprite's size and position
            SpriteRenderer spriteRenderer = line.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                // Check if the object already has a PolygonCollider2D, if not, add one
                PolygonCollider2D polygonCollider = line.GetComponent<PolygonCollider2D>();
                if (polygonCollider == null)
                {
                    polygonCollider = line.AddComponent<PolygonCollider2D>();
                }

                // Get the corners (vertices) of the sprite
                Vector3[] corners = new Vector3[4];
                corners[0] = spriteRenderer.bounds.min;  // Bottom-left
                corners[1] = new Vector3(spriteRenderer.bounds.min.x, spriteRenderer.bounds.max.y); // Top-left
                corners[2] = spriteRenderer.bounds.max;  // Top-right
                corners[3] = new Vector3(spriteRenderer.bounds.max.x, spriteRenderer.bounds.min.y); // Bottom-right

                // Rotate the corners based on the object's rotation
                for (int i = 0; i < corners.Length; i++)
                {
                    corners[i] = line.transform.TransformPoint(corners[i]); // Apply the object’s rotation/position
                }

                // Create a collider points array based on the corners
                Vector2[] colliderPoints = new Vector2[4];
                for (int i = 0; i < corners.Length; i++)
                {
                    colliderPoints[i] = new Vector2(corners[i].x, corners[i].y);
                }

                // Set the collider's points
                polygonCollider.SetPath(0, colliderPoints);
            }
        }
    }
}
