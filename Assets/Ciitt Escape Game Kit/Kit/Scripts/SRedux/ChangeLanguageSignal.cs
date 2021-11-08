using System;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// ChangeLanguageSignal class
    /// </summary>
    public class ChangeLanguageSignal : SSC.SReduxStateBase<ChangeLanguageSignal>
    {

#if UNITY_EDITOR

        /// <summary>
        /// Use SystemManager.Instance.systemLanguage
        /// </summary>
        [Obsolete("Use SystemManager.Instance.systemLanguage", true)]
        public SystemLanguage language = SystemLanguage.Unknown;

#endif

        /// <summary>
        /// Set state
        /// </summary>
        /// <param name="watcher">watcher</param>
        [Obsolete("Use sendSignal", false)]
        // ---------------------------------------------------------------------------------------------------------------
        public void setState(SSC.StateWatcher<ChangeLanguageSignal> watcher)
        {
            watcher.sendState();
        }

        /// <summary>
        /// Send signal
        /// </summary>
        // ---------------------------------------------------------------------------------------------------------------
        public void sendSignal()
        {
            this.m_refWatcher.sendState();
        }

        /// <summary>
        /// Reset
        /// </summary>
        // ---------------------------------------------------------------------------------------------------------------
        public override void resetOnSceneLevelLoaded()
        {

        }

    }

}