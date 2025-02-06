using UnityEngine;
using DG.Tweening;

public class ScaleLoop : MonoBehaviour {

	float tweenTime = 1f;

	void Start() {
		StartScaling();
	}

	void StartScaling() {
		transform.DOScale(0.028f, tweenTime)
			.SetEase(Ease.InOutSine)
			.OnComplete(() => {
				transform.DOScale(0.02f, tweenTime)
					.SetEase(Ease.InOutSine)
					.OnComplete(StartScaling);
			});
	}
}
