using UnityEngine;

public class GunSway : MonoBehaviour
{
    public float swayAmount = 0.02f;
    public float maxSwayAmount = 0.06f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the new position based on mouse input
        float newX = Mathf.Clamp(mouseX * swayAmount, -maxSwayAmount, maxSwayAmount);
        float newY = Mathf.Clamp(mouseY * swayAmount, -maxSwayAmount, maxSwayAmount);

        Vector3 targetPosition = initialPosition + new Vector3(newX, newY, 0);

        // Lerp to the new position for smoother sway
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 5f);
    }
}