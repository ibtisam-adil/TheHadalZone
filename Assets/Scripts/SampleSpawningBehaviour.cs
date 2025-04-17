
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SampleSpawningBehaviour : MonoBehaviour
{
    private List<GameObject> samples;
    public int SampleCount;
    public Sprite SampleSprite;
    void Start()
    {
        samples = new List<GameObject>(GameObject.FindGameObjectsWithTag("Sample"));
        SampleSelection();
        SampleCreation();
    }

    private void SampleSelection()
    {
        for(int i = samples.Count ; i> SampleCount; i--)
        {
            int tempRandom = Random.Range(0, samples.Count);
            GameObject.Destroy(samples[tempRandom]);
            samples.Remove(samples[tempRandom]);
        }

    }

    private void SampleCreation()
    {
        foreach (GameObject s in samples)
        {
            s.AddComponent<Sample>();
            SpriteRenderer sr = s.AddComponent<SpriteRenderer>();
            sr.sprite = SampleSprite;

            Rigidbody2D body = s.AddComponent<Rigidbody2D>();
            body.gravityScale = 0;

            CircleCollider2D collider = s.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;

            s.tag = "Sample";

            if (SampleSprite != null)
            {
                float pixelsPerUnit = SampleSprite.pixelsPerUnit;
                float spriteWidth = SampleSprite.rect.width / pixelsPerUnit;
                float spriteHeight = SampleSprite.rect.height / pixelsPerUnit;

                float minDimension = Mathf.Min(spriteWidth, spriteHeight);
                collider.radius = minDimension / 2f * 0.9f;
            }
        }
    }

}
