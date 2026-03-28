using System.Collections.Generic;

using UnityEngine;

namespace ModularPrototypes.Platformer.Settings.UI
{    
    public class UIStateMachine : MonoBehaviour
    {
        [SerializeField] private PlatformSettings _initialState;
        [SerializeField] private List<UIState> _uiStatesList;
        [SerializeField] private Dictionary<PlatformSettings, UIState> _uiStatesDictionary;

        #region Events and Callbacks
        public delegate void UIObserver(SettingsData data);
        public event UIObserver OnUIStateChanged;
        #endregion

        public void Initialize(PlatformSettings platformSettings)
        {
            _initialState = platformSettings;

            if (_uiStatesList.Count == 0)
            {
                D("State Machine with no states!", isError: true);
                return;
            }

            foreach (var state in _uiStatesList)
            {
                state.Initialize();

                _uiStatesDictionary ??= new Dictionary<PlatformSettings, UIState>();
                _uiStatesDictionary.Add(state.GetSettingsData().GetSettingsType(), state);
            }

            TransitionTo(_initialState);
        }

        public UIState CurrentState
        {
            get; private set;
        }

        public void TransitionTo(PlatformSettings newSettings)
        {
            var newState = _uiStatesDictionary[newSettings];

            if (newState == null)
            {
                D("Trying to transition into null state!", isError: true);
                return;
            }

            if (CurrentState != null)
            {
                CurrentState.OnUIStateChanged -= OnUIInteracted;
                CurrentState.OnExit();
            }

            D($"Trying to transition from state: {(CurrentState == null ? "Empty" : CurrentState.GetStateName())} to state: {newState.GetStateName()}.");
            CurrentState = newState;


            if (CurrentState != null)
            {
                CurrentState.OnEnter();
                CurrentState.OnUIStateChanged += OnUIInteracted;             
            }            
        }

        private void OnUIInteracted(SettingsData settingsData)
        {
            OnUIStateChanged?.Invoke(settingsData);
        }

        #region DEBUG
        private void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIStateMachine>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
        #endregion
    }
}
