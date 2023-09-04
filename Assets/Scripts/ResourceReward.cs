using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceReward : MonoBehaviour
{
    [SerializeField] private int _woodReward = 3;
    [SerializeField] private int _rockReward = 2;

    private static ResourceReward s_instance;

    public static ResourceReward Instance => s_instance;

    public int WoodReward => _woodReward;
    public int RockReward => _rockReward;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
