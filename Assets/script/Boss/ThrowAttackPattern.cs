using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThrowAttackPattern : MonoBehaviour
{
    [SerializeField] private float spreadRadius = 8f;
    [SerializeField] private float forwardOffset = 50f;
    [SerializeField] private float throwHeight = 5f;
    [SerializeField] private float throwDuration = 1f;
    [SerializeField] private int objectCount = 5;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackPrefab;

public void ThrowAttack(Transform target)
    {
        if (attackPrefab == null || target == null) return;

        Vector3 centerPoint = target.position + (target.forward * forwardOffset);

        for (int i = 0; i < objectCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            float radius = Random.Range(0f, spreadRadius);
            Vector3 offset = new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );
            Vector3 targetPosition = centerPoint + offset;

            StartCoroutine(ThrowProjectile(targetPosition));
        }
    }

    private IEnumerator ThrowProjectile(Vector3 targetPos)
    {


        GameObject projectile = Instantiate(attackPrefab, throwPoint.position, Quaternion.identity);
        Vector3 startPos = projectile.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < throwDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / throwDuration;

            float heightCurve = Mathf.Sin(normalizedTime * Mathf.PI);
            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, normalizedTime);
            currentPos.y += heightCurve * throwHeight;

            projectile.transform.position = currentPos;
            yield return null;
        }

        projectile.transform.position = targetPos;
    }
}
