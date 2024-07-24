using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class TimerController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Image timerImage;

    private Canvas canvas;

    private bool isCountingDown = false;

    private void Awake() {
        canvas = GetComponent<Canvas>();
    }

    public void StartTimer(UnityAction countdownAction, float seconds) {
        StartCoroutine(StartTimerCoroutine(countdownAction, seconds));
    }

    private IEnumerator StartTimerCoroutine(UnityAction countdownAction, float seconds) {
        timerImage.fillAmount = 1;
        canvas.enabled = true;
        isCountingDown = true;

        float startTime = Time.time;

        // Wait until the input is true before continuing.
        while (Time.time - startTime < seconds) {
            timerImage.fillAmount = (seconds - (Time.time - startTime)) / seconds;

            if (isCountingDown == false) {
                canvas.enabled = false;

                yield break;
            }

            yield return null;
        }

        canvas.enabled = false;
        countdownAction?.Invoke();
    }

    public void DisableCountDown() {
        isCountingDown = false;
    }
}
