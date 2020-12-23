/** 
 * 범위를 나가면 이펙트를 파괴시키는 스크립트
 */

using UnityEngine;

public class DestoryEffect : MonoBehaviour
{
    private Magic _magic;

    private void FixedUpdate()
    {
        if (transform.position.z < -2 || transform.position.z > 15
            || transform.position.x < -7 || transform.position.x > 7
            || transform.position.y < -1 || transform.position.y > 10)
        {
            _magic.UpdatePercent();
            Destroy(gameObject);
        }
    }

    public void SetMagic(Magic magic) => this._magic = magic;
}
