using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    [SerializeField] private Transform visual;

    private float radius;
    private float duration;
    private float timer;

    public void Initialize(float radius, float duration)
    {
        this.radius = radius;
        this.duration = duration;

        visual.localScale = new Vector3(radius * 2f, 1f, radius * 2f);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        float progress = timer / duration;

        float speed = Mathf.Lerp(4f, 18f, progress);

        float pulse = 1f + Mathf.Sin(Time.time * speed) * 0.05f;

        visual.localScale = new Vector3(
            radius * 2f * pulse,
            1f,
            radius * 2f * pulse
        );
    }
}