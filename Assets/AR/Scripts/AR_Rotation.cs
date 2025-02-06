using UnityEngine;
using DG.Tweening;

public class AR_Rotation : MonoBehaviour
{
    public GameObject targetObject;
    public float rotationSpeedX, rotationSpeedY, minRotationX, maxRotationX, minRotationY, maxRotationY, rotationDuration, xSwipeThreshold, ySwipeThreshold;
    public bool enableXRotation, enableYRotation, clampXRotation, clampYRotation, reverseXRotation, reverseYRotation;
    public Ease rotationEase;

    public static AR_Rotation Instance;

    private void Awake() => Instance = this;

    private void Update()
    {

        //if (DetectClickOnUI.IsPointerOverUIElement()) return;

		if (targetObject == null || Input.touchCount != 1 || Input.GetTouch(0).phase != TouchPhase.Moved) return;

        Vector2 delta = Input.GetTouch(0).deltaPosition;
        if (Mathf.Abs(delta.x) < xSwipeThreshold && Mathf.Abs(delta.y) < ySwipeThreshold) return;

        Vector3 rotation = targetObject.transform.eulerAngles;

        if (enableXRotation && Mathf.Abs(delta.y) >= ySwipeThreshold)
        {
            rotation.x += (reverseXRotation ? delta.y : -delta.y) * rotationSpeedX;
            if (clampXRotation) rotation.x = Mathf.Clamp(rotation.x, minRotationX, maxRotationX);
        }

        if (enableYRotation && Mathf.Abs(delta.x) >= xSwipeThreshold)
        {
            rotation.y += (reverseYRotation ? -delta.x : delta.x) * rotationSpeedY;
            if (clampYRotation) rotation.y = Mathf.Clamp(rotation.y, minRotationY, maxRotationY);
        }

        targetObject.transform.DORotate(rotation, rotationDuration).SetEase(rotationEase);
    }

    public void AssignTargetObject(GameObject newTarget) => Instance.targetObject = newTarget;
}
