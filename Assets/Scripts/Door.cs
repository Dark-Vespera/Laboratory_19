using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public float openHeight = 5f;
    public float openSpeed = 2f;

    private bool isOpen = false;

    public void TryOpen(int currentFuses, int neededFuses)
    {
        if (isOpen) return;

        if (currentFuses >= neededFuses)
        {
            UIManager.Instance.ShowNotification("Дверь открыта! Вы сбежали!");
            StartCoroutine(OpenDoorRoutine());
        }
        else
        {
            UIManager.Instance.ShowNotification($"Дверь заперта. Нужно ещё {neededFuses - currentFuses} предохранителей.");
        }
    }

    private IEnumerator OpenDoorRoutine()
    {
        isOpen = true;

        // 1. Отключаем коллайдер, чтобы дверь не тащила игрока
        Collider doorCollider = GetComponent<Collider>();
        if (doorCollider != null) doorCollider.enabled = false;

        // 2. Запоминаем стартовую позицию и считаем конечную
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, openHeight, 0);

        // 3. Плавно двигаем дверь вверх
        float timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime * openSpeed;
            transform.position = Vector3.Lerp(startPos, endPos, timer);
            yield return null; // Ждем следующий кадр
        }

        // На всякий случай ставим точную конечную позицию
        transform.position = endPos;
    }
}