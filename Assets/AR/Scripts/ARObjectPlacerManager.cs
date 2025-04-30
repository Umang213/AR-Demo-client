using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectPlacerManager : MonoBehaviour
{
	[Header("AR Components")]
	[SerializeField] private ARRaycastManager arRaycastManager;
	[SerializeField] private Camera arCamera;

	[Header("Components")]
	private GameObject placementIndicator;
	[SerializeField] private GameObject arInstructionPanel;
	[SerializeField] private Button placeButton;
	[SerializeField] private Button restartButton;
	[SerializeField] private Button quitButton;

	[Header("Prefabs")]
	[SerializeField] private GameObject instantiatedObject;
	public GameObject indicatorPrefab;

	internal Pose placementPose;
	internal bool isPlacementPoseValid = false;
	internal bool isObjectPlaced = false;

	public GameObject Prefab;





	private void Start()
	{
		arCamera = Camera.main;
		arRaycastManager ??= FindFirstObjectByType<ARRaycastManager>();

		var prefab = indicatorPrefab;
		placementIndicator = Instantiate(prefab);

		SetupButtonListeners();
		InitializeUI();
	}

	private void SetupButtonListeners()
	{
		placeButton.onClick.AddListener(OnPlaceButtonClicked);
		restartButton.onClick.AddListener(OnRestartButtonClicked);
		quitButton.onClick.AddListener(OnQuitGame);
	}

	private void Update()
	{
		if (!isObjectPlaced)
		{
			HandlePlacement();
		}
	}

	private void InitializeUI()
	{
		arInstructionPanel.SetActive(true);
		placementIndicator.SetActive(false);
		placeButton.gameObject.SetActive(false);
		restartButton.gameObject.SetActive(false);
	}

	private void OnPlaceButtonClicked()
	{
		if (instantiatedObject == null)
		{

			instantiatedObject = Instantiate(Prefab, placementPose.position, placementPose.rotation);
			AR_Rotation.Instance.AssignTargetObject(instantiatedObject);
			AR_Scale.Instance.AssignTargetObject(instantiatedObject);
		}
		else
		{
			instantiatedObject.transform.position = placementPose.position;
		}
		// instantiatedObject.transform.localScale = GetAdjustedScale();
		AlignObjectWithCamera(instantiatedObject);
		if (instantiatedObject != null)
		{
			AssignPrefabs(instantiatedObject);


		}

		placementIndicator.SetActive(false);
		placeButton.gameObject.SetActive(false);
		restartButton.gameObject.SetActive(true);
		isObjectPlaced = true;


	}

	private void AlignObjectWithCamera(GameObject obj)
	{
		Vector3 cameraPosition = arCamera.transform.position;
		Vector3 directionToCamera = new Vector3(cameraPosition.x, obj.transform.position.y, cameraPosition.z) - obj.transform.position;
		Quaternion targetRotation = Quaternion.LookRotation(directionToCamera, Vector3.up);
		Vector3 eulerRotation = targetRotation.eulerAngles + new Vector3(0, 180, 0);
		eulerRotation.x = 0;
		eulerRotation.z = 0;
		obj.transform.rotation = Quaternion.Euler(eulerRotation);
	}



	private void OnRestartButtonClicked()
	{

		arInstructionPanel.SetActive(true);
		placementIndicator.SetActive(false);
		placeButton.gameObject.SetActive(false);
		restartButton.gameObject.SetActive(false);

		if (instantiatedObject != null)
		{
			Destroy(instantiatedObject);
			instantiatedObject = null;
		}
		isObjectPlaced = false;
		isPlacementPoseValid = false;
		placementPose = new Pose();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnQuitGame()
	{
		Application.Quit();
	}

	public void AssignPrefabs(GameObject obj)
	{
		// AR_Scale.Instance.AssignTargetObject(obj);
		// RotateTargetObject.Me.AssignTargetObject(obj.transform);
	}



	private void HandlePlacement()
	{
		UpdatePlacementPose();
		UpdatePlacementIndicator();

		if (isPlacementPoseValid)
		{
			placeButton.gameObject.SetActive(true);
		}
		else
		{
			placeButton.gameObject.SetActive(false);
		}
	}

	private void UpdatePlacementPose()
	{
		var screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
		var hits = new List<ARRaycastHit>();
		_ = arRaycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon);

		isPlacementPoseValid = hits.Count > 0;

		if (isPlacementPoseValid)
		{
			placementPose = hits[0].pose;
			arInstructionPanel.SetActive(false);
			placementIndicator.SetActive(true);
		}
		else
		{
			arInstructionPanel.SetActive(true);
			placementIndicator.SetActive(false);
		}
	}

	private void UpdatePlacementIndicator()
	{
		if (isPlacementPoseValid)
		{
			placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
		}
	}
	private float zoomConst;
	private float DetectDist = 0.9f;
	private Vector3 GetAdjustedScale()
	{
		Vector3 v = Vector3.one;
		zoomConst = Vector3.Distance(arCamera.transform.position, placementIndicator.transform.position) * 0.075f / DetectDist;
		v.x = v.y = v.z = zoomConst;
		return v;
	}

}
