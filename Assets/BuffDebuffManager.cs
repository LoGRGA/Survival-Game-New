using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BuffDebuffManager : MonoBehaviour
{
    public Image poisonDebuffIcon;
    public Image slownessDebuffIcon;
    public Image speedBuffIcon;
    public Image damageBuffIcon;

    private HashSet<string> activeEffects = new HashSet<string>();

    void Start()
    {
        UpdateIcons();
    }

    void UpdateIcons()
    {
        poisonDebuffIcon.gameObject.SetActive(activeEffects.Contains("Poison"));
        slownessDebuffIcon.gameObject.SetActive(activeEffects.Contains("Slowness"));
        speedBuffIcon.gameObject.SetActive(activeEffects.Contains("Speed"));
        damageBuffIcon.gameObject.SetActive(activeEffects.Contains("Damage"));
    }

    public void ApplyEffect(string effect)
    {
        activeEffects.Add(effect);
        UpdateIcons();
    }

    public void RemoveEffect(string effect)
    {
        activeEffects.Remove(effect);
        UpdateIcons();
    }
}
