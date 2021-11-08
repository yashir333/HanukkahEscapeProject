using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SSC;
using System;
using UnityEngine.SceneManagement;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Custom SimpleReduxManager
    /// </summary>
    public class CustomReduxManager : SimpleReduxManager
    {

        /// <summary>
        /// Instance
        /// </summary>
        public static CustomReduxManager CustomReduxManagerInstance { get { return (CustomReduxManager)CustomReduxManager.Instance; } }

        /// <summary>
        /// StateWatcher for MainGameSceneState
        /// </summary>
        protected StateWatcher<MainGameSceneState> m_mainGameSceneStateWatcher = new StateWatcher<MainGameSceneState>();

        /// <summary>
        /// StateWatcher for UserProgressDataSignal
        /// </summary>
        protected StateWatcher<UserProgressDataSignal> m_userProgressDataSignalWatcher = new StateWatcher<UserProgressDataSignal>();

        /// <summary>
        /// StateWatcher for ChangeLanguageSignal
        /// </summary>
        protected StateWatcher<ChangeLanguageSignal> m_changeLanguageSignalWatcher = new StateWatcher<ChangeLanguageSignal>();

        // ----------------------------------------------------------------------------------------------

        /// <summary>
        /// StateWatcher<MainGameSceneState> getter
        /// </summary>
        public StateWatcher<MainGameSceneState> MainGameSceneStateWatcher { get { return this.m_mainGameSceneStateWatcher; } }

        /// <summary>
        /// StateWatcher<UserProgressDataSignal> getter
        /// </summary>
        public StateWatcher<UserProgressDataSignal> UserProgressDataSignalWatcher { get { return this.m_userProgressDataSignalWatcher; } }

        /// <summary>
        /// StateWatcher<ChangeLanguageSignal> getter
        /// </summary>
        public StateWatcher<ChangeLanguageSignal> ChangeLanguageSignalWatcher { get { return this.m_changeLanguageSignalWatcher; } }

        // ----------------------------------------------------------------------------------------------

        /// <summary>
        /// Reset states on scene loaded
        /// </summary>
        /// <param name="scene">Scene</param>
        /// <param name="mode">LoadSceneMode</param>
        // ----------------------------------------------------------------------------------------------
        protected override void resetOnSceneLoaded(Scene scene, LoadSceneMode mode)
        {

            base.resetOnSceneLoaded(scene, mode);

            if (mode != LoadSceneMode.Single)
            {
                return;
            }

            // ---------------

            this.m_mainGameSceneStateWatcher.state().resetOnSceneLevelLoaded();
            this.m_userProgressDataSignalWatcher.state().resetOnSceneLevelLoaded();
            this.m_changeLanguageSignalWatcher.state().resetOnSceneLevelLoaded();

        }

        /// <summary>
        /// Add MainGameSceneState receiver action
        /// </summary>
        /// <param name="action">MainGameSceneState</param>
        // ----------------------------------------------------------------------------------------------
        public void addMainGameSceneStateReceiver(Action<MainGameSceneState> action)
        {

            if (action != null)
            {
                this.m_mainGameSceneStateWatcher.addAction(action);
            }

        }

        /// <summary>
        /// Add UserProgressDataSignal receiver action
        /// </summary>
        /// <param name="action">UserProgressDataSignal</param>
        // ------------------------------------------------------------------------------------------------------------------------------
        public void addUserProgressDataSignalReceiver(Action<UserProgressDataSignal> action)
        {

            if (action != null)
            {
                this.m_userProgressDataSignalWatcher.addAction(action);
            }

        }

        /// <summary>
        /// Add ChangeLanguageSignal receiver action
        /// </summary>
        /// <param name="action">ChangeLanguageSignal</param>
        // ------------------------------------------------------------------------------------------------------------------------------
        public void addChangeLanguageSignalReceiver(Action<ChangeLanguageSignal> action)
        {
            if (action != null)
            {
                this.m_changeLanguageSignalWatcher.addAction(action);
            }

        }

    }

}
