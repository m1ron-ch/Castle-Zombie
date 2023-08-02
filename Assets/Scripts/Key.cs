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

    public enum Prefs
    {
        Wood, Rock, Coins, Gem, InvetoryCapacity, MaxInvetoryCapacity,
    }
}
