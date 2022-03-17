using System;
using System.Collections.Generic;
using UnityEngine;
public class ShootingBehaviour : MonoBehaviour
{
    [SerializeField] protected float fireDelay = 02f;
    [SerializeField] protected AudioClip sFX;
    [SerializeField] protected List<Transform> firePoints;

    public static Action<AudioClip> OnFire = delegate { };
    protected float timer;
}