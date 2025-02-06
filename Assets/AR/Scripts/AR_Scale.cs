using UnityEngine;
using DG.Tweening;

public class AR_Scale : MonoBehaviour
{
    public GameObject targetObject;
    public float minScale, maxScale, scaleSpeed;
    public float tweenDuration;
    public Ease tweenEase;

    private float startTouchDist, targetScale;
    private bool isZooming;

    public static AR_Scale Instance;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (targetObject == null || Input.touchCount != 2) return;

        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
        {
            StartZoom();
        }

        if (isZooming && (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved))
        {
            ScaleObject();
        }

        if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
        {
            EndZoom();
        }
    }

    private void StartZoom()
    {
        Vector2 touch1 = Input.GetTouch(0).position;
        Vector2 touch2 = Input.GetTouch(1).position;
        startTouchDist = Vector2.Distance(touch1, touch2);
        targetScale = targetObject.transform.localScale.x;
        isZooming = true;
    }

    private void ScaleObject()
    {
        Vector2 touch1 = Input.GetTouch(0).position;
        Vector2 touch2 = Input.GetTouch(1).position;
        float currentTouchDist = Vector2.Distance(touch1, touch2);
        float scaleAmount = (currentTouchDist - startTouchDist) * scaleSpeed * Time.deltaTime;
        float newScale = Mathf.Clamp(targetScale + scaleAmount, minScale, maxScale);

        targetObject.transform.DOScale(newScale, tweenDuration).SetEase(tweenEase);
    }

    private void EndZoom() => isZooming = false;

    public void AssignTargetObject(GameObject newTarget) => Instance.targetObject = newTarget;
}
