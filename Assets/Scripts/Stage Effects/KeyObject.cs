using UnityEngine;
using TMPro;

public class KeyObjectInteraction : MonoBehaviour
{
    public GameObject keyPrefab;
    public TextMeshProUGUI interactionText;

    private bool isPlayerNearby = false;
    private static bool keyFound = false;

    void Start()
    {
        if (KeyManager.Instance != null)
            KeyManager.Instance.Register(this);

        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNearby && !keyFound && Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (KeyManager.Instance != null && KeyManager.Instance.IsKeyHolder(this))
        {
            Instantiate(keyPrefab, transform.position + Vector3.up, Quaternion.identity);
            Debug.Log("? You found the key!");
            keyFound = true;
        }
        else
        {
            Debug.Log("? Nothing here...");
        }

        if (interactionText != null)
            interactionText.gameObject.SetActive(false);

        isPlayerNearby = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!keyFound && other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (interactionText != null)
            {
                interactionText.text = "Press F to search";
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }
}
