using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MaskToggle : MonoBehaviour, IDataPersistence
{
    public bool maskStatus;
    public bool readyToPress;
    public PlayerMovement pmScript;
    public DataPersistenceManager dpmScript;
    public Animator playerAnimation;
    public GameObject maskIndicator;
    public Timer timerScript;
    public DialogueManager dmScript;

    public int maskCount;
    public TextMeshProUGUI maskCountText;
    public Image timerFill;
    public Image vignetteImage;
    public AudioSource maskAudio;

    // Add LayerMask fields to specify the layers
    public LayerMask ghostLayer;
    public LayerMask ghostInteractableLayer; // New LayerMask for GhostInteractable layer
    public LayerMask trapezistPosterLayer;
    public LayerMask ghostGrappleLayer;

    void Start()
    {
        maskStatus = false;
        SetMaskIndicatorVisibility(false);
        SetParticleEffectsVisibility(true);
        playerAnimation = GameObject.Find("PlayerObjHolder").GetComponent<Animator>();
        pmScript = GameObject.Find("Player").GetComponent<PlayerMovement>();
        dpmScript = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        dmScript = GameObject.Find("DialogueBox").GetComponent<DialogueManager>();

        readyToPress = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && readyToPress && pmScript.hasMask == true)
        {
            maskToggle();

            readyToPress = false;
        }

        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            maskCount = maskCount + 1;
        }

        if (maskStatus == false)
        {
            SetMaskIndicatorVisibility(false);
            SetLayerVisibility(false);
            SetParticleEffectsVisibility(true);
            timerFill.enabled = false;
        }
        else if (maskStatus == true)
        {
            SetMaskIndicatorVisibility(true);
            SetLayerVisibility(true);
            SetParticleEffectsVisibility(false);

            timerFill.enabled = true;
        }

        maskCountText.text = "" + maskCount;
    }

    public void maskToggle()
    {
        if (maskStatus == false)
        {
            maskStatus = true;
            SetLayerVisibility(true);
            playerAnimation.SetTrigger("Mask On Trigger");
            pmScript.playerAudio.PlayOneShot(pmScript.maskSound, 0.5f);
            if (!maskAudio.isPlaying)
            {
                maskAudio.loop = true;
                maskAudio.Play();
            }
            timerScript.Pause = false;
            timerScript.Begin(timerScript.Duration);
        }
        else if (maskStatus == true)
        {
            maskStatus = false;
            SetLayerVisibility(false);
            playerAnimation.SetTrigger("Mask Off Trigger");
            pmScript.playerAudio.PlayOneShot(pmScript.demaskSound, 0.5f);
            if (maskAudio.isPlaying)
            {
                maskAudio.Stop();
            }
            timerScript.Begin(timerScript.Duration);
            timerScript.Pause = true;
            timerScript.obscurity.color = new Color(timerScript.obscurity.color.r, timerScript.obscurity.color.g, timerScript.obscurity.color.b, 0);
        }
        StartCoroutine(MaskCooldown());
        Debug.Log("LOL");
    }

    public void SetLayerVisibility(bool isVisible)
    {
        // Find all objects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check if the object is on the ghost layer
            if (((1 << obj.layer) & ghostLayer) != 0)
            {
                // Toggle the Renderer component
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = isVisible;
                }

                // Toggle the Collider component
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = isVisible;
                }

                // Toggle the Light component
                Light light = obj.GetComponent<Light>();
                if (light != null)
                {
                    light.enabled = isVisible;
                }

                Canvas canvas = obj.GetComponent<Canvas>();
                if (canvas != null)
                {
                    canvas.enabled = isVisible;
                }
            }

            // Check if the object is on the ghost interactable layer
            if (((1 << obj.layer) & ghostInteractableLayer) != 0)
            {
                // Toggle the Renderer component
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = isVisible;
                }

                // Toggle the Collider component
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = isVisible;
                }
            }

            if (((1 << obj.layer) & ghostGrappleLayer) != 0)
            {
                // Toggle the Renderer component
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = isVisible;
                }

                // Toggle the Collider component
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = isVisible;
                }
            }

            // Check if the object is on the trapezistPoster layer
            if (((1 << obj.layer) & trapezistPosterLayer) != 0)
            {
                // Only enable components if dialogueViewedSave is 7 or higher
                if (dmScript.dialogueViewedSave >= 7)
                {
                    // Toggle the Renderer component
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.enabled = isVisible;
                    }

                    // Toggle the Collider component
                    Collider collider = obj.GetComponent<Collider>();
                    if (collider != null)
                    {
                        collider.enabled = isVisible;
                    }

                    // Toggle the Canvas component
                    Canvas canvas = obj.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        canvas.enabled = isVisible;
                    }
                }
                else
                {
                    // Ensure components are disabled if dialogueViewedSave is less than 7
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.enabled = false;
                    }

                    Collider collider = obj.GetComponent<Collider>();
                    if (collider != null)
                    {
                        collider.enabled = false;
                    }

                    Canvas canvas = obj.GetComponent<Canvas>();
                    if (canvas != null)
                    {
                        canvas.enabled = false;
                    }
                }
            }

            if (obj.layer == LayerMask.NameToLayer("Inverse") || obj.layer == LayerMask.NameToLayer("InverseSwap"))
            {
                // Toggle the Renderer component
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = !isVisible; // Opposite of ghost layer visibility
                }

                // Toggle the Collider component
                Collider collider = obj.GetComponent<Collider>();
                if (collider != null)
                {
                    collider.enabled = !isVisible; // Opposite of ghost layer visibility
                }
            }
        }
    }

    private void SetMaskIndicatorVisibility(bool isVisible)
    {
        // Get the Image component on the maskIndicator
        Image maskImage = maskIndicator.GetComponent<Image>();
        if (maskImage != null)
        {
            maskImage.enabled = isVisible;
        }

        // Get the Image components on the children of the maskIndicator
        Image[] childImages = maskIndicator.GetComponentsInChildren<Image>();
        foreach (Image img in childImages)
        {
            img.enabled = isVisible;
        }

        if (vignetteImage != null)
        {
            vignetteImage.enabled = isVisible;
        }
    }

    public IEnumerator MaskCooldown()
    {
        yield return new WaitForSeconds(2);
        readyToPress = true;
        Debug.Log("Mask Ready!");
    }

    public void LoadData(GameData data)
    {
        maskCount = data.maskCount;
    }

    public void SaveData(ref GameData data)
    {
        data.maskCount = maskCount;
    }

    public void SetParticleEffectsVisibility(bool isVisible)
    {
        // Find all ParticleSystem components in the scene
        ParticleSystem[] allParticleSystems = FindObjectsOfType<ParticleSystem>();

        foreach (ParticleSystem ps in allParticleSystems)
        {
            if (ps.gameObject.layer == LayerMask.NameToLayer("ParticleGhost"))
            {
                if (isVisible)
                {
                    if (!ps.isPlaying)
                    {
                        ps.Clear();
                        ps.Play();
                    }
                }
                else
                {
                    if (ps.isPlaying)
                    {
                        ps.Stop();
                    }
                }
            }

            if (ps.gameObject.layer == LayerMask.NameToLayer("ParticleInverse"))
            {
                if (!isVisible)
                {
                    if (!ps.isPlaying)
                    {
                        ps.Clear();
                        ps.Play();
                    }
                }
                else
                {
                    if (ps.isPlaying)
                    {
                        ps.Stop();
                    }
                }
            }
        }
    }
}
