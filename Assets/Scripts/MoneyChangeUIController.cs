using System.Collections;
using TMPro;
using UnityEngine;

public class MoneyChangeUIController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Color moneyUpColor;
    [SerializeField] private Color moneyDownColor;

    [Header("Settings")]
    [Tooltip("The amount of seconds this UI will last for.")]
    [SerializeField] private float lifespanSeconds;
    [Tooltip("The target end postion offset from the beginning position.")]
    [SerializeField] private Vector3 targetOffset;

    private IEnumerator AnimateCoroutine() {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + targetOffset;

        Color startColor = moneyText.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        float elapsedTime = 0;
        while (elapsedTime < lifespanSeconds) {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / lifespanSeconds));
            moneyText.color = Color.Lerp(startColor, endColor, (elapsedTime / lifespanSeconds));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    public void MoneyUp(int moneyAmount) {
        moneyText.text = $"+${moneyAmount}";
        moneyText.color = moneyUpColor;

        StartCoroutine(AnimateCoroutine());
    }

    public void MoneyDown(int moneyAmount) {
        moneyText.text = $"-${Mathf.Abs(moneyAmount)}";
        moneyText.color = moneyDownColor;

        StartCoroutine(AnimateCoroutine());
    }
}
