using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace ModularPrototypes.Platformer.UI.StateMachine.States
{
    public class UIState_Translation : UIState
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Dropdown _axisDropdown;
        [SerializeField] private Slider _periodSlider;
        [SerializeField] private TMP_InputField _periodInputField;

        [Header("Per Axis UI Elements")]
        //TODO: Group and use DKV for X, Y, Z data
        [SerializeField] private UIAxisElements _xAxisElements;
        [SerializeField] private UIAxisElements _yAxisElements;
        [SerializeField] private UIAxisElements _zAxisElements;



        public override event UIObserver OnUIStateChanged;

        public override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        protected override void ApplyToUI()
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

        protected override void OnUIInteracted()
        {
            throw new System.NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFrame()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}
