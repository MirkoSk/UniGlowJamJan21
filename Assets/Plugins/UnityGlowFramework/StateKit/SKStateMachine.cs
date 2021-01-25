using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


namespace UniGlow.Utility.StateKit
{
	public sealed class SKStateMachine<T>
	{
        #region Variable Declarations
        // Public
        public event Action onStateChanged;

        // Private
        T context;
        Dictionary<System.Type, SKState<T>> states = new Dictionary<System.Type, SKState<T>>();
		SKState<T> currentState;
        SKState<T> previousState;
        float elapsedTimeInState = 0f;
        #endregion



        #region Public Properties
        public SKState<T> CurrentState { get { return currentState; } }

        public SKState<T> PreviousState { get => previousState; }
        #endregion



        #region Public Functions
        public SKStateMachine( T context, SKState<T> initialState )
		{
			this.context = context;

			// setup our initial state
			AddState( initialState );
			currentState = initialState;
			currentState.OnStateEntered();
		}


		/// <summary>
		/// adds the state to the machine
		/// </summary>
		public void AddState( SKState<T> state )
		{
			state.SetMachineAndContext( this, context );
			states[state.GetType()] = state;
		}


		/// <summary>
		/// ticks the state machine with the provided delta time
		/// </summary>
		public void Update( float deltaTime )
		{
			elapsedTimeInState += deltaTime;
			currentState.Update( deltaTime );
		}


		/// <summary>
		/// changes the current state
		/// </summary>
		public R ChangeState<R>() where R : SKState<T>
		{
			// avoid changing to the same state
			var newType = typeof( R );
			if( currentState.GetType() == newType )
				return currentState as R;

			// only call end if we have a currentState
			if( currentState != null )
				currentState.OnStateExited();

			#if UNITY_EDITOR
			// do a sanity check while in the editor to ensure we have the given state in our state list
			if( !states.ContainsKey( newType ) )
			{
				var error = GetType() + ": state " + newType + " does not exist. Did you forget to add it by calling addState?";
				Debug.LogError( error );
				throw new Exception( error );
			}
			#endif

			// swap states and call begin
			previousState = currentState;
			currentState = states[newType];
			currentState.OnStateEntered();
			elapsedTimeInState = 0f;

			// fire the changed event if we have a listener
			if( onStateChanged != null )
				onStateChanged();

			return currentState as R;
		}
        #endregion
    }
}