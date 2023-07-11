using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunSystem : MonoBehaviour
{
    [SerializeField] public float stunScale { get; private set; } = 0;

    public bool IsStunned { get; private set; } = false;
    

    private void Awake()
    {
        GameManager.serviceLocator.ProvideServices(GameManager.serviceLocator.GetAudioMixer(), this);

    }
    
    

}
