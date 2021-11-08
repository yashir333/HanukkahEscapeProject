using System;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// UserProgressDataSignal class
    /// </summary>
    public class UserProgressDataSignal : SSC.SReduxStateBase<UserProgressDataSignal>
    {

        /// <summary>
        /// Add data function
        /// </summary>
        public Action<string, string> addDataAction = null;

        /// <summary>
        /// Callback action
        /// </summary>
        public Func<string, string> getDataFunc = null;

        /// <summary>
        /// Send create signal
        /// </summary>
        /// <param name="watcher">watcher</param>
        /// <param name="_addDataAction">addDataAction</param>
        // ---------------------------------------------------------------------------------------------------------------
        public void sendCreateSignal(SSC.StateWatcher<UserProgressDataSignal> watcher, Action<string, string> _addDataAction)
        {

            this.addDataAction = _addDataAction;
            this.getDataFunc = null;

            watcher.sendState();

        }

        /// <summary>
        /// Reset
        /// </summary>
        // ---------------------------------------------------------------------------------------------------------------
        public override void resetOnSceneLevelLoaded()
        {
            this.addDataAction = null;
            this.getDataFunc = null;
        }

    }

}