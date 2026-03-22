using System.Collections.Generic;

using UnityEngine;

namespace ModularPrototypes.BulletHell.UI.StateMachine
{
    public class UIStateMachine : MonoBehaviour
    {
        [SerializeField] private BulletPattern _initialState;
        [SerializeField] private List<UIState> _uiStatesList;
        [SerializeField] private Dictionary<BulletPattern, UIState> _uiStatesDictionary;

        public void Initialize(BulletPattern bulletPattern)
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

                _uiStatesDictionary ??= new Dictionary<BulletPattern, UIState>();

                _uiStatesDictionary.Add(state.GetBulletPattern(), state);
            }        

            TransitionTo(_initialState);
        }

        public UIState CurrentState
        {
            get; private set;
        }

        public void TransitionTo(BulletPattern newPattern)
        {
            var newState = _uiStatesDictionary[newPattern];

            if (newState == null)
            {
                D("Trying to transition into null state!", isError: true);
                return;
            }

            if (CurrentState != null)
            {
                CurrentState.OnExit();
            }

            D($"Trying from state {(CurrentState == null ? "Empty" : CurrentState.GetStateName() )} to {newState.GetStateName()}.");
            CurrentState = newState;

            if (CurrentState != null)
            {
                CurrentState.OnEnter();
            }
        }

        #region DEBUG
        private void D(string message, bool isError = false)
        {
            DebugUtils.DebugInfo.Print($"<<UIStateMachine>> {message}", isError ? DebugUtils.DebugConstants.ERROR : DebugUtils.DebugConstants.INFO);
        }
        #endregion
    }
}
