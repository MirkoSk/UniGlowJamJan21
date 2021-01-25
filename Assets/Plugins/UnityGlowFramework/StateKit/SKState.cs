using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UniGlow.Utility.StateKit
{
	public abstract class SKState<T>
	{
        #region Variable Definitions
        protected SKStateMachine<T> _machine;
		protected T _context;
        #endregion

        public SKState() { }

		

		internal void SetMachineAndContext( SKStateMachine<T> machine, T context )
		{
			_machine = machine;
			_context = context;
			OnInitialized();
		}



		/// <summary>
		/// called directly after the machine and context are set allowing the state to do any required setup
		/// </summary>
		public virtual void OnInitialized() { }


        public abstract void OnStateEntered();
		
		
		public abstract void Update(float deltaTime);


        public abstract void OnStateExited();
	
	}
}