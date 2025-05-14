using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
     public GameObject[] weapons;

    private bool isNear = false;
    private bool isOpenning = false;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && isNear && !isOpenning){
            StartCoroutine(OpenChest());
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player")){
            isNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        isNear = false;
    }

    private IEnumerator OpenChest(){
        isOpenning = true;
        int i = Random.Range(0,weapons.Length);
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        Instantiate(weapons[i], gameObject.transform.position + new Vector3(0,1,0), weapons[i].transform.rotation);
        Destroy(gameObject);
    }
}
