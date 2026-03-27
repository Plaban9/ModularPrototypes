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
    }
}
