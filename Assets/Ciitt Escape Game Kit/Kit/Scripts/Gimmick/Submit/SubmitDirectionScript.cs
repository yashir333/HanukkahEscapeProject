using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Submit direction input
    /// </summary>
    public class SubmitDirectionScript : SubmitScript
    {

        /// <summary>
        /// Answer
        /// </summary>
        [SerializeField]
        [Tooltip("Answer")]
        List<DirectionEnum> m_correctAnswer = new List<DirectionEnum>();

        /// <summary>
        /// Answer
        /// </summary>
        List<DirectionEnum> m_userInputAnswer = new List<DirectionEnum>();

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

            if(this.m_correctAnswer.Count <= 0)
            {
                return;
            }

            // -----------------------

            // not correct
            if (this.m_correctAnswer.Count != this.m_userInputAnswer.Count)
            {

                // changeAndResumeColorWithInvalidAnswerSe
                {
                    this.changeAndResumeColorWithInvalidAnswerSe();
                }

                return;

            }

            // -----------------------

            int size = this.m_userInputAnswer.Count;

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
        /// Add user input
        /// </summary>
        /// <param name="val">DirectionEnum</param>
        // ----------------------------------------------------------------------------------
        public void addUserInput(DirectionEnum val)
        {

            if (this.m_userInputAnswer.Count > 0 && this.m_userInputAnswer.Count >= this.m_correctAnswer.Count)
            {
                this.m_userInputAnswer.RemoveAt(0);
            }

            this.m_userInputAnswer.Add(val);

        }

    }

}
