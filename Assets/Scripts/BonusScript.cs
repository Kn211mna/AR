using UnityEngine;

public class BonusScript : MonoBehaviour
{
    public float speed = 0.2f;
    public float maxHeight = 3.0f;
    public GameObject smoke;

    private bool used = false;

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
        if (transform.position.y > maxHeight)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyBonus(Vector3? effectPosition = null, Vector3? effectNormal = null)
    {
        if (used) return;
        used = true;

        var uiManager = Object.FindFirstObjectByType<GameUIManager>();
        if (uiManager != null)
        {
            uiManager.AddLife();
        }
        if (smoke != null && effectPosition.HasValue && effectNormal.HasValue)
            Instantiate(smoke, effectPosition.Value, Quaternion.LookRotation(effectNormal.Value));

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (other.CompareTag("PlayerBullet") || other.CompareTag("Player"))
        {
            ApplyBonus();
        }
    }
}
