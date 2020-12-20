using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryEffect : MonoBehaviour
{
    private Magic magic;
    private void FixedUpdate()
    {
        if (transform.position.z < -2 || transform.position.z > 15
            || transform.position.x < -7 || transform.position.x > 7
            || transform.position.y < -1 || transform.position.y > 10)
        {
            magic.UpdatePercent();
            Destroy(gameObject);
        }
    }
    public void SetMagic(Magic magic) => this.magic = magic;
}
