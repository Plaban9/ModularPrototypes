using ModularPrototypes.Platformer;
using ModularPrototypes.Platformer.Data;

using System.Collections.Generic;

using UnityEngine;

namespace ModularPrototypes.Platformer.UI.StateMachine
{
    public class UIStateMachine : MonoBehaviour
    {
        [SerializeField] private PlatformTransformationSettings.TransformDomain _initialState;
        [SerializeField] private List<UIState> _uiStatesList;
        [SerializeField] private Dictionary<PlatformTransformationSettings.TransformDomain, UIState> _uiStatesDictionary;

        #region Events and Callbacks
        public delegate void UIObserver(PlatformTransformationSettings.TransformDomain domain, PlatformConfig platformConfig);
        public event UIObserver OnUIStateChanged;
        #endregion

        public void Initialize(PlatformTransformationSettings.TransformDomain bulletPattern)
        {
            _initialState = bulletPattern;

            if (_uiStatesList.Count == 0)
            {
                D("State Machine with no states!", isError: true);
                return;
            }

            foreach (var state in _uiStatesList)
            {
                state.Initialize();

                _uiStatesDictionary ??= new Dictionary<PlatformTransformationSettings.TransformDomain, UIState>();

                _uiStatesDictionary.Add(state.GetTransformDomain(), state);
            }

            TransitionTo(_initialState);
        }

        public UIState CurrentState
        {
            get; private set;
        }

        public void TransitionTo(PlatformTransformationSettings.TransformDomain newPattern)
        {
            var newState = _uiStatesDictionary[newPattern];

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

        private void OnUIInteracted(PlatformTransformationSettings.TransformDomain bulletPattern, PlatformConfig platformConfig)
        {
            OnUIStateChanged?.Invoke(bulletPattern, platformConfig);
        }

        #region DEBUG
        private void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIStateMachine>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
        #endregion
    }
}
