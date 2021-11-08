#pragma warning disable 0618

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SSC
{

    /// <summary>
    /// Class for AssetBundle startup
    /// </summary>
    public partial class AssetBundleStartupManager : SingletonMonoBehaviour<AssetBundleStartupManager>
    {

        /// <summary>
        /// Add all dependencies
        /// </summary>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator addAllDependencies()
        {
            
            if (!this.m_manifestInfo.manifest || this.hasError())
            {
                yield break;
            }

            // ----------------
            
            // add to m_dependencies
            {

                foreach (var group in this.m_absList)
                {
                    
                    foreach (var dependencyNameDotVariant in this.m_manifestInfo.manifest.GetAllDependencies(group.Value.nameDotVariant))
                    {

                        if (!this.m_dependencies.ContainsKey(dependencyNameDotVariant))
                        {

                            if (group.Value is AbStartupContentsGroupWww)
                            {
                                this.m_dependencies.Add(dependencyNameDotVariant, new AbStartupContentsGroupWww(dependencyNameDotVariant));
                                this.m_dependencies[dependencyNameDotVariant].absList.Add(new AbStartupContentsWww());
                            }

                            else if (group.Value is AbStartupContentsGroupUwr)
                            {
                                this.m_dependencies.Add(dependencyNameDotVariant, new AbStartupContentsGroupUwr(dependencyNameDotVariant));
                                this.m_dependencies[dependencyNameDotVariant].absList.Add(new AbStartupContentsUwr());
                            }

#if UNITY_EDITOR
                            else
                            {
                                Debug.LogWarning("TODO");
                            }
#endif

                        }

                    }

                }

            }

        }

        /// <summary>
        /// Add all dependencies
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="nameDotVariant">name.variant</param>
        /// <returns>IEnumerator</returns>
        // -------------------------------------------------------------------------------------------------------
        protected IEnumerator addAllDependencies(Type type, string nameDotVariant)
        {

            if (!this.m_manifestInfo.manifest || this.hasError())
            {
                yield break;
            }

            // ----------------

            // add to m_dependencies
            {

                foreach (var dependencyNameDotVariant in this.m_manifestInfo.manifest.GetAllDependencies(nameDotVariant))
                {

                    if (!this.m_dependencies.ContainsKey(dependencyNameDotVariant))
                    {

                        if (type == typeof(AbStartupContentsGroupWww))
                        {
                            this.m_dependencies.Add(dependencyNameDotVariant, new AbStartupContentsGroupWww(dependencyNameDotVariant));
                            this.m_dependencies[dependencyNameDotVariant].absList.Add(new AbStartupContentsWww());
                        }

                        else if (type == typeof(AbStartupContentsGroupUwr))
                        {
                            this.m_dependencies.Add(dependencyNameDotVariant, new AbStartupContentsGroupUwr(dependencyNameDotVariant));
                            this.m_dependencies[dependencyNameDotVariant].absList.Add(new AbStartupContentsUwr());
                        }

#if UNITY_EDITOR
                        else
                        {
                            Debug.LogWarning("TODO");
                        }
#endif

                    }

                }

            }

        }

    }

}
