using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Platformer.Settings.UI
{
    public class UIState_MeasurementSetting : UIState
    {
        #region UI Elements
        [Header("Measurement UI Elements")]
        [Header("Central Axis")]
        [SerializeField] private Slider _minWidth;
        [SerializeField] private TMP_InputField _minWidthInputField;

        [SerializeField] private Slider _maxWidth;
        [SerializeField] private TMP_InputField _maxWidthInputField;

        [SerializeField] private Slider _normalized;
        [SerializeField] private TMP_InputField _normalizedInputField;

        [Header("Bounds")]
        [SerializeField] private Slider _boundsMinWidth;
        [SerializeField] private TMP_InputField _boundsMinWidthInputField;

        [SerializeField] private Slider _boundsMaxWidth;
        [SerializeField] private TMP_InputField _boundsMaxWidthInputField;

        [SerializeField] private Slider _separationWidth;
        [SerializeField] private TMP_InputField _separationWidthInputField;

        [SerializeField] private Slider _boundsNormalized;
        [SerializeField] private TMP_InputField _boundsNormalizedInputField;

        [Header("Extras")]
        [SerializeField] private Toggle _showReferenceToggle;
        [SerializeField] private Toggle _hideMesh;
        [SerializeField] private Button _resetToDefaultButton;
        #endregion

        public override event UIObserver OnUIStateChanged;

        public override void Initialize()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFrame()
        {
            throw new System.NotImplementedException();
        }

        protected override void ApplyToUI()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnUIInteracted()
        {
            throw new System.NotImplementedException();
        }

        protected override void SubscribeToUIElements()
        {
            throw new System.NotImplementedException();
        }

        protected override void UnsubscribeFromUIElements()
        {
            throw new System.NotImplementedException();
        }
    }
}
