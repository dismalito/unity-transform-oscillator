using UnityEngine;

namespace TransformOscillator.Scripts
{
    public class PositionOscillator : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] internal Vector3 _offsetPoint = new Vector3(1, 0, 0);
        [SerializeField] internal TransformOscillatorConstraint _constraints;
        [SerializeField] private float _speed = 1f;
        [SerializeField] private OscillatorType _type;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private bool _playOnStart = true;

        private bool _activated = false;
        private bool _oppositeDirection = false;

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

            var time = GetTimeFromMovementType();
            var offset = _animationCurve.Evaluate(time);
            var position = Vector3.Lerp(TargetPoint, OppositeTargetPoint, offset);
            _pivot.position = position;
        }

        private float GetTimeFromMovementType() => _type == OscillatorType.Oscillator
            ? Mathf.PingPong(Time.time * _speed, 1)
            : Mathf.Repeat(Time.time * _speed, 1);

        private Vector3 TargetPoint => _offsetPoint + transform.position;
        private Vector3 OppositeTargetPoint => _offsetPoint * -1 + transform.position;
    }
}