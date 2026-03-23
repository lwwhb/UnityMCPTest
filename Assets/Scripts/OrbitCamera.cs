using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;
    public float height = 20f;
    public float radius = 30f;
    public float speed = 30f;

    private float angle = 0f;

    void Update()
    {
        angle += speed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 targetPos = target != null ? target.position : Vector3.zero;
        transform.position = targetPos + new Vector3(Mathf.Sin(rad) * radius, height, Mathf.Cos(rad) * radius);
        transform.LookAt(targetPos);
    }
}
