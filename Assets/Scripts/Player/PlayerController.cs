using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class PlayerController : FPSInput
{

    //------------------------------------------------------------------------------------junjie add debuff------------------------------------------------------------------------------
    private List<string> debuffList = new List<string>();

    private Vector3 originalPos;
    private bool isInvincible;
    private Cone cone;
    private IEnumerator attackCoroutine;

    //Debuff Booleans
    private bool isPoisoned, isBurned, isBleeding;

    public GameObject[] weapons;
    public GameObject[] cones;

    [Header("Camera")]
    public Camera cam;

    [Header("Stats")]
    public int maxHealth = 100;
    public TMP_Text healthText;
    public Slider slider;
    int currentHealth;
    GameObject currentWeap;
    Weapons weap;
    Shield shield;

    [Header("Models")]
    public Transform RH;
    public Transform LH;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // get the components
        charController = GetComponent<CharacterController>();
        animator = GetComponentsInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        weap = GetComponentInChildren<Weapons>();
        shield = GetComponentInChildren<Shield>();

        currentHealth = maxHealth;
        healthText.SetText(currentHealth.ToString());

        slider.maxValue = maxHealth;
        slider.value = currentHealth; 

        weapons = new GameObject[2];

        currentWeap = weapons[0];

        attackLayer = LayerMask.GetMask("Hittable");

        originalPos = gameObject.transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        // initiate attack on fire button press
        if (Input.GetButtonDown("Fire1"))
        {
            weap.Attack();
        }
        // initiate Special Attack on RMB
        if (Input.GetButtonDown("Fire2"))
        {
            weap.SpecialAttack();
        }

        SetAnimations();

        if (TryGetComponent(out ShieldStat shieldStat))
        {
            Debug.Log("True");
            isInvincible = true;
        }

        Debuff();
        SceneChange();
    }

    // Animation
    public const string IDLE = "Idle";
    public const string SHIELDI = "Shield Idle";
    public const string SWORDI = "Sword Idle";
    public const string THIDLE = "ThIdle";
    public const string WALK = "Walk";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";
    public const string SWORDATTACK = "Sword Attack";
    public const string SWORDHEAVY = "Sword Heavy";
    public const string AXEATTACK = "Axe Attack";
    public const string AXEHEAVY = "Axe Heavy";
    public const string THROW = "Throw";
    
    public const string BLOCk = "Block";

    string currentAnimationState;

    public void ChangeAnimationState(string newState)
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;


        //Debug.Log(currentAnimationState);
        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        foreach (Animator anim in animator) {
            anim.CrossFadeInFixedTime(currentAnimationState, 0.2f);
        }
    }

    void SetAnimations()
    {
        // If player is not attacking
        if (!attacking)
        {

            RH.transform.localPosition = Vector3.zero + new Vector3(0.05f, -1.46f, 0.7f); // Reset Arm animation
            RH.transform.localRotation = new Quaternion(-0.16044f, 0, 0.02294f, 0.98678f);
            if (movement.x == 0 && movement.z == 0)
            { weap.Idle(); }
            else
            { weap.Walk(); }
        }
    }

    public void Idle()
    {
        ChangeAnimationState(IDLE);
    }

    public void SwordIdle()
    {
        ChangeAnimationState(SHIELDI);
    }

    public void ShieldIdle()
    {
        ChangeAnimationState(SHIELDI);
    }

    public void THIdle()
    {
        ChangeAnimationState(THIDLE);
    }

    public void Walk()
    {
        ChangeAnimationState(WALK);
    }

    public void Throw()
    {
        if (!readyToAttack || attacking) return;
        attacking = true;
        readyToAttack = false;

        // Set Arm animation to throw at the right position
        RH.transform.position = RH.position - new Vector3(0, 0.4f, 0);
        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(Thrown), attackDelay);

        ChangeAnimationState(THROW);
    }

    // Attack

    [HideInInspector] public float attackDistance = 3f;
    [HideInInspector] public float attackDelay = 0.4f;
    [HideInInspector] public float attackSpeed = 1f;
    [HideInInspector] public int attackDamage = 25;
    [HideInInspector] public float hitSoundDelay;
    private float basicSpeed;
    private int basicDamage;

    
    [HideInInspector] public LayerMask attackLayer;

    [Header("Attacking")]
    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public bool Attacking()
    {
        if (!readyToAttack || attacking) return false;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);
        Invoke(nameof(HitAudio), hitSoundDelay);

        return true;
    }

    public void HitAudio()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swordSwing);
    }

    // Basic Attack
    public void Attack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;

        bool attack = Attacking();
        if (!attack) 
            return;

        if (attackCount == 0)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            RH.transform.localPosition =  new Vector3(-0.45f, -1.46f, 0.6f);
            RH.transform.Rotate(0, -20f, 0, Space.Self);
            ChangeAnimationState(ATTACK2);
            attackCount = 0;
        }
    }

    public void SwordAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.05f, -1.46f, 0.0f);
        ChangeAnimationState(SWORDATTACK);
        attackCount++;
    }

    public void AxeAttack()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.05f, -1.46f, 0.2f);
        ChangeAnimationState(AXEATTACK);
        attackCount++;
    }

    public void Thrown()
    {
        weap.Thrown();
    }

    public void SwordHeavy()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 1.5f;
        attackDelay = 1f;
        attackDamage = 50;
        hitSoundDelay = 1f;

        bool attack = Attacking();
        if (!attack)
            return;
        RH.transform.localPosition = Vector3.zero + new Vector3(0.3f, -1.46f, 0.2f);
        ChangeAnimationState(SWORDHEAVY);
        attackCount++;
    }

    public void AxeHeavy()
    {
        basicSpeed = attackSpeed;
        basicDamage = attackDamage;
        attackSpeed = 2.5f;
        attackDelay = 2f;
        attackDamage = 75;
        hitSoundDelay = 2f;

        bool attack = Attacking();
        if (!attack)
            return;

        RH.transform.localPosition = Vector3.zero + new Vector3(0.6f, -1.46f, 0f);
        ChangeAnimationState(AXEHEAVY);
        attackCount++;
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        cone = GetComponentInChildren<Cone>(false);

        foreach (GameObject gObject in cone.GetArray())
        {
            if(gObject.IsDestroyed()) continue;
            print(gObject.name);
            
            if (Physics.Linecast(cam.transform.position, gObject.transform.position, out RaycastHit hits, attackLayer))
            {
                HitTarget(hits.point);

                if (hits.transform.TryGetComponent<EnemyBehaviour>(out EnemyBehaviour T))
                {
                    T.TakeDamage(attackDamage);
                }
            }
        }


        attackDamage = basicDamage;
    }

    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }

    // Health Logic
    public void TakeDamge(int amount)
    {
        //if (!isInvincible)
        //{
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Prevent health from going below 0
            healthText.SetText(currentHealth.ToString());  // Update health text display
            slider.value = currentHealth;  // Update slider value
        //}
        //else { }
    }
    public void Heal(int amount)
    {
        
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // Prevent health from going above maxHealth
            healthText.SetText(currentHealth.ToString());  // Update health text display
            slider.value = currentHealth;  // Update slider value
        
    }

    //Scene Change
    void SceneChange()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "GameScene") // Change the name to the demo scene name
            {
                SceneManager.LoadScene("SampleScene");
            }
            else if (scene.name == "SampleScene")
            {
                SceneManager.LoadScene("GameScene"); // Change the name to the demo scene name
            }
        }
    }

    //Debuffs

    private void Debuff()
    {
        foreach (string d in debuffList)
        {
            switch (d)
            {
                case "Poison":

                    if (isPoisoned)
                        StopCoroutine(PoisonDegen(10));
                    isPoisoned = true;
                    StartCoroutine(PoisonDegen(10));
                    break;
                case "Burn":
                    if (isBurned)
                        StopCoroutine(Burned(5));
                    isBurned = true;
                    StartCoroutine(Burned(5));
                    break;
                case "Bleed":
                    if (isBleeding)
                        StopCoroutine(Bleeding(20));
                    isBleeding = true;
                    StartCoroutine(Bleeding(20));
                    break;
                default:
                    Debug.LogWarning("Debuff does not exist");
                    break;
            }
            
        }
        debuffList.Clear();
    }

    IEnumerator PoisonDegen(int degenCount)
    {
        for (int degenCounter = degenCount; degenCounter > 0; degenCounter--)
        {
            if (degenCounter != 0)
            {
                TakeDamge(3);
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                isPoisoned = false;
            }
        }
    }

    IEnumerator Burned(int burnCount)
    {
        for (int burnCounter = burnCount; burnCounter > 0; burnCounter--)
        {
            if (burnCounter != 0)
            {
                TakeDamge(5);
                yield return new WaitForSeconds(3.0f);
            }
            else
            {
                isBurned = false;
            }
        }
    }

    IEnumerator Bleeding(int bleedCount)
    {
        for (int bleedCounter = bleedCount; bleedCounter > 0; bleedCounter--)
        {
            if (bleedCounter != 0)
            {
                TakeDamge(1);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                isBurned = false;
            }
        }
    }

    //junjie add
    public void ChangePlayerPosition(Vector3 position){
        transform.position = position;
    }

    //------------------------------------------------------------------------------------junjie add debuff------------------------------------------------------------------------------
    public void AddDebuff(string debuff){
        debuffList.Add(debuff);
    }

}
