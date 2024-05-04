using System.Collections;
using PlayerScripts;
using UnityEngine;

namespace Spawnables.Weapons
{
    /// <summary>
    /// A lazer beam damaging the player or entities by shooting raycasts, visualized by a LineRenderer.
    /// The lazer has 3 phases: Cooldown, Telegraph and Release. The beam deals damage only during the Release phase.
    /// </summary>
    [RequireComponent(typeof(LineRenderer))]
    public class Lazer : MonoBehaviour, IShootable
    {
        /* --------------------- Constant --------------------- */
        
        private const float MAX_LENGTH = 50;                        // Maximum length of the LineRenderer
        private const string HITBOX_LAYER_NAME = "PlayerHitbox";    // Name of the layer containing the player hitbox
        
        /* ------------------ Public Fields ------------------- */
        
        /// <summary>
        /// Duration od the Cooldown phase in seconds
        /// </summary>
        public float cooldown;
        
        /// <summary>
        /// Duration of the Telegraph phase in seconds
        /// </summary>
        public float telegraphDuration;
        
        /// <summary>
        /// Duration of the Release phase in seconds
        /// </summary>
        public float releaseDuration;

        /// <summary>
        /// Material of the LineRenderer during the Telegraph phase
        /// </summary>
        public Material telegraphMaterial;
        
        /// <summary>
        /// Mateiral of the LineRenderer during the Release phase
        /// </summary>
        public Material releaseMaterial;
        
        /// <summary>
        /// Width of the LineRenderer during the Telegraph phase
        /// </summary>
        public float telegraphWidth;
        
        /// <summary>
        /// Width of the LineRenderer during the Release phase
        /// </summary>
        public float releaseWidth;
        
        /* ------------------ Private Fields ------------------ */
        
        private LineRenderer _lineRenderer;         // Visualizes the lazer beam
        private int _playerHitboxLayer;             // Layer mask index containing the player hitbox

        private bool _emitting;                     // True if the lazer is currently in the Telegraph or Release phase
        private float _timeOfLastShot;              // Time in seconds when the lazer was last shot  
        
        /* ------------------ Unity Callbacks ----------------- */
        
        void Start()
        {
            _timeOfLastShot = Time.time - cooldown;
            
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.enabled = false;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.SetPosition(0, transform.position);
            
            _playerHitboxLayer = 1 << LayerMask.NameToLayer(HITBOX_LAYER_NAME);
        } 

        /* ------------------ Public Methods ------------------ */
        
        // Starts the Telegraph phase when the spent enough time in the Cooldown phase
        public void OnShoot()
        {
            if (_emitting) return;
            if (Time.time - _timeOfLastShot < cooldown) return;
            
            StartCoroutine(Telegraph());
            _emitting = true;
        }
        
        /* ------------------ Private Methods ----------------- */

        // Coroutine for the Telegraph phase, beam is visualized but not active
        private IEnumerator Telegraph()
        {
            _lineRenderer.enabled = true;
            ChangeAppearance(telegraphMaterial, telegraphWidth);
            float timeOfTelegraphEnd = Time.time + telegraphDuration;
            
            while (Time.time < timeOfTelegraphEnd)
            {
                UpdatePosition();
                yield return null;
            }
            
            StartCoroutine(Release());
        }

        // Coroutine for the Release phase, beam is active and deals damage
        private IEnumerator Release()
        {
            ChangeAppearance(releaseMaterial, releaseWidth);
            float timeOfReleaseEnd = Time.time + releaseDuration;

            while (Time.time < timeOfReleaseEnd)
            {
                UpdatePosition();
                CheckDamages();
                yield return null;
            }

            _lineRenderer.enabled = false;
            _timeOfLastShot = Time.time;
            _emitting = false;
        }

        // Set the start and end position of the LineRenderer
        private void UpdatePosition()
        {
            Vector3 startPoint = transform.position;
            _lineRenderer.SetPosition(0, startPoint);

            Vector3 endPoint = startPoint + transform.rotation * Vector3.up * MAX_LENGTH;
            _lineRenderer.SetPosition(1, endPoint);
        }

        // Use Box raycast to check if the beam hits the player
        private void CheckDamages()
        {
            Vector2 origin = transform.position;
            Vector2 size = new Vector2(releaseWidth, releaseWidth) / 2;
            Vector2 direction = transform.rotation * Vector2.up;
            
            RaycastHit2D hitInfo = Physics2D.BoxCast(origin, size, 0, direction, MAX_LENGTH, _playerHitboxLayer);
            
            PlayerHitbox playerHitbox = hitInfo.collider?.GetComponent<PlayerHitbox>();
            if (playerHitbox == null) return;
            
            playerHitbox.AttemptHit(hitInfo.collider.gameObject);
        }

        // Change the width and material of the LineRenderer
        private void ChangeAppearance(Material material, float width)
        {
            _lineRenderer.widthMultiplier = width;
            _lineRenderer.material = material;
        }
        
        // Draw a sphere at the position of the lazer start point
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 0.05f);
        }
    }
}
