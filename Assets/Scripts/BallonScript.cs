using UnityEngine;

public class BallonScript : MonoBehaviour
{
    public enum BalloonType { Virus, Healthy }
    public BalloonType balloonType;
    public float maxHeight = 3.0f;

    private int virusHealth = 1;

    void Start()
    {
        if (balloonType == BalloonType.Virus && UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 3)
        {
            virusHealth = 2;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * 0.2f);

        if (transform.position.y > maxHeight)
        {
            if (balloonType == BalloonType.Virus)
            {
                var uiManager = Object.FindFirstObjectByType<GameUIManager>();
                if (uiManager != null)
                {
                    uiManager.DecreaseLife();
                }
            }
            Destroy(gameObject);
        }
    }
    public void OnPopped()
    {
        if (balloonType == BalloonType.Virus && virusHealth > 1)
        {
            virusHealth--;
            return;
        }

        var uiManager = Object.FindFirstObjectByType<GameUIManager>();
        if (balloonType == BalloonType.Healthy && uiManager != null)
        {
            uiManager.ShowLosePanel();
        }
        Destroy(gameObject);
    }
}