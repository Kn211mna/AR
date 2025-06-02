using UnityEngine;

public class ShootScript : MonoBehaviour
{
    public GameObject arCamera;
    public GameObject smoke;

    void Start()
    {
        if (arCamera == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                arCamera = cam.gameObject;
        }
    }
    public void Shoot()
    {
        RaycastHit hit;
        if (arCamera == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                arCamera = cam.gameObject;
            else
                return; 
        }
        if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hit))
        {
            var balloon = hit.transform.GetComponent<BallonScript>();
            if (balloon != null)
            {
                balloon.OnPopped();
                Instantiate(smoke, hit.point, Quaternion.LookRotation(hit.normal));
                return;
            }
            var bonus = hit.transform.GetComponent<BonusScript>();
            if (bonus != null)
            {
                bonus.ApplyBonus(hit.point, hit.normal);
                return;
            }
        }
    }
}