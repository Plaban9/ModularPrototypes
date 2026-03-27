using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Platformer.UI.StateMachine
{
    [System.Serializable]
    public class UIAxisElements
    {
        [SerializeField] private TMP_Dropdown _functionDropdown;
        [SerializeField] private Slider _amplitudeSlider;
        [SerializeField] private TMP_InputField _amplitudeInputField;
        [SerializeField] private Toggle _moduloToggle;
        [SerializeField] private Toggle _negateToggle;

        #region Getters
        public TMP_Dropdown FunctionDropdown => _functionDropdown;
        public Slider AmplitudeSlider => _amplitudeSlider;
        public TMP_InputField AmplitudeInputField => _amplitudeInputField;
        public Toggle ModuloToggle => _moduloToggle;
        public Toggle NegateToggle => _negateToggle;
        #endregion
    }
}
