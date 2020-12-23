
/**
 * 마법을 만들고 쏘는 스크립트
 */

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
    private List<ParticleSystem> _magicParticleList;

    [Header("Player")]
    [SerializeField] private Text _aimPercentText;
    private float _shotCount;
    public float _hitCount;

    void Start()
    {
        instance = this;

        _leftHandRd = _leftHand.GetComponent<Rigidbody>();
        _rightHandRd = _rightHand.GetComponent<Rigidbody>();

        _magicParticleList = new List<ParticleSystem>();

        _shotCount = 0;
        _hitCount = 0;
    }

    private void FixedUpdate()
    {
        // 양 손바닥 사이의 거리를 측정
        distance = Vector3.Distance(_leftHand.position, _rightHand.position);

        // 거리가 0.1 보다 작고 마법이 생성되지 않은 상태
        if (distance < 0.1f && !_activeMagic) CreateEffect();

        // 마법이 생성되지 않았거나, 현재 생성된 이팩트가 없는경우
        if (!_activeMagic || _currentEffect == null) return;

        // 마법 발사
        ShotMagic();
    }


    private void CreateEffect()
    {
        // 초기화
        _activeMagic = true;

        // 랜덤으로 이펙트 결정 후 생성
        index = Random.Range(0, _magicArray.Length);
        _currentEffect = Instantiate(_magicArray[index]);
        _currentEffect.GetComponent<DestoryEffect>().SetMagic(instance);

        _magicRd = _currentEffect.GetComponent<Rigidbody>();

        _magicParticleList.Clear();

        // 여러 파티클로 이루어 진것을 리스트로 담음
        for (int i = 0; i < _currentEffect.childCount; i++)
            _magicParticleList.Add(_currentEffect.GetChild(i).GetComponent<ParticleSystem>());
    }

    private void ShotMagic()
    {
        // 손 바닥의 거리가 0.5보다 커지면 그냥 앞으로 발사
        if (distance > 0.5f)
        {
            _activeMagic = false;
            _magicRd.AddForce(Vector3.forward * 2, ForceMode.Impulse);
            _shotCount++;
            return;
        }

        // 이펙트 위치를 양손의 중앙으로 설정
        _currentEffect.position = (_leftHand.position + _rightHand.position) * 0.5f;

        // 리스트에 담긴 여러 파티클을 꺼내서 스케일 조정
        for (int i = 0; i < _magicParticleList.Count; i++)
            _magicParticleList[i].transform.localScale = new Vector3(distance, distance, distance) * 0.1f;

        // 양손의 힘이 2보다 크고 거리가 0.2보다 크면 마법 발사
        if (_leftHandRd.velocity.magnitude > 2f && _rightHandRd.velocity.magnitude > 2f && distance > 0.2f)
        {
            _activeMagic = false;
            var direction = _leftHandRd.velocity.normalized;
           
            // 방향 조정
            direction = new Vector3(Mathf.Clamp(direction.x,-0.4f,0.4f), Mathf.Clamp(direction.y, -0.05f, 0.05f), Mathf.Clamp(direction.z,0.1f,10));
          
            _magicRd.AddForce(direction * 5, ForceMode.Impulse);
            _shotCount++;
        }
    }

    /// <summary>
    /// 명중률 업데이트
    /// </summary>
    public void UpdatePercent()
    {
        var percent = _hitCount / _shotCount * 100;

        _aimPercentText.text = "명중률 : " + percent.ToString("F1") + "%";
    }
}
