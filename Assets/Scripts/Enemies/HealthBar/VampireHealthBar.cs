using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VampireHealthBar : MonoBehaviour
{
    private Slider healthSlider;
    private Slider easeHealthSlider;
    private float maxHealth;
    private float currentHealth;
    private float lerpSpeed = 0.005f;

    private EnemyBehaviour enemy;

    //invisible
    private VampireBehaviour vampireBehaviour;
    private bool isInvisible;

    // Start is called before the first frame update
    void Start()
    {
        //set two slider
        healthSlider = transform.Find("Health Bar").GetComponent<Slider>();
        easeHealthSlider = transform.Find("Ease Health Bar").GetComponent<Slider>();

        enemy = FindEnemyBehaviourComponent(transform);

        maxHealth = enemy.GetEnemyMaxHealth();
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth; 
        easeHealthSlider.maxValue = maxHealth;

        //invisible
        vampireBehaviour = GetComponentInParent<VampireBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = enemy.GetEnemyCurrentHealth();
        UpdateHealthBarVisibility();

        if(healthSlider.value != currentHealth){
            healthSlider.value = currentHealth;
        }

        if(healthSlider.value != easeHealthSlider.value){
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, currentHealth, lerpSpeed);
        }

        //invisible
        isInvisible = vampireBehaviour.GetIsInvisible();
    }

    private EnemyBehaviour FindEnemyBehaviourComponent(Transform currentTransform)
    {
        // if object has EnemyBehaviour componet return the componet
        EnemyBehaviour enemyScript = currentTransform.GetComponent<EnemyBehaviour>();
        if (enemyScript != null)
        {
            return enemyScript;
        }

        // if got parent class find go up again
        if (currentTransform.parent != null)
        {
            return FindEnemyBehaviourComponent(currentTransform.parent);
        }

        // if not return null
        return null;
    }

    private void UpdateHealthBarVisibility()
    {
        bool shouldHide = (currentHealth == maxHealth || currentHealth <= 0 || isInvisible);
        healthSlider.gameObject.SetActive(!shouldHide);
        easeHealthSlider.gameObject.SetActive(!shouldHide);
    }
}
