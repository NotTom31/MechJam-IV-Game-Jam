using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockModule : MonoBehaviour
{
    [SerializeField] private MechState mechStateRef;
    [SerializeField] private Collider blockVolume;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Animator animatorRef;
    [Tooltip("Energy cost applied per second.")]
    [SerializeField] [Range(0, 1)] private float energyCostPercent;
    public bool IsBlocking { get; private set; }
    private bool isAIControl = false;
    private float blockTime = 0;
    
    public delegate void EnergyChangeHandler(float energyChangePercent, int mechIndex);
    public static event EnergyChangeHandler OnEnergyChange;

    // Start is called before the first frame update
    void Awake()
    {
        if (gameObject.GetComponent<PlayerInput>() == null && gameObject.GetComponentInChildren<PlayerInput>() == null)
            isAIControl = true;
        blockVolume.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAIControl)
        {
            blockTime -= Time.deltaTime;
            if (blockTime <= 0f)
            {
                SetBlock(false);
            }
            else
                SetBlock(true);
            return;
        }
        if(blockVolume.enabled == true)
        {
            OnEnergyChange?.Invoke(-energyCostPercent * Time.deltaTime, mechStateRef.GetMechIndex());
        }
    }
    private void SetBlock(bool boolValue)
    {
        if (mechStateRef.Energy <= 0f)
        { 
            blockVolume.enabled = false;
            meshRenderer.enabled = false;
            IsBlocking = false;
            animatorRef.SetBool("Is Block", false);
            return;
        }

        blockVolume.enabled = boolValue;
        meshRenderer.enabled = boolValue;
        IsBlocking = boolValue;
        animatorRef.SetBool("Is Block", boolValue);
    }
    public void OnBlock(float timeSeconds)
    {
        blockTime = timeSeconds;
        SetBlock(true);
    }
    void OnBlock(InputValue value)
    {
        /*if (GameManager.instance.gameState != GameState.PLAYING_ACTIVE) //Prevent attacking when loading, commented out because shielding while loading is fun and harmless
            return;*/
        if (value.isPressed)
        {
            Debug.Log("Block held!");
            SetBlock(true);
        }
        else
        {
            SetBlock(false);
        }
    }

}
