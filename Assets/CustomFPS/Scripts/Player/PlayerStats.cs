/// DarkTreeDevelopment (2019) DarkTree FPS v1.21
/// If you have any questions feel free to write me at email --- darktreedevelopment@gmail.com ---
/// Thanks for purchasing my asset!

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DarkTreeFPS
{
    /// <summary>
    /// It's very simple class so I think that there is no need in deep explanation of it
    /// When health less than or equal to zero -> Death() || if respawn -> Respawn()
    /// Death and Respawn methods works in same way but in oposite direction
    /// </summary>

    public class PlayerStats : MonoBehaviour, IDamageable
    {
        [Header("Health")]
        public bool isGod = false;
        [Tooltip("Player's health")]
        public int health = 100;
        [Tooltip("UI element to draw health as number")]
        public Text healthUIText;
        [Tooltip("UI element to draw health as slider")]
        public Slider healthUISlider;

        public float minFailingDamage = 10;

        [Header("Damage effect")]
        [Tooltip("UI Image with fullscreen hit fx")]
        public Image damageScreenFX;
        [Tooltip("UI Image color to change on hit")]
        public Color damageScreenColor;
        [Tooltip("UI Image fade speed after hit")]
        public float damageScreenFadeSpeed = 1.4f;

        private Color damageScreenColor_temp;

        public static bool isPlayerDead = false;

        [HideInInspector]
        public Vector3 playerPosition;
        [HideInInspector]
        public Quaternion playerRotation;

        private AudioSource audioSource;
        public AudioClip[] painClips;
        public AudioClip breathingSound;

        private GameObject playerBody;

        #region utility objects
        private Rigidbody playerRigidbody;
        private FPSController controller;
        private CapsuleCollider playerCollider;
        private Sway sway;

        private Transform cameraHolder;

        private WeaponManager weaponManager;
        //Don't create any rigidbody here
        private Rigidbody rigidbody_temp;
        #endregion
        
        private void Start()
        {
            cameraHolder = GameObject.Find("Camera Holder").GetComponent<Transform>();

            isPlayerDead = false;

            audioSource = GetComponent<AudioSource>();

            playerRigidbody = GetComponent<Rigidbody>();
            controller = GetComponent<FPSController>();
            playerCollider = GetComponent<CapsuleCollider>();
            weaponManager = FindObjectOfType<WeaponManager>();
            sway = FindObjectOfType<Sway>();

            if(!InputManager.useMobileInput)
            playerBody = FindObjectOfType<Body>().gameObject;
        }
        
        void Update()
        {
            if (isPlayerDead)
            {
                weaponManager.gameObject.SetActive(false);
            }

            if (health == 0 && !isPlayerDead && !isGod)
            {
                PlayerDeath();
            }

            if (health < 0)
            {
                health = 0;
            }

            if(health >  100)
            {
                health = 100;
            }

            WritePlayerTransform();
            DrawHealthStats();
        }

        public void ApplyDamage(int damage)
        {
            if (isPlayerDead)
                return;

            if (damage > 0)
            {
                if(audioSource != null && painClips.Length > 0)
                {
                    audioSource.PlayOneShot(painClips[Random.Range(0, painClips.Length)]);
                }

                health -= damage;
                damageScreenFX.color = damageScreenColor;
                damageScreenColor_temp = damageScreenColor;
                StartCoroutine(HitFX());
            }
        }

        public void Damage(int damage) {
            if (isPlayerDead)
                return;

            if (damage > 0) {
                if (audioSource != null && painClips.Length > 0) {
                    audioSource.PlayOneShot(painClips[Random.Range(0, painClips.Length)]);
                }

                health -= damage;
                damageScreenFX.color = damageScreenColor;
                damageScreenColor_temp = damageScreenColor;
                StartCoroutine(HitFX());
            }
        }

        public void AddHealth(int hp)
        {
            health += hp;
        }

        IEnumerator HitFX()
        {
            while (damageScreenFX.color.a > 0)
            {
                damageScreenColor_temp = new Color(damageScreenColor_temp.r, damageScreenColor_temp.g, damageScreenColor_temp.b, damageScreenColor_temp.a -= damageScreenFadeSpeed * Time.deltaTime);
                damageScreenFX.color = damageScreenColor_temp;

                yield return new WaitForEndOfFrame();
            }
        }

        public void PlayerDeath()
        {
            if (!isPlayerDead)
            {
                sway.enabled = false;
                var leanController = FindObjectOfType<Lean>().enabled = false;
                controller.enabled = false;
                
                if(playerBody != null && !InputManager.useMobileInput)
                {
                    playerBody.SetActive(false);
                }

                if (!rigidbody_temp)
                {
                    rigidbody_temp = cameraHolder.gameObject.AddComponent<Rigidbody>();
                    rigidbody_temp.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    rigidbody_temp.mass = 10;
                }

                Camera.main.GetComponent<Animator>().enabled = false;
                Destroy(Camera.main.GetComponent<Rigidbody>());

                cameraHolder.parent = null;
                cameraHolder.GetComponent<BoxCollider>().enabled = true;

                rigidbody_temp.AddForce(cameraHolder.transform.forward * 5, ForceMode.VelocityChange);
                rigidbody_temp.constraints = RigidbodyConstraints.FreezeRotation;
                

                weaponManager.HideWeaponOnDeath();
                
                controller.lockCursor = false;

                isPlayerDead = true;
                GameManager.Instance.GameOver();
            }
            else
                return;
        }

        public void RespawnPlayer()
        {
            isPlayerDead = false;

            FindObjectOfType<FPSController>().mainCameraAnimator.gameObject.SetActive(true);

            sway.enabled = true;
            var leanController = FindObjectOfType<Lean>().enabled = true;
            controller.enabled = true;
            
            if (playerBody != null && !InputManager.useMobileInput)
            {
                playerBody.SetActive(true);
            }
            
            controller.lockCursor = false;
        }
        
        void WritePlayerTransform()
        {
            playerPosition = gameObject.transform.position;
            playerRotation = gameObject.transform.rotation;
        }

        void DrawHealthStats()
        {
            if (healthUIText != null)
                healthUIText.text = health.ToString();

            if (healthUISlider != null)
                healthUISlider.value = health;
        }
    }

}