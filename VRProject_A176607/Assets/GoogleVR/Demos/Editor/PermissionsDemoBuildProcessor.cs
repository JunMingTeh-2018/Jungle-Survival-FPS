//-----------------------------------------------------------------------
// <copyright file="PermissionsDemoBuildProcessor.cs" company="Google Inc.">
// Copyright 2017 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

// Only invoke custom build processor when building for Android.
#if UNITY_ANDROID
namespace GoogleVR.Demos
{
    using System;
    using UnityEditor;
    using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
    using UnityEditor.Build.Reporting;
#endif
    using UnityEditorInternal.VR;

#if UNITY_2018_1_OR_NEWER
    internal class PermissionsDemoBuildProcessor
        : IPreprocessBuildWithReport, IPostprocessBuildWithReport
#else
    internal class PermissionsDemoBuildProcessor : IPreprocessBuild, IPostprocessBuild
#endif
    {
        private const string SCENE_NAME_PERMISSIONS_DEMO = "PermissionsDemo";

        private bool cardboardAddedFromCode = false;

        public int callbackOrder
        {
            get { return 0; }
        }

#if UNITY_2018_1_OR_NEWER
        public void OnPreprocessBuild(BuildReport report)
        {
            OnPreprocessBuild(report.summary.platform, report.summary.outputPath);
        }
#endif

        /// <summary>A build preprocess operation for this module.</summary>
        /// <remarks><para>
        /// OnPreprocessBuild() is called right before the build process begins. If it detects that
        /// the first enabled scene in the build arrays is the PermissionsDemo, and Daydream is in
        /// the VR SDKs, it will add Cardboard to the VR SDKs. This is done because the
        /// PermissionsDemo needs a perm statement in the Manifest.  Other demos do not need this.
        /// </para><para>
        /// Adding Cardboard to VR SDKs will merge in the Manifest-Cardboard which has perm
        /// statement in it.
        /// </para></remarks>
        /// <param name="target">The BuildTarget to build.</param>
        /// <param name="path">The path to the BuildTarget.</param>
        public void OnPreprocessBuild(BuildTarget target, string path)
        {
            cardboardAddedFromCode = false;

            //string[] androidVrSDKs =
                //VREditor.GetVREnabledDevicesOnTargetGroup(BuildTargetGroup.Android);

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

            // See if PermissionsDemo is the first enabled scene in the array of scenes to build.
            for (int i = 0; i < scenes.Length; i++)
            {
                if (scenes[i].path.Contains(SCENE_NAME_PERMISSIONS_DEMO))
                {
                    if (!scenes[i].enabled)
                    {
                        return;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (scenes[i].enabled)
                    {
                        return;
                    }
                }
            }
        }

#if UNITY_2018_1_OR_NEWER
        /// @cond
        /// <summary>A build postprocess operation for this module.</summary>
        /// <param name="report">A report of the completed build.</param>
        public void OnPostprocessBuild(BuildReport report)
        {
            OnPostprocessBuild(report.summary.platform, report.summary.outputPath);
        }

        /// @endcond
#endif

        /// @cond
        /// <summary>A build postprocess operation for this module.</summary>
        /// <remarks>
        /// OnPostprocessBuild() is called after the build process. It does appropriate cleanup
        /// so that this script only affects build process for PermissionsDemo, not others.
        /// </remarks>
        /// <param name="target">The BuildTarget to run postprocess operations on.</param>
        /// <param name="path">The path to the BuildTarget.</param>
        public void OnPostprocessBuild(BuildTarget target, string path)
        {
            if (!cardboardAddedFromCode)
            {
                return;
            }
        }

        /// @endcond
    }
}
#endif  // UNITY_ANDROID
