using System.Collections;
using UnityEngine;

public class SonarHighlight : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine glowRoutine;

    public Color glowColor = Color.cyan;
    public float glowDuration = 1f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void Ping()
    {
        if (glowRoutine != null)
            StopCoroutine(glowRoutine);

        glowRoutine = StartCoroutine(GlowEffect());
    }

    IEnumerator GlowEffect()
    {
        sr.color = glowColor;

        float elapsed = 0f;
        while (elapsed < glowDuration)
        {
            float t = elapsed / glowDuration;
            sr.color = Color.Lerp(glowColor, originalColor, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        sr.color = originalColor;
    }
}
