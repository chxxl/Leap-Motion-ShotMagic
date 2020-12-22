using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Magic : MonoBehaviour
{
    Magic instance;

    [Header("Hand")]
    [SerializeField] private Transform _leftHand;
    [SerializeField] private Transform _rightHand;
    private Rigidbody _leftHandRd;
    private Rigidbody _rightHandRd;
    private float distance;

    [Header("Effect")]
    [SerializeField] private Transform[] _magicArray;
    private Transform _currentEffect;
    private Rigidbody _magicRd;
    private int index;
    private bool _activeMagic;
    private List<ParticleSystem> _magicParticle;

    [Header("Player")]
    [SerializeField] private Text _aimPercentText;
    private float _shotCount;
    public float _hitCount;

    void Start()
    {
        instance = this;

        _leftHandRd = _leftHand.GetComponent<Rigidbody>();
        _rightHandRd = _rightHand.GetComponent<Rigidbody>();

        _magicParticle = new List<ParticleSystem>();

        _shotCount = 0;
        _hitCount = 0;
    }

    private void FixedUpdate()
    {
        distance = Vector3.Distance(_leftHand.position, _rightHand.position);

        if (distance < 0.1f && !_activeMagic) ResetEffect();

        if (!_activeMagic || _currentEffect == null) return;

        ShotMagic();
    }


    private void ResetEffect()
    {
        // 초기화
        _activeMagic = true;

        // 랜덤 이팩트
        index = Random.Range(0, _magicArray.Length);
        _currentEffect = Instantiate(_magicArray[index]);
        _currentEffect.GetComponent<DestoryEffect>().SetMagic(instance);

        _magicRd = _currentEffect.GetComponent<Rigidbody>();

        _magicParticle.Clear();

        for (int i = 0; i < _currentEffect.childCount; i++)
            _magicParticle.Add(_currentEffect.GetChild(i).GetComponent<ParticleSystem>());
    }

    private void ShotMagic()
    {
        if (distance > 0.5f)
        {
            _activeMagic = false;
            _magicRd.AddForce(Vector3.forward * 2, ForceMode.Impulse);
            _shotCount++;
            return;
        }

        _currentEffect.position = (_leftHand.position + _rightHand.position) * 0.5f;

        for (int i = 0; i < _magicParticle.Count; i++)
            _magicParticle[i].transform.localScale = new Vector3(distance, distance, distance) * 0.1f;

        if (_leftHandRd.velocity.magnitude > 2f && _rightHandRd.velocity.magnitude > 2f && distance > 0.2f)
        {
            _activeMagic = false;
            var direction = _leftHandRd.velocity.normalized;
           
            direction = new Vector3(Mathf.Clamp(direction.x,-0.4f,0.4f), Mathf.Clamp(direction.y, -0.1f, 0.1f), Mathf.Clamp(direction.z,0.1f,10));
            print(direction);
            _magicRd.AddForce(direction * 5, ForceMode.Impulse);
            _shotCount++;
        }
    }

    public void UpdatePercent()
    {
        var percent = _hitCount / _shotCount * 100;

        _aimPercentText.text = "명중률 : " + percent.ToString("F1") + "%";
    }
}
