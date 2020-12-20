using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Target : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Magic _magic;
    [SerializeField] private Material _original;
    [SerializeField] private Material _hit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Effect"))
        {
            StartCoroutine(Hit());

            _magic._hitCount++;
            _magic.UpdatePercent();

            Destroy(other.gameObject);
        }           
    }

    IEnumerator Hit()
    {
        _meshRenderer.material = _hit;

        yield return new WaitForSeconds(1f);

        _meshRenderer.material = _original;
    }



}
