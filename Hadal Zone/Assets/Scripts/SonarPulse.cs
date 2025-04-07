using UnityEngine;

public class SonarPulse : MonoBehaviour
{
    public float pulseSpeed = 5f;
    public float maxScale = 10f;
    public float fadeDuration = 1f;

    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.localScale = Vector3.zero;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Scale the pulse
        float scale = Mathf.Lerp(0f, maxScale, timer / fadeDuration);
        transform.localScale = new Vector3(scale, scale, 1f);

        // Fade out
        Color c = sr.color;
        c.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
        sr.color = c;

        // Destroy after fading
        if (timer >= fadeDuration)
            Destroy(gameObject);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * 0.5f);
        foreach (var hit in hits)
        {
            SonarHighlight highlight = hit.GetComponent<SonarHighlight>();
            if (highlight != null)
            {
                highlight.Ping();
            }
        }
    }
}
