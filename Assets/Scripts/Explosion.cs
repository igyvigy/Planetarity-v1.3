using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Planetarity
{
    public class Explosion : MonoBehaviour
    {
        public static void Create(Vector3 pos, float scale)
        {
            var explosionGameobject = Instantiate(GameAssets.i.pfExplosion, pos, Quaternion.identity);
            explosionGameobject.transform.localScale = new Vector3(scale, scale, scale);
            var sparksGo = explosionGameobject.transform.Find("Sparks");
            sparksGo.localScale = new Vector3(scale, scale, scale);
            var explosion = explosionGameobject.GetComponent<ParticleSystem>();
            explosion.Play();
            Destroy(explosionGameobject, explosion.main.duration);
        }
    }
}


