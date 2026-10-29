using UnityEngine;
using TMPro; // Добавили для работы с текстом

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    
    // Подсказка на экране
    public GameObject interactPrompt; 

    // Счетчик
    public static int fusesCollected = 0;
    public int totalFusesNeeded = 3;

    void Start() {
        fusesCollected = 0; // Сброс счетчика собранных предметов
        if (interactPrompt != null) interactPrompt.SetActive(false); // Прячем подсказку
    }

    void Update()
    {
        CheckForInteractable(); // Проверяем, смотрим ли мы на что-то

        if (Input.GetKeyDown(KeyCode.E)) {
            TryInteract();
        }
    }

    void CheckForInteractable()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            // Если смотрим на предохранитель
            if (hit.collider.CompareTag("Fuse")) {
                interactPrompt.SetActive(true);
                interactPrompt.GetComponent<TextMeshProUGUI>().text = "Нажмите E, чтобы взять";
                return;
            }
            // Если смотрим на дверь
            else if (hit.collider.TryGetComponent(out Door door)) {
                interactPrompt.SetActive(true);
                interactPrompt.GetComponent<TextMeshProUGUI>().text = "Нажмите E, чтобы открыть";
                return;
            }
        }
        // Если ни на что не смотрим
        interactPrompt.SetActive(false);
    }

    void TryInteract()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance)) {
            // Если смотрим на предохранитель
            if (hit.collider.CompareTag("Fuse")) {
                fusesCollected++;
                Destroy(hit.collider.gameObject);
                
                // Вызов уведомления
                UIManager.Instance.ShowNotification($"Найден предохранитель ({fusesCollected}/{totalFusesNeeded})");
                
                // Обновление задачи
                if (fusesCollected >= totalFusesNeeded) {
                    UIManager.Instance.UpdateObjective("Найдите главную дверь и откройте ее");
                }
                else {
                    UIManager.Instance.UpdateObjective($"Найти предохранители ({fusesCollected}/{totalFusesNeeded})");
                }
            }
            else if (hit.collider.TryGetComponent(out Door door)) {
                door.TryOpen(fusesCollected, totalFusesNeeded);
            }
        }
    }
}