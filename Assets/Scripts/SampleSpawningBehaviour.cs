
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

    // Update is called once per frame
    void Update()
    {

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
        foreach(GameObject s in samples)
        {
            s.AddComponent<Sample>();
            s.AddComponent<SpriteRenderer>();

            s.AddComponent<Rigidbody2D>();
            s.AddComponent<BoxCollider2D>();

            s.AddComponent<SpriteRenderer>();
            s.GetComponent<SpriteRenderer>().sprite = SampleSprite;


            Rigidbody2D body = s.GetComponent<Rigidbody2D>();

            body.gravityScale = 0;

            BoxCollider2D collider = s.GetComponent<BoxCollider2D>();

            collider.isTrigger = true;
        }
    }
}
