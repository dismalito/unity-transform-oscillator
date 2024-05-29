using UnityEngine;

namespace TransformOscillator.Scripts
{
    public class ScaleOscillator : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] internal Vector3 _targetScale = new Vector3(2,2,2);
        [SerializeField] private float _speed = 1f;
        [SerializeField] private PeriodType _type;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private bool _playOnStart = true;

        private bool _activated = false;
        private Vector3 _defaultScale;

        private void Awake()
        {
            _defaultScale = _pivot.localScale;
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
            var scale = Vector3.Lerp(_defaultScale, _targetScale, offset);
            _pivot.localScale = scale;
        }

        private float GetTimeFromPeriodType() => _type == PeriodType.Oscillator
            ? Mathf.PingPong(Time.time * _speed, 1)
            : Mathf.Repeat(Time.time * _speed, 1);
    }
}