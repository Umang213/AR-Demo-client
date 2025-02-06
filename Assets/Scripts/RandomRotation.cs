using UnityEngine;
using DG.Tweening;

public class RandomRotation : MonoBehaviour
{
    public float duration = 1f; // Time to complete one rotation
    public Vector3 rotationAxis = Vector3.up; // Rotation axis (Y-axis by default)

    void Start ()
    {
        RotateRandomly ();
    }

    void RotateRandomly ()
    {
        Vector3 randomRotation = new Vector3 (
            Random.Range (0f, 360f),
            Random.Range (0f, 360f),
            Random.Range (0f, 360f)
        );

        transform.DORotate (randomRotation, duration, RotateMode.FastBeyond360)
            .SetEase (Ease.Linear)
            .OnComplete (RotateRandomly); // Loop rotation
    }
}
