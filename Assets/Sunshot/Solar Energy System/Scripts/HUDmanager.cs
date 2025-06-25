using Sunshot.Gameplay;
using Sunshot.Generic.ObjectPool;
using Sunshot.PlayerSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sunshot.SolarEnergySystem.UI
{
    public class HUDmanager : MonoBehaviour
    {
        public static HUDmanager instance;

        [Header("Solar energy bar data")]
        [SerializeField] Image energyFillImage;
        [SerializeField] float maxEnergy = 100f;
        [SerializeField] float drainSpeed = 10f;

        [Header("Thruster energy bar data")]
        [SerializeField] Image thrusterFillImage;
        [SerializeField] float maxThruster = 100f;
        [SerializeField] float thrusterDrainSpeed = 30f;
        [SerializeField] float thrusterRegenSpeed = 20f;

        [SerializeField] GameObject gameOverPanel;
        [SerializeField] GameObject TwoXSpeedText;

        [Header("Dependencies")]
        [SerializeField] PlayerMovement playerMovement;

        private GameManager gameManager;

        private float currentEnergy;
        private bool isZeroEnergy = false;

        public event Action OnZeroEnergy;

        private float currentThruster;

        private void Awake()
        {
            if(instance == null) 
                instance = this;
            else
                Destroy(instance);
        }

        void Start()
        {
            gameManager = GameManager.instance;

            currentEnergy = maxEnergy;
            UpdateSolarEnergyBar();

            currentThruster = maxThruster;
            UpdateThrusterBar();
        }

        void Update()
        {
            DrainOverTime();
            HandleThrusterEnergy();

            if(isZeroEnergy == false && currentEnergy <= 0)
            {
                isZeroEnergy = true;
                OnZeroEnergy?.Invoke();

                gameManager.GameOver();
            }
        }

        void DrainOverTime()
        {
            bool isThrusterOn = playerMovement.GetIsThrusterOn();

            TwoXSpeedText.SetActive(isThrusterOn);

            float currentDrainSpeed = isThrusterOn ? drainSpeed * 2 : drainSpeed;

            if (currentEnergy > 0)
            {
                currentEnergy -= currentDrainSpeed * Time.deltaTime;
                currentEnergy = Mathf.Max(currentEnergy, 0f);
                UpdateSolarEnergyBar();
            }
        }

        void HandleThrusterEnergy()
        {
            bool isThrusterOn = playerMovement.GetIsThrusterOn();

            if (isThrusterOn && currentThruster > 0)
            {
                currentThruster -= thrusterDrainSpeed * Time.deltaTime;
                currentThruster = Mathf.Max(currentThruster, 0f);
            }
            else if (!isThrusterOn && currentThruster < maxThruster)
            {
                currentThruster += thrusterRegenSpeed * Time.deltaTime;
                currentThruster = Mathf.Min(currentThruster, maxThruster);
            }

            UpdateThrusterBar();
        }

        public void SetSolarEnergy(float value)
        {
            currentEnergy += value;
            currentEnergy = MathF.Min(currentEnergy, 100f);
            UpdateSolarEnergyBar();
        }

        void UpdateSolarEnergyBar()
        {
            float percent = currentEnergy / maxEnergy;
            energyFillImage.fillAmount = percent;
            energyFillImage.color = Color.Lerp(Color.red, Color.green, percent);
        }

        void UpdateThrusterBar()
        {
            float percent = currentThruster / maxThruster;
            thrusterFillImage.fillAmount = percent;
            thrusterFillImage.color = Color.Lerp(Color.red, Color.cyan, percent);
        }

        public bool CanUseThruster()
        {
            if(currentThruster > 0)
                return true;

            return false;
        }
    }
}
