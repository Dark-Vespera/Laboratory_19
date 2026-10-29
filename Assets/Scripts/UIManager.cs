using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI objectiveText;
    public TextMeshProUGUI notificationText;

    private Coroutine notificationCoroutine;

    void Awake() {
        if (Instance == null) Instance = this;
    }

    void Start() {
        // Начальная задача
        UpdateObjective("Найти 3 предохранителя");
        notificationText.alpha = 0;
    }

    public void UpdateObjective(string newText) {
        objectiveText.text = "ЗАДАЧА: " + newText;
    }

    public void ShowNotification(string message) {
        if (notificationCoroutine != null) StopCoroutine(notificationCoroutine);
        
        notificationCoroutine = StartCoroutine(NotificationRoutine(message));
    }

    private IEnumerator NotificationRoutine(string message) {
        notificationText.text = message;
        notificationText.alpha = 1;

        yield return new WaitForSeconds(3f);

        float timer = 0;
        while (timer < 1f) {
            timer += Time.deltaTime;
            notificationText.alpha = Mathf.Lerp(1, 0, timer);
            yield return null;
        }
        notificationText.alpha = 0;
    }
}