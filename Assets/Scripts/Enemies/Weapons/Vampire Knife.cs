using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireKnife : MonoBehaviour
{
    //get the vampire behaviour
    private VampireBehaviour vampireBehaviour;
    private bool isInvisible;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        vampireBehaviour = GetComponentInParent<VampireBehaviour>();
        meshRenderer = transform.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isInvisible = vampireBehaviour.GetIsInvisible();
        if(isInvisible){
            meshRenderer.enabled = !isInvisible;
        }else{
            meshRenderer.enabled = !isInvisible;
        }
    }
}
