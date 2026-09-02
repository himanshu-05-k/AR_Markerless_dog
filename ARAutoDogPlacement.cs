using UnityEngine;

public class ARAutoDogPlacement : MonoBehaviour
{
    public GameObject dogPrefab;
    public Camera arCamera;

    public float distanceFromCamera = 2.0f;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 5.0f;

    private GameObject dog;

    void Start()
    {
        if (dogPrefab != null)
        {
            Vector3 startPosition =
                arCamera.transform.position +
                arCamera.transform.forward * distanceFromCamera;

            dog = Instantiate(
                dogPrefab,
                startPosition,
                Quaternion.identity
            );
        }
    }

    void Update()
    {
        if (dog == null || arCamera == null)
            return;

        Vector3 targetPosition =
            arCamera.transform.position +
            arCamera.transform.forward * distanceFromCamera;

        dog.transform.position = Vector3.Lerp(
            dog.transform.position,
            targetPosition,
            Time.deltaTime * moveSpeed
        );

        Vector3 direction =
            arCamera.transform.position - dog.transform.position;

        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            dog.transform.rotation = Quaternion.Slerp(
                dog.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }
}