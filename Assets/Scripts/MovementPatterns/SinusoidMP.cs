using MyBox;
using Spawnables;
using UnityEngine;

namespace MovementPatterns
{
    /// <summary>
    /// Produces and follows a sine wave shaped path. The sine wave can be rotated by specifying the axis of oscillation
    /// </summary>
    public class SinusoidMP : MovementPattern
    {
        /* ------------------- Inspector Variables ------------------- */
        
        /// <summary>
        /// if true, produces oscillating movement along a straight line, perpendicular to the given axis of oscillation.
        /// </summary>
        [SerializeField] private bool staysInLine;
        
        /// <summary>
        /// Rate of change of the sine function's argument
        /// </summary>
        [SerializeField] private float speed;
        
        /// <summary>
        /// Amplitude of the sine wave
        /// </summary>
        [SerializeField] private float amplitude;
        
        /// <summary>
        /// Frequency of the sine wave
        /// </summary>
        [SerializeField] private float frequency;
        
        /// <summary>
        /// Phase of the sine wave
        /// </summary>
        [SerializeField] private float phase;
        
        /// <summary>
        /// Rate of change of speed
        /// </summary>
        [Separator]
        [SerializeField] private float acceleration;
        
        /// <summary>
        /// Rate of change of amplitude
        /// </summary>
        [SerializeField] private float amplitudeChange;
        
        /// <summary>
        /// Rate of change of frequency
        /// </summary>
        [SerializeField] private float frequencyChange;
        
        /// <summary>
        /// Rate of change of phase
        /// </summary>
        [SerializeField] private float phaseShift;
        
        /// <summary>
        /// Rotation of the axis of oscillation in degrees, where 0 is up
        /// </summary>
        [Separator]
        [SerializeField] private float axisRotation;
        
        /* ---------------------- Private Fields ---------------------- */
        
        // The direction of the axis of oscillation represented as a unit vector
        private Vector3 _axisDirection;
        
        // Normal vector to the axis of oscillation
        private Vector3 _axisNormal;
        
        // The position of the entity at the start of the movement. 
        private Vector3 _origin;
        
        /* ---------------------- Public Methods ---------------------- */

        public override void Initialize(Entity entity)
        {
            OnValidate();
            _origin = entity.transform.position;
        }
        
        
        public override Vector3 GetNextPosition(Entity entity)
        {
            float time = entity.MPLifeTime;
            
            // Actual calculation is done in a separate method so it can be simulated and drawn with Gizmos
            return GetNextPosition(time);
        }
        
        /* ---------------------- Private Methods ---------------------- */

        //
        private Vector3 GetNextPosition(float time)
        {
            float currentSpeed = speed + acceleration * time;
            float currentAmplitude = amplitude + amplitudeChange * time;
            float currentFrequency = frequency + frequencyChange * time;
            float currentPhase = (phase + phaseShift * Time.time) % 360;
            
            float distance = currentSpeed * time;
            
            Vector3 axisPosition = !staysInLine ?
                GetAxisPosition(distance) :
                _origin;

            Vector3 sinusoidPosition = GetSinusoidPosition(distance, currentAmplitude, currentFrequency, currentPhase);
            
            return axisPosition + sinusoidPosition;
        }
        
        // Returns the position of the axis at the given distance from the origin
        private Vector3 GetAxisPosition(float distance)
        {
            return _origin + _axisDirection * distance;
        }
        
        // Sine function, where the result is represented as a vector pointing perpendicular to the axis of oscillation
        private Vector3 GetSinusoidPosition(float t, float amplitude, float frequency, float phase)
        {
            phase *= Mathf.Deg2Rad;
            float sineValue = amplitude * Mathf.Sin(frequency * t + phase);
            return _axisNormal * sineValue;
        }
        
        /* ---------------------- Editor-only Callbacks ---------------------- */
        // Updates axes from the inspector values
        void OnValidate()
        {
            _origin = transform.position;
            _axisDirection = Quaternion.Euler(0, 0, axisRotation) * Vector3.up;
            _axisDirection = _axisDirection.normalized;
            _axisNormal = Quaternion.Euler(0, 0, -90) * _axisDirection;
            _axisNormal = -_axisNormal.normalized;
        }
        
        // Draws the trajectory and the axes of the movement
        private void OnDrawGizmosSelected()
        {
            float precalculatedSeconds = 5;
            float timeStep = 0.01f;
            
            GizmoDrawAxes(precalculatedSeconds);
            GizmoDrawTrajectory(precalculatedSeconds, timeStep);
        }

        // Draws the axis of oscillation and its normal vector
        private void GizmoDrawAxes(float timeFrame)
        {
            Gizmos.color = Color.blue;
            
            Vector3 position = transform.position;
            Vector3 line = _axisDirection * timeFrame * speed;
            Gizmos.DrawLine(position, position + line);
            Gizmos.DrawLine(position, position + _axisNormal * 3);
        }
        
        // Draws the sine wave path of the entity
        private void GizmoDrawTrajectory(float timeFrame, float timeStep)
        {
            Gizmos.color = Color.green;
            
            Vector3 newPosition = transform.position;
            Vector3 lastPosition = newPosition;
            
            for (float t = 0; t < timeFrame; t += timeStep)
            {
                newPosition = GetNextPosition(t);
                Gizmos.DrawLine(lastPosition, newPosition);
                lastPosition = newPosition;
            }
        }
    }
}
