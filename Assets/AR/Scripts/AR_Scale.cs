using UnityEngine;

public class AR_Scale : MonoBehaviour
{
    public GameObject targetObject;
    public float minScale = 0.5f;
    public float maxScale = 2.0f;
    public float scaleSpeed = 0.01f;
    public float scrollSpeed = 0.5f;
    public float smoothTime = 10f;

    private float targetScale;
    private bool isZooming;

    public static AR_Scale Instance;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (targetObject == null) return;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseScroll(); // desktop testing
#endif
        HandleTouch();       // mobile
        SmoothScale();       // interpolation
    }

    private void HandleTouch()
    {
        if (Input.touchCount != 2) return;

        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        Vector2 pos1 = touch1.position;
        Vector2 pos2 = touch2.position;
        float currentTouchDist = Vector2.Distance(pos1, pos2);

        if (!isZooming)
        {
            isZooming = true;
            targetScale = targetObject.transform.localScale.x;
        }

        // Change scale based on distance delta per frame
        float prevTouchDist = Vector2.Distance(pos1 - touch1.deltaPosition, pos2 - touch2.deltaPosition);
        float delta = (currentTouchDist - prevTouchDist) * scaleSpeed;
        targetScale = Mathf.Clamp(targetScale + delta, minScale, maxScale);

        if (touch1.phase == TouchPhase.Ended || touch2.phase == TouchPhase.Ended)
            isZooming = false;
    }

    private void HandleMouseScroll()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            float currentScale = targetObject.transform.localScale.x;
            float delta = scrollDelta * scrollSpeed;
            targetScale = Mathf.Clamp(currentScale + delta, minScale, maxScale);
        }
    }

    private void SmoothScale()
    {
        float currentScale = targetObject.transform.localScale.x;
        float newScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime * smoothTime);
        targetObject.transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void AssignTargetObject(GameObject newTarget)
    {
        Instance.targetObject = newTarget;

        if (newTarget != null)
        {
            float currentScale = newTarget.transform.localScale.x;
            targetScale = currentScale;
            isZooming = false;
        }
    }
}
