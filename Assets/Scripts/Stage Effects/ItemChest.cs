using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemChest : MonoBehaviour
{
    public GameObject[] items;

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
        int i = Random.Range(0,10);
        audioSource.Play();
        yield return new WaitForSeconds(2f);
        if(i<=4){
            Instantiate(items[0], gameObject.transform.position + new Vector3(0,0,0), items[0].transform.rotation);
        }else if(i<=7){
            Instantiate(items[1], gameObject.transform.position + new Vector3(0,0,0), items[1].transform.rotation);
        }else{
            Instantiate(items[2], gameObject.transform.position + new Vector3(0,0,0), items[2].transform.rotation);
        }
        Destroy(gameObject);

    }
}
