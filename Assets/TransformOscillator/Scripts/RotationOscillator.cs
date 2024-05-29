using UnityEngine;

namespace TransformOscillator.Scripts
{
    public class RotationOscillator : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] internal Vector3 _targetRotation = new Vector3(0, 180, 0);
        [SerializeField] private float _speed = 1f;
        [SerializeField] private PeriodType _type;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private bool _playOnStart = true;

        private bool _activated = false;
        private Quaternion _defaultRotation;

        private void Awake()
        {
            _defaultRotation = _pivot.localRotation;
        }

        private void Start()
        {
            if (_playOnStart)
                Play();
        }

        public void Play()
        {
            _activated = true;
        }

        public void Stop()
        {
            _activated = false;
        }

        private void Update()
        {
            if (!_activated) return;

            var time = GetTimeFromPeriodType();
            var offset = _animationCurve.Evaluate(time);
            _pivot.localRotation = Quaternion.Lerp(_defaultRotation, Quaternion.Euler(_targetRotation), offset);
        }

        private float GetTimeFromPeriodType() => _type == PeriodType.Oscillator
            ? Mathf.PingPong(Time.time * _speed, 1)
            : Mathf.Repeat(Time.time * _speed, 1);
    }
}