using UnityEngine;
using System.Collections.Generic;

public class ThrowOBSAttackPattern : MonoBehaviour
{
    public Vector3 detectionSize = new Vector3(10f, 20f, 2f); // 長方体のサイズ
    private List<GameObject> throwableObjects = new List<GameObject>(); 
    private List<GameObject> currentThrowables = new List<GameObject>();

    void Start()
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = detectionSize;
        collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ThrowObj") && !throwableObjects.Contains(other.gameObject))
        {
            throwableObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (throwableObjects.Contains(other.gameObject))
        {
            throwableObjects.Remove(other.gameObject);
        }
    }

    public void Attack(Transform playerTransform)
    {
        StartCoroutine(PickUpAllThrowables());
        StartCoroutine(ThrowAllAfterDelay(1.5f, playerTransform));
    }

    private System.Collections.IEnumerator PickUpAllThrowables()
    {
        foreach (GameObject throwable in throwableObjects)
        {
            currentThrowables.Add(throwable);
            throwable.transform.SetParent(transform);
            throwable.transform.localPosition = new Vector3(0, 2, 1);

            // Rigidbodyを追加または取得
            Rigidbody rb;
            if (!throwable.TryGetComponent<Rigidbody>(out rb))
            {
                rb = throwable.AddComponent<Rigidbody>();
            }
            rb.isKinematic = true;

            yield return new WaitForSeconds(0.1f); // 各オブジェクトを持ち上げる間隔
        }
        throwableObjects.Clear();
    }

    private System.Collections.IEnumerator ThrowAllAfterDelay(float delay, Transform playerTransform)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject throwable in currentThrowables)
        {
            ThrowAtPlayer(throwable, playerTransform);
        }
        currentThrowables.Clear();
    }

    private void ThrowAtPlayer(GameObject throwable, Transform playerTransform)
    {
        if (throwable != null)
        {
            throwable.transform.SetParent(null);
            Rigidbody rb = throwable.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.AddForce(CalculateThrowForce(throwable.transform.position, playerTransform.position), ForceMode.Impulse);
        }
    }

    private Vector3 CalculateThrowForce(Vector3 start, Vector3 target)
    {
        Vector3 direction = target - start;
        direction.y = 0;
        float height = 2f;
        float gravity = Physics.gravity.y;

        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (target.y - start.y + height) / gravity);
        Vector3 velocity = new Vector3(direction.x / time, Mathf.Sqrt(-2 * gravity * height), direction.z / time);

        return velocity * 1.5f;
    }
}
