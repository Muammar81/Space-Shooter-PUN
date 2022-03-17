using PanettoneGames;
using System;
using System.Collections.Generic;
using UnityEngine;
public class ShootingBehaviour : MonoBehaviour
{
    [SerializeField] protected float fireDelay = 02f;
    [SerializeField] protected AudioClip sFX;
    [SerializeField] protected List<Transform> firePoints;
    [SerializeField] private GameObjectPool pool;

    public static Action<AudioClip> OnFire = delegate { };
    protected float timer;
    private void Awake() => pool.Prewarm();
    protected void Fire()
    {
        for (int i = 0; i < firePoints.Count; i++)
        {
            var bullet = pool.Get();
            bullet.transform.position = firePoints[i].position;
            bullet.transform.rotation = firePoints[i].rotation;
        }
        OnFire?.Invoke(sFX);
    }
}