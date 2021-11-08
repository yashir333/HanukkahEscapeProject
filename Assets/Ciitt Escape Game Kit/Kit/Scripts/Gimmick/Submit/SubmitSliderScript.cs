using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Submit script for sliders
    /// </summary>
    public class SubmitSliderScript : SubmitScript
    {

        /// <summary>
        /// Answer
        /// </summary>
        [SerializeField]
        [Tooltip("Answer")]
        List<NumberCharacters> m_correctAnswer = new List<NumberCharacters>();

        /// <summary>
        /// Answer
        /// </summary>
        List<NumberCharacters> m_userInputAnswer = new List<NumberCharacters>();

        /// <summary>
        /// Awake
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Awake()
        {

            base.Awake();

            for (int i = this.m_correctAnswer.Count - 1; i >= 0; i--)
            {
                this.m_userInputAnswer.Add(NumberCharacters._0);
            }

        }

        /// <summary>
        /// Start
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void Start()
        {

            base.Start();

#if UNITY_EDITOR

            if (this.m_correctAnswer.Count <= 0)
            {
                Debug.LogError("m_correctAnswer.Count <= 0 : " + Funcs.createHierarchyPath(this.transform));
            }

#endif

        }

        /// <summary>
        /// Check answer
        /// </summary>
        // ----------------------------------------------------------------------------------
        protected override void checkAnswer()
        {

            int size = Mathf.Min(this.m_correctAnswer.Count, this.m_userInputAnswer.Count);

            if (size <= 0)
            {
                return;
            }

            // -----------------------

            for (int i = 0; i < size; i++)
            {

                // not correct
                if (this.m_correctAnswer[i] != this.m_userInputAnswer[i])
                {

                    // changeAndResumeColorWithInvalidAnswerSe
                    {
                        this.changeAndResumeColorWithInvalidAnswerSe();
                    }

                    return;

                }

            }

            // correct
            {
                this.unlockByCorrectAnswer(true);
            }

        }

        /// <summary>
        /// Set user input
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="val">NumberCharacters</param>
        // ----------------------------------------------------------------------------------
        public void setUserInput(int index, NumberCharacters val)
        {

            if (index >= 0 && index < this.m_userInputAnswer.Count)
            {
                this.m_userInputAnswer[index] = val;
            }

        }

    }

}
