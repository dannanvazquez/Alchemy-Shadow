using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
public class ClickAnywhereController : MonoBehaviour {
    private Canvas canvas;

    [Header("Settings")]
    [Tooltip("The amount of seconds waiting before enabling the UI.")]
    [SerializeField] private float uiInterval;

    private void Awake() {
        canvas = GetComponent<Canvas>();
    }

    public void AwaitInput(UnityEvent unityEvent) {
        StartCoroutine(AwaitInputCoroutine(unityEvent));
    }

    private IEnumerator AwaitInputCoroutine(UnityEvent unityEvent) {
        // Prevent accidental clicks
        yield return new WaitForSeconds(0.25f);

        bool isCanvasEnabled = false;
        float startTime = Time.time;

        // Wait until the input is true before continuing.
        while (!Input.GetKeyDown(KeyCode.Mouse0)) {
            if (!isCanvasEnabled && Time.time - startTime >= uiInterval) {
                canvas.enabled = true;
                isCanvasEnabled = true;
            }

            yield return null;
        }

        canvas.enabled = false;
        unityEvent?.Invoke();
    }
}
