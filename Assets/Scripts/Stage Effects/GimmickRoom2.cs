using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GimmickRoom2 : MonoBehaviour
{
    private float checkInterval = 0.5f;
    private Collider triggerCollider;
    public GameObject door;
    public LayerMask hittableLayer;

    private bool isCoroutineStart = false;

    private Material ogSkybox;
    private AmbientMode ogAmbientMode;
    private Color ogAmbientColor;

    //For Main Camera to Change setting when Trigger
    private Camera playerCam;
    private CameraClearFlags ogClearFlags;
    private Color ogBackgroundColor;

    private bool isRestDark = false;
    // Start is called before the first frame update
    void Start()
    {
        triggerCollider = GetComponent<Collider>();
        StartCoroutine(CheckForHittableObjects());
        isCoroutineStart = true;

        // Store the original ambient light color
        ogAmbientColor = RenderSettings.ambientLight;
        // Store the original Skybox Material
        ogSkybox = RenderSettings.skybox;
        //Store the original Ambient Mode
        ogAmbientMode = RenderSettings.ambientMode;

        //Find and save camera settings in the inspector
        playerCam = Camera.main;
        if (playerCam != null)
        {
            ogClearFlags = playerCam.clearFlags;
            ogBackgroundColor = playerCam.backgroundColor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCoroutineStart)
        {
            StartCoroutine(CheckForHittableObjects());
        }
    }


    private IEnumerator CheckForHittableObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            Bounds colliderBounds = triggerCollider.bounds;

            Collider[] colliders = Physics.OverlapBox(
                colliderBounds.center,
                colliderBounds.extents,
                transform.rotation,
                hittableLayer
            );

            if (colliders.Length <= 3)
            {
                door.SetActive(true);
                if (!isRestDark) {
                    isRestDark = true;
                    ResetDarkRoom();
                }
                
            }
            else
            {
                door.SetActive(false);
            }
        }
    }

    void OnDisable()
    {
        isCoroutineStart = false;
    }
    
    public void ResetDarkRoom()
    {
        // Restore original lighting
        RenderSettings.skybox = ogSkybox;
        RenderSettings.ambientMode = ogAmbientMode;
        RenderSettings.ambientLight = ogAmbientColor;

        // Restore camera settings
        if (playerCam != null)
        {
            playerCam.clearFlags = ogClearFlags;
            playerCam.backgroundColor = ogBackgroundColor;
        }

        // Turn off fog
        RenderSettings.fog = false;

        Debug.Log("Dark room deactivated and lighting reset.");
    }
}
