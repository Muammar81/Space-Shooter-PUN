using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] [Tooltip("SFX particle")] ParticleSystem FX;
    [SerializeField] private AudioClip sFX;

    [SerializeField] [Tooltip("Damaging Objects Layer")] private LayerMask hitLayers;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((hitLayers & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            print("player hit with astroid");
            FX?.Play();
            var delay = Mathf.Max(FX.main.duration, sFX.length);
            KillPlayer(delay*100);
        }
    }

    private async void KillPlayer(float delay)
    {
        if (delay > 0)
        {
            //Disable Visuals
            var rends = gameObject.GetComponentsInChildren<Renderer>(true).ToList();
            rends.ForEach(r => r.enabled = false);

            //Disable Physics
            var cols = gameObject.GetComponentsInChildren<Collider>(true).ToList();
            cols.ForEach(c => c.enabled = false);

            var rbs = gameObject.GetComponentsInChildren<Rigidbody2D>(true).ToList();
            rbs.ForEach(rb => Destroy(rb));

            //Delete everything but this component


            var components = gameObject.GetComponentsInChildren<Component>().ToList();
            foreach (var c in components)
            {
                if (c == this || c.GetType() == typeof(Transform)) continue;
                Destroy(c);
            }
            var ts = gameObject.GetComponentsInChildren<Transform>().ToList();
            ts.Where(t => t != transform).ToList().ForEach(tt => Destroy(tt.gameObject));

            var endTime = Time.time + delay;
            while (Time.time < endTime)
            {
                await Task.Yield();
            }
        }
        Debug.Log("You died!");



  
        //Kill Player/Game Over Logic
        //Destroy(gameObject);
    }
}
