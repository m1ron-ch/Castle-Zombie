using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public enum Animations
    {
        Idle, Walking, Running, Chop, PistolIdle, PistolRunning
    }

    public enum ObjectType
    {
        Tree, Rock, MachineGun
    }

    public enum ResourcePrefs
    {
        Wood, Rock, Coins, Gems
    }

    public enum Prefs
    {
        InvetoryCapacity, MaxInvetoryCapacity, CurrentBuildingHierarchy, Buldings
    }
}
