using System.Collections;
using UnityEngine;

public class BuffChest : MonoBehaviour
{
    public GameObject keyPrefab;
    private CanvasGroup interactionCanvasGroup;

    public float baseKeyChance = 5f;
    public float keyChanceIncrease = 2f;
    private static float currentKeyChance = 5f;

    private bool isPlayerInRange = false;
    private PlayerController playerController;

    public float fadeDuration = 0.5f;

    private void Start()
    {
        GameObject borderObj = GameObject.Find("interactionborder");
        if (borderObj != null)
        {
            interactionCanvasGroup = borderObj.GetComponent<CanvasGroup>();
            if (interactionCanvasGroup != null)
            {
                interactionCanvasGroup.alpha = 0f; // Start invisible
                interactionCanvasGroup.blocksRaycasts = true;
                interactionCanvasGroup.interactable = true;
            }
        }
        else
        {
            Debug.LogWarning("Interaction border not found.");
        }
    }


    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(FadeOutUI());
            OpenChest();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerController = other.GetComponent<PlayerController>();
            if (interactionCanvasGroup != null)
                StartCoroutine(FadeInUI());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerController = null;
            if (interactionCanvasGroup != null)
                StartCoroutine(FadeOutUI());
        }
    }

    public void OpenChest()
    {
        if (playerController == null) return;

        float roll = Random.Range(0f, 100f);

        if (roll < currentKeyChance)
        {
            Instantiate(keyPrefab, transform.position, Quaternion.identity);
            Debug.Log("Key obtained!");
            currentKeyChance = baseKeyChance;
        }
        else if (roll < 50f)
        {
            ApplyGoodEffect();
        }
        else
        {
            ApplyBadEffect();
        }

        currentKeyChance += keyChanceIncrease;
        Destroy(gameObject);
        interactionCanvasGroup.alpha = 0f;
    }

    private void ApplyGoodEffect()
    {
        int effectType = Random.Range(0, 3);
        switch (effectType)
        {
            case 0: playerController.Heal(10); break;
            case 1: playerController.ChangeSpeed(3); break;
            case 2: playerController.ChangeJump(10); break;
        }
    }

    private void ApplyBadEffect()
    {
        int effectType = Random.Range(0, 3);
        switch (effectType)
        {
            case 0: playerController.TakeDamge(10); break;
            case 1: playerController.ChangeSpeed(-3); break;
            case 2: playerController.ChangeJump(-3); break;
        }
    }

    private IEnumerator FadeInUI()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            interactionCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        interactionCanvasGroup.alpha = 1f;
     
    }

    private IEnumerator FadeOutUI()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            interactionCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        interactionCanvasGroup.alpha = 0f;
    }
}
