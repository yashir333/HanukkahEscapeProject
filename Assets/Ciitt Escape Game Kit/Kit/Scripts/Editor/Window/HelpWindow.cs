using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    /// <summary>
    /// Help menu for Generator
    /// </summary>
    public class HelpWindow : EditorWindow
    {

        /// <summary>
        /// Help message type
        /// </summary>
        enum HelpMessageType
        {
            Contents,
            Pdf,
            HowToPrepareYourProjectScenes,
            HowToClickAndMove,
            HowToChangeRooms,
            HowToLockAndUnlockWithoutItems,
            HowToExit,
            AboutItem,
            HowToLockAndUnlockWithItems,
            AboutGimmick,
            HowToSaveUserProgressData,
            HowToCustomizeUi,
        }

        /// <summary>
        /// type
        /// </summary>
        HelpMessageType m_type = HelpMessageType.Contents;

        /// <summary>
        /// Scroll pos
        /// </summary>
        Vector2 m_scrollPos = Vector2.zero;

        /// <summary>
        /// space
        /// </summary>
        readonly float m_space = 30.0f;

        /// <summary>
        /// Language
        /// </summary>
        SystemLanguage m_language = SystemLanguage.English;

        /// <summary>
        /// Set SystemLanguage
        /// </summary>
        /// <param name="language"></param>
        // -----------------------------------------------------------------------------------------------
        public void setLanguage(SystemLanguage language)
        {
            this.m_language = language;
        }

        /// <summary>
        /// Show back button
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void backButton()
        {

            if (GUILayout.Button("Back", GUILayout.MaxWidth(100), GUILayout.MinHeight(30)))
            {
                this.m_type = HelpMessageType.Contents;
            }

        }

        /// <summary>
        /// OnGUI Contents
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIContents()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                if (GUILayout.Button("PDF", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.Pdf;
                }

                GUILayout.Space(this.m_space);

                if (GUILayout.Button("シーンの用意", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToPrepareYourProjectScenes;
                }

                if (GUILayout.Button("クリックによる移動の仕方", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToClickAndMove;
                }

                if (GUILayout.Button("部屋の変更方法", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToChangeRooms;
                }

                if (GUILayout.Button("オブジェクトのロックとアンロック(アイテム無し)", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToLockAndUnlockWithoutItems;
                }

                if (GUILayout.Button("終了の仕方", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToExit;
                }

                if (GUILayout.Button("アイテムについて", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.AboutItem;
                }

                if (GUILayout.Button("オブジェクトのロックとアンロック(アイテム有り)", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToLockAndUnlockWithItems;
                }

                if (GUILayout.Button("ギミックについて", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.AboutGimmick;
                }

                if (GUILayout.Button("セーブ機能について", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToSaveUserProgressData;
                }

                if (GUILayout.Button("UIのカスタマイズ", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToCustomizeUi;
                }

            }

            else
            {

                if (GUILayout.Button("PDF", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.Pdf;
                }

                GUILayout.Space(this.m_space);

                if (GUILayout.Button("How to prepare your project scenes", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToPrepareYourProjectScenes;
                }

                if (GUILayout.Button("How to click and move", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToClickAndMove;
                }

                if (GUILayout.Button("How to change rooms", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToChangeRooms;
                }

                if (GUILayout.Button("How to lock and unlock objects (without items)", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToLockAndUnlockWithoutItems;
                }

                if (GUILayout.Button("How to exit", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToExit;
                }

                if (GUILayout.Button("About items", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.AboutItem;
                }

                if (GUILayout.Button("How to lock and unlock objects (with items)", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToLockAndUnlockWithItems;
                }

                if (GUILayout.Button("About gimmicks", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.AboutGimmick;
                }

                if (GUILayout.Button("How to save user progress data", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToSaveUserProgressData;
                }

                if (GUILayout.Button("How to customize UI", GUILayout.MinHeight(30)))
                {
                    this.m_type = HelpMessageType.HowToCustomizeUi;
                }

            }

        }

        /// <summary>
        /// OnGUI Pdf
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIPdf()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("Assets/Ciitt Escape Game Kit/Readme (JP)", EditorStyles.boldLabel);

            }

            else
            {

                EditorGUILayout.LabelField("Assets/Ciitt Escape Game Kit/Readme (EN)", EditorStyles.boldLabel);

            }

            GUILayout.Space(this.m_space);

        }

        /// <summary>
        /// OnGUI How to prepare your project scenes
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIHowToPrepareYourProjectScenes()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("シーンの用意", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. 以下のシーンを適当な場所にコピーします");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Kit/Scenes/base init.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Kit/Scenes/base title.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Kit/Scenes/base main game.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Kit/Scenes/base ending.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. コピーしたシーンを[Build Settings]に追加します");
                EditorGUILayout.LabelField("   ([base init.unity]が一番上になるようにしてください)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("3. 各シーンのCanvasのテキストに指示があるのでそれに従ってください");

            }

            else
            {

                EditorGUILayout.LabelField("How to prepare your scenes", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. Copy the following base scenes to anywhere");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Kit/Scenes/base init.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Kit/Scenes/base title.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Kit/Scenes/base main game.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Kit/Scenes/base ending.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. Add the scenes you copied to [Build Settings]");
                EditorGUILayout.LabelField("   ([base init.unity] must be first scene)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("3. Follow the canvas texts in each scene to make the scenes work");

            }

            GUILayout.Space(this.m_space);

        }

        /// <summary>
        /// OnGUI How to click and move
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIHowToClickAndMove()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("クリックによる移動の仕方", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. 参考シーン");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/000 Click and Move.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [ViewPoint]");
                EditorGUILayout.LabelField("   [ViewPoint]はカメラ位置と向きを定義します。");
                EditorGUILayout.LabelField("   親関係にある[ViewPoint]へは[PlayerCameraScript.onClickBackViewButton]を実行することで移動できます。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_disableMoveToParentByClick]");
                EditorGUILayout.LabelField("       [m_disableMoveToParentByClick]が有効な場合、親関係にある[ViewPoint]へは移動できません。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [Start Sync with Scene Camera]");
                EditorGUILayout.LabelField("       [ViewPoint]とシーンカメラの同期を開始します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.3 [Move Scene Camera and Main Camera]");
                EditorGUILayout.LabelField("       シーンカメラとメインカメラを[ViewPoint]の位置へ移動させます。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. [ViewPointGroup]");
                EditorGUILayout.LabelField("   [ViewPoint]は[ViewPointGroup]の子とすることで自身の所属を定義します。");
                EditorGUILayout.LabelField("   別の[ViewPointGroup]の[ViewPoint]へ移動することはできません。");
                EditorGUILayout.LabelField("   ([ViewPointGroup]を変更する(部屋を移動する)場合は[ChangeRoomScript]を使用します)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("3. [ClickableColliderScript]");
                EditorGUILayout.LabelField("   [ClickableColliderScript]を継承したスクリプトを所持するオブジェクトをクリックすることで移動できます。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.1 [m_refTargetViewPoint]");
                EditorGUILayout.LabelField("       [m_refTargetViewPoint]はカメラの移動先を定義します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.2 [m_lockState]");
                EditorGUILayout.LabelField("       [m_lockState]は継承スクリプトが機能する状態かを示します。");
                EditorGUILayout.LabelField("       (クリックによるカメラ移動は[m_lockState]の状態に関わらず可能です)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.3 [m_wayPointList]");
                EditorGUILayout.LabelField("       [m_wayPointList]はカメラ移動時の道筋を定義します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.5 [m_disableColliderAtStart]");
                EditorGUILayout.LabelField("       有効な場合、シーン開始時にコライダーを無効化します。");
                EditorGUILayout.LabelField("       (シーン開始時にコライダーが無効な場合、有効化したときの挙動が変になる場合の回避用)");

            }

            else
            {

                EditorGUILayout.LabelField("How to click and move", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. Sample scenes");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/000 Click and Move.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [ViewPoint]");
                EditorGUILayout.LabelField("   [ViewPoint] is a script to define camera position and rotationos.");
                EditorGUILayout.LabelField("   You can move the camera to parent [ViewPoint] by calling [PlayerCameraScript.onClickBackViewButton.");
                EditorGUILayout.LabelField("   (Only if [ViewPoint.m_disableMoveToParentByClick] is checked.)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_disableMoveToParentByClick]");
                EditorGUILayout.LabelField("       If [m_disableMoveToParentByClick] id checked, you can't move the camera to parent [ViewPoint].");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [Start Sync with Scene Camera]");
                EditorGUILayout.LabelField("       The [ViewPoint] starts synchronization with editor scene camera.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.3 [Move Scene Camera and Main Camera]");
                EditorGUILayout.LabelField("       Move editor scene camera and main camera to the [ViewPoint].");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. [ViewPointGroup]");
                EditorGUILayout.LabelField("   [ViewPointGroup] is a script to define a group of [ViewPoint]s");
                EditorGUILayout.LabelField("   Main camera can't move to [ViewPoint]s that belong to another [ViewPointGroup].");
                EditorGUILayout.LabelField("   (Use [ChangeRoomScript] if you want to change current [ViewPointGroup].");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("3. [ClickableColliderScript]");
                EditorGUILayout.LabelField("   [ClickableColliderScript] is a base script to move main camera by clicking its collider.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.1 [m_refTargetViewPoint]");
                EditorGUILayout.LabelField("       [m_refTargetViewPoint] defines a reference to [ViewPoint] when clicked.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.2 [m_lockState]");
                EditorGUILayout.LabelField("       [m_lockState] defines inherited script's feature works or not.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.3 [m_wayPointList]");
                EditorGUILayout.LabelField("       [m_wayPointList] defines a path to [m_refTargetViewPoint].");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.5 [m_disableColliderAtStart]");
                EditorGUILayout.LabelField("       If [m_refTargetViewPoint] is checked, the collider will be disbled in [Start] function.");
                EditorGUILayout.LabelField("       (To avoid a collider bug occurred when the collider is disabled at the scene starts)");

            }

            GUILayout.Space(this.m_space);

        }

        /// <summary>
        /// OnGUI How to change rooms
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIHowToChangeRooms()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("部屋の移動", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. 参考シーン");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/001 Room Change.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [ChangeRoomScript]");
                EditorGUILayout.LabelField("   [ChangeRoomScript]は[ViewPointGroup]を変更(部屋を移動)するためのスクリプトです。");
                EditorGUILayout.LabelField("   往路と復路の[ViewPoint]と道筋を定義します。");

            }

            else
            {

                EditorGUILayout.LabelField("How to change rooms", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. Sample scenes");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/001 Room Change.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [ChangeRoomScript]");
                EditorGUILayout.LabelField("   [ChangeRoomScript] is a script to change current [ViewPointGroup].");
                EditorGUILayout.LabelField("   It also defines outward and return [ViewPoint]s and paths to them.");

            }

            GUILayout.Space(this.m_space);

        }


        /// <summary>
        /// OnGUI How to lock and unlock without items
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIHowToLockAndUnlockWithoutItems()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("オブジェクトのロックとアンロック(アイテム無し)", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. 参考シーン");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/002 Open and Close Simple.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/003 Open and Close Animator.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [OpenCloseAnimScript]");
                EditorGUILayout.LabelField("   対象をアニメーションさせることでロックとアンロックを実現します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_refUnlockTargetList]");
                EditorGUILayout.LabelField("       アンロックする対象を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [m_refAnimList]");
                EditorGUILayout.LabelField("       アニメーションさせる対象を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. [OpenCloseIeHolder]");
                EditorGUILayout.LabelField("   移動や回転や拡大によるアニメーションを定義します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("3. [OpenCloseAnimatorHolder]");
                EditorGUILayout.LabelField("   Animatorよるアニメーションを定義します。");

            }

            else
            {
                
                EditorGUILayout.LabelField("How to lock and unlock objects (without items)", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. Sample scenes");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/002 Open and Close Simple.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/003 Open and Close Animator.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [OpenCloseAnimScript]");
                EditorGUILayout.LabelField("   [OpenCloseAnimScript] is a script to lock and unlock objects by animations");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_refUnlockTargetList]");
                EditorGUILayout.LabelField("       Target list to unlock (and lock).");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [m_refAnimList]");
                EditorGUILayout.LabelField("       Target list to animate when unlocked (and locked)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. [OpenCloseIeHolder]");
                EditorGUILayout.LabelField("   [OpenCloseIeHolder] is a script to define an animation by IEnumerator.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("3. [OpenCloseAnimatorHolder]");
                EditorGUILayout.LabelField("   [OpenCloseAnimatorHolder] is a script to define an animation by Animator.");

            }

            GUILayout.Space(this.m_space);

        }

        /// <summary>
        /// OnGUI How to exit
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIHowTExit()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("終了の仕方", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. 参考シーン");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/004 Exit.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [ExitScript]");
                EditorGUILayout.LabelField("   クリックすることでシーン変更します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_nextScene]");
                EditorGUILayout.LabelField("       読み込むシーンを指定します。");

            }

            else
            {

                EditorGUILayout.LabelField("How to exit", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. Sample scenes");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/004 Exit.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [ExitScript]");
                EditorGUILayout.LabelField("   [ExitScript] is a script to define next scene called when clicked.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_nextScene]");
                EditorGUILayout.LabelField("       Next scene.");

            }

            GUILayout.Space(this.m_space);

        }

        /// <summary>
        /// OnGUI About item
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIAboutItem()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("アイテムについて", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. 参考シーン");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/100 Get Item.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/101 Use Item.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/102 Item Evolution 1.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/103 Item Evolution 2.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/106 Multiple Items.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [ItemObjectScript]");
                EditorGUILayout.LabelField("   クリックすることでそのアイテムを取得できます。");
                EditorGUILayout.LabelField("   アイテムには進化前と進化後という2つの状態があります。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_activateBeforeEvolutionList]");
                EditorGUILayout.LabelField("       アイテムを進化させる時に有効化(表示)したいオブジェクトを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [m_deactivateAfterEvolutionList]");
                EditorGUILayout.LabelField("       アイテムを進化させる時に無効化(非表示)したいオブジェクトを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. [ItemWaitingRoomScript]");
                EditorGUILayout.LabelField("   アイテムの情報を設定するためのスクリプトです。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.1 [m_conditionsForEvolution]");
                EditorGUILayout.LabelField("       このアイテムを進化させる条件を設定します。");
                EditorGUILayout.LabelField("       (進化させる必要がない場合は無視することができます)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.1 [item]");
                EditorGUILayout.LabelField("         このアイテムを進化させるためのアイテムを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.2 [requiredNumberOfItems]");
                EditorGUILayout.LabelField("         このアイテムを進化させるためのアイテムの必要個数を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.3 [requiredEvolution]");
                EditorGUILayout.LabelField("         このアイテムを進化させるためのアイテムが進化している必要があるかを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.2 [m_refMainRoom]");
                EditorGUILayout.LabelField("       1つ目に取得したアイテムの保存先です。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.3 [m_refSubRoom]");
                EditorGUILayout.LabelField("       2つ目以降に取得したアイテムの保存先です。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.4 [m_refItemImage]");
                EditorGUILayout.LabelField("       アイテムのスプライトの表示先を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.5 [m_beforeSprite]");
                EditorGUILayout.LabelField("       進化前のスプライトを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.6 [m_afterSprite]");
                EditorGUILayout.LabelField("       進化後のスプライトを指定します。");
                EditorGUILayout.LabelField("       (進化させる必要がない場合は無視することができます)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.7 [m_beforeEvolutionItemText]");
                EditorGUILayout.LabelField("       進化前のアイテム表示名です。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.8 [m_beforeEvolutionItemText]");
                EditorGUILayout.LabelField("       進化後のアイテム表示名です。");
                EditorGUILayout.LabelField("       (進化させる必要がない場合は無視することができます)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("3. [FieldObjectAffectorScript]");
                EditorGUILayout.LabelField("   アイテムを進化させることができるフィールドオブジェクト用のスクリプトです。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.1 [m_refTargetItem]");
                EditorGUILayout.LabelField("       進化させる対象を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("4. [ItemManager]");
                EditorGUILayout.LabelField("   アイテム全体の情報を管理するためのスクリプトです。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("5. [ItemCameraScript]");
                EditorGUILayout.LabelField("   アイテム表示のカメラ制御用です。");
                EditorGUILayout.LabelField("   [ItemObjectScript]を持つオブジェクトがこのカメラに映るために、");
                EditorGUILayout.LabelField("   [ItemObjectScript]を持つオブジェクトのレイヤー名は[Item]である必要があります。");

            }

            else
            {

                EditorGUILayout.LabelField("About items", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. Sample scenes");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/100 Get Item.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/101 Use Item.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/102 Item Evolution 1.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/103 Item Evolution 2.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/106 Multiple Items.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [ItemObjectScript]");
                EditorGUILayout.LabelField("   You can get an item by clicking an object that have this script.");
                EditorGUILayout.LabelField("   An item object have two states [Before Evolution] and [After Evolution].");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_activateBeforeEvolutionList]");
                EditorGUILayout.LabelField("       Target list to activate when the script starts its evolution.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [m_deactivateAfterEvolutionList]");
                EditorGUILayout.LabelField("       Target list to deactivate when the script starts its evolution.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. [ItemWaitingRoomScript]");
                EditorGUILayout.LabelField("   [ItemWaitingRoomScript] is a script to manage its item informations.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.1 [m_conditionsForEvolution]");
                EditorGUILayout.LabelField("       [m_conditionsForEvolution] defines conditions to evolve the item.");
                EditorGUILayout.LabelField("       (You can ignore this parameter if the item can't evolve)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.1 [item]");
                EditorGUILayout.LabelField("         An item to evolve this item. (A)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.2 [requiredNumberOfItems]");
                EditorGUILayout.LabelField("         The number of (A) to evolve.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.3 [requiredEvolution]");
                EditorGUILayout.LabelField("         (A) needs to be evolved.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.2 [m_refMainRoom]");
                EditorGUILayout.LabelField("       A destination to put first item.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.3 [m_refSubRoom]");
                EditorGUILayout.LabelField("       A destination to put items after second.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.4 [m_refItemImage]");
                EditorGUILayout.LabelField("       [m_refItemImage] defines [ItemImageScript] to show sprite images.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.5 [m_beforeSprite]");
                EditorGUILayout.LabelField("       A sprite image before evolution.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.6 [m_afterSprite]");
                EditorGUILayout.LabelField("       A sprite image after evolution.");
                EditorGUILayout.LabelField("       (You can ignore this parameter if the item can't evolve)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.7 [m_beforeEvolutionItemText]");
                EditorGUILayout.LabelField("       An item name before evolution.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.8 [m_beforeEvolutionItemText]");
                EditorGUILayout.LabelField("       An item name after evolution.");
                EditorGUILayout.LabelField("       (You can ignore this parameter if the item can't evolve)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("3. [FieldObjectAffectorScript]");
                EditorGUILayout.LabelField("   [FieldObjectAffectorScript] is a script for objects to evolve items.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.1 [m_refTargetItem]");
                EditorGUILayout.LabelField("       Target item to evolve.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("4. [ItemManager]");
                EditorGUILayout.LabelField("   [ItemManager] is a script to manage all items.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("5. [ItemCameraScript]");
                EditorGUILayout.LabelField("   [ItemCameraScript] is a script for a camera to show items.");
                EditorGUILayout.LabelField("   [ItemObjectScript]'s layer must be [Item] for the camera.");

            }

            GUILayout.Space(this.m_space);

        }

        /// <summary>
        /// OnGUI How to lock and unlock with items
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIHowToLockAndUnlockWithItems()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("オブジェクトのロックとアンロック(アイテム有り)", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("(アイテムによるフィールドオブジェクトの進化について)", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. 参考シーン");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/104 Field Object Evolution 1.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/105 Field Object Evolution 2.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [EvolvableFieldObjectScript]");
                EditorGUILayout.LabelField("   フィールドオブジェクトの進化内容を設定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_refAnimList]");
                EditorGUILayout.LabelField("       進化によってアンロックする対象を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [m_refUnlockTargetList]");
                EditorGUILayout.LabelField("       進化する時にアニメーションさせる対象を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.3 [m_conditionsForEvolution]");
                EditorGUILayout.LabelField("       進化させるために必要な情報を設定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.1 [item]");
                EditorGUILayout.LabelField("         このフィールドオブジェクトを進化させるためのアイテムを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.2 [requiredNumberOfItems]");
                EditorGUILayout.LabelField("         このフィールドオブジェクトを進化させるためのアイテムの必要個数を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.3 [requiredEvolution]");
                EditorGUILayout.LabelField("         このフィールドオブジェクトを進化させるためのアイテムが進化している必要があるかを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.4 [m_viewPointWhenEvolution]");
                EditorGUILayout.LabelField("         進化時の視点を変更したい場合に指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.5 [m_backToParentViewPointAfterEvolution]");
                EditorGUILayout.LabelField("         有効な場合、進化後に親視点へ移動します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.6 [m_activateBeforeEvolutionList]");
                EditorGUILayout.LabelField("         進化させる時に有効化(表示)したいオブジェクトを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.7 [m_deactivateAfterEvolutionList]");
                EditorGUILayout.LabelField("         進化後に無効化(非表示)したいオブジェクトを指定します。");


            }

            else
            {

                EditorGUILayout.LabelField("How to lock and unlock objects (with items)", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("(Field objects' evolution by using items)", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. Sample scenes");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/104 Field Object Evolution 1.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/105 Field Object Evolution 2.unity]");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [EvolvableFieldObjectScript]");
                EditorGUILayout.LabelField("   [EvolvableFieldObjectScript] defines settings about self evolution by an item.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [m_refAnimList]");
                EditorGUILayout.LabelField("       [m_refAnimList] is a list to animate when the object evolved.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [m_refUnlockTargetList]");
                EditorGUILayout.LabelField("       [m_refUnlockTargetList] is a list to unlock when the object evolved.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.3 [m_conditionsForEvolution]");
                EditorGUILayout.LabelField("       [m_conditionsForEvolution] defines settings to evolve.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.1 [item]");
                EditorGUILayout.LabelField("         An item to evolve this object. (A)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.2 [requiredNumberOfItems]");
                EditorGUILayout.LabelField("         The number of (A) to evolve.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.3 [requiredEvolution]");
                EditorGUILayout.LabelField("         (A) needs to be evolved.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.4 [m_viewPointWhenEvolution]");
                EditorGUILayout.LabelField("         [m_viewPointWhenEvolution] defines a [ViewPoint] when the object evolved.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.5 [m_backToParentViewPointAfterEvolution]");
                EditorGUILayout.LabelField("         If [m_backToParentViewPointAfterEvolution] is checked, main camera would move to parent [ViewPoint].");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.6 [m_activateBeforeEvolutionList]");
                EditorGUILayout.LabelField("         [m_activateBeforeEvolutionList] is a list to activate before the object evolved.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.3.7 [m_deactivateAfterEvolutionList]");
                EditorGUILayout.LabelField("         [m_activateBeforeEvolutionList] is a list to deactivate after the object evolved.");


            }

            GUILayout.Space(this.m_space);

        }


        /// <summary>
        /// OnGUI About Gimmick
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIAboutGimmick()
        {

            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("ギミックについて", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. 参考シーン");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/200 Number Input.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/201 Alphabet Input.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/202 Color Order Input.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/203 Direction Order Input.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/204 Slider Input.unity]");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("1. ギミック用基底スクリプト");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [ButtonInputScript]");
                EditorGUILayout.LabelField("       入力を管理するための基底スクリプトです。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [SubmitScript]");
                EditorGUILayout.LabelField("       入力の答えを管理するための基底スクリプトです。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.1.1 [m_refUnlockTargetList]");
                EditorGUILayout.LabelField("         正解時にアンロックする対象を指定します");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.1.2 [m_refAdditionalDisableTargetList]");
                EditorGUILayout.LabelField("         正解時に無効化したいオブジェクトを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.1.3 [m_unlockedMaterial]");
                EditorGUILayout.LabelField("         正解時のマテリアルを指定します。");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("2. 数字のギミック");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.1 [NumberInputScript]");
                EditorGUILayout.LabelField("       数字の入力を管理します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.1 [m_refSubmitNumberScript]");
                EditorGUILayout.LabelField("         答えあわせをするための[SubmitNumberScript]を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.2 [m_answerCharacterIndex]");
                EditorGUILayout.LabelField("         自分の桁数のインデックスを指定します。(0から)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.2 [SubmitNumberScript]");
                EditorGUILayout.LabelField("       [NumberInputScript]の答えを確認します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         答えを指定します。");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("3. アルファベットのギミック");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.1 [AlphabetInputScript]");
                EditorGUILayout.LabelField("       アルファベットの入力を管理します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      3.1.1 [m_refSubmitAlphabetScript]");
                EditorGUILayout.LabelField("         答えあわせをするための[SubmitAlphabetScript]を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      3.1.2 [m_answerCharacterIndex]");
                EditorGUILayout.LabelField("         自分の桁数のインデックスを指定します。(0から)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.2 [SubmitAlphabetScript]");
                EditorGUILayout.LabelField("       [AlphabetInputScript]の答えを確認します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      3.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         答えを指定します。");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("4. 色の順番のギミック");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   4.1 [ColorInputScript]");
                EditorGUILayout.LabelField("       色の入力を管理します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      4.1.1 [m_refSubmitColorScript]");
                EditorGUILayout.LabelField("         答えあわせをするための[SubmitColorScript]を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      4.1.2 [m_color]");
                EditorGUILayout.LabelField("         自分の色を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   4.2 [SubmitColorScript]");
                EditorGUILayout.LabelField("       [ColorInputScript]の答えを確認します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      4.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         答えを指定します。");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("5. 矢印の順番のギミック");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   5.1 [DirectionInputScript]");
                EditorGUILayout.LabelField("       矢印の入力を管理します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      5.1.1 [m_refSubmitDirectionScript]");
                EditorGUILayout.LabelField("         答えあわせをするための[SubmitDirectionScript]を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      5.1.2 [m_direction]");
                EditorGUILayout.LabelField("         自分の矢印を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   5.2 [SubmitDirectionScript]");
                EditorGUILayout.LabelField("       [DirectionInputScript]の答えを確認します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      5.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         答えを指定します。");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("6. スライダーのギミック");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   6.1 [SliderInputScript]");
                EditorGUILayout.LabelField("       スライダーの入力を管理します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.1 [m_refSubmitSliderScript]");
                EditorGUILayout.LabelField("         答えあわせをするための[SubmitSliderScript]を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.2 [m_refSliderTransform]");
                EditorGUILayout.LabelField("         スライダーの位置を表すオブジェクトを指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.3 [m_division]");
                EditorGUILayout.LabelField("         スライダーの位置の分割数を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.4 [m_fromLocalPos]");
                EditorGUILayout.LabelField("         スライダーの動く範囲の開始位置を指定します。");


                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.5 [m_toLocalPos]");
                EditorGUILayout.LabelField("         スライダーの動く範囲の終了位置を指定します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.6 [m_answerCharacterIndex]");
                EditorGUILayout.LabelField("         自分のスライダーのインデックスを指定します。(0から)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   6.2 [SubmitSliderScript]");
                EditorGUILayout.LabelField("       [DirectionInputScript]の答えを確認します。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         答えを指定します。");

                GUILayout.Space(this.m_space);

            }

            else
            {

                EditorGUILayout.LabelField("About gimmicks", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("0. Sample scenes");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/200 Number Input.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/201 Alphabet Input.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/202 Color Order Input.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/203 Direction Order Input.unity]");
                EditorGUILayout.LabelField("   [Assets/Ciitt Escape Game Kit/Demo Assets/Scenes/Samples/204 Slider Input.unity]");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("1. Base script for gimmicks");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.1 [ButtonInputScript]");
                EditorGUILayout.LabelField("       [ButtonInputScript] is a base script to manage user input.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   1.2 [SubmitScript]");
                EditorGUILayout.LabelField("       [ButtonInputScript] is a base script to manage correct answer.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.1.1 [m_refUnlockTargetList]");
                EditorGUILayout.LabelField("         [m_refUnlockTargetList] is a list to unlock when user input is correct.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.1.2 [m_refAdditionalDisableTargetList]");
                EditorGUILayout.LabelField("         [m_refUnlockTargetList] is a list to disable when user input is correct.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      1.1.3 [m_unlockedMaterial]");
                EditorGUILayout.LabelField("         [m_unlockedMaterial] is a material for correct answer.");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("2. Number gimmick");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.1 [NumberInputScript]");
                EditorGUILayout.LabelField("       [NumberInputScript] is a script to manage user input for numbers.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.1 [m_refSubmitNumberScript]");
                EditorGUILayout.LabelField("         [m_refSubmitNumberScript] defines [SubmitNumberScript] to answer.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.1.2 [m_answerCharacterIndex]");
                EditorGUILayout.LabelField("         [m_answerCharacterIndex] defines an index of the answer. (from 0)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   2.2 [SubmitNumberScript]");
                EditorGUILayout.LabelField("       [SubmitNumberScript] checks [NumberInputScript]s' input.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      2.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         The correct answer.");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("3. Alphabet gimmick");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.1 [AlphabetInputScript]");
                EditorGUILayout.LabelField("       [AlphabetInputScript] is a script to manage user input for alphabets.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      3.1.1 [m_refSubmitAlphabetScript]");
                EditorGUILayout.LabelField("         [m_refSubmitAlphabetScript] defines [SubmitAlphabetScript] to answer.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      3.1.2 [m_answerCharacterIndex]");
                EditorGUILayout.LabelField("         [m_answerCharacterIndex] defines an index of the answer. (from 0)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   3.2 [SubmitAlphabetScript]");
                EditorGUILayout.LabelField("       [SubmitAlphabetScript] checks [AlphabetInputScript]s' input.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      3.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         The correct answer.");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("4. Color order gimmick");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   4.1 [ColorInputScript]");
                EditorGUILayout.LabelField("       [ColorInputScript] is a script to manage user input for colors.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      4.1.1 [m_refSubmitColorScript]");
                EditorGUILayout.LabelField("         [m_refSubmitColorScript] defines [SubmitColorScript] to answer.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      4.1.2 [m_color]");
                EditorGUILayout.LabelField("         [m_color] defines self color.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   4.2 [SubmitColorScript]");
                EditorGUILayout.LabelField("       [SubmitColorScript] checks [ColorInputScript]s' input order.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      4.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         The correct answer.");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("5. Direction order gimmick");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   5.1 [DirectionInputScript]");
                EditorGUILayout.LabelField("       [DirectionInputScript] is a script to manage user input for directions.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      5.1.1 [m_refSubmitDirectionScript]");
                EditorGUILayout.LabelField("         [m_refSubmitDirectionScript] defines [SubmitDirectionScript] to answer.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      5.1.2 [m_direction]");
                EditorGUILayout.LabelField("         [m_color] defines self direction.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   5.2 [SubmitDirectionScript]");
                EditorGUILayout.LabelField("       [SubmitDirectionScript] checks [DirectionInputScript]s' input order.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      5.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         The correct answer.");

                GUILayout.Space(this.m_space);

                // ------------------------------------------------------

                EditorGUILayout.LabelField("6. Slider gimmick");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   6.1 [SliderInputScript]");
                EditorGUILayout.LabelField("       [SliderInputScript] is a script to manage user input for sliders.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.1 [m_refSubmitSliderScript]");
                EditorGUILayout.LabelField("         [m_refSubmitSliderScript] defines [SubmitSliderScript] to answer.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.2 [m_refSliderTransform]");
                EditorGUILayout.LabelField("         [m_refSliderTransform] defines a object that expresses slider.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.3 [m_division]");
                EditorGUILayout.LabelField("         [m_division] defines a division for slider.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.4 [m_fromLocalPos]");
                EditorGUILayout.LabelField("         [m_fromLocalPos] defines a start position for slider.");


                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.5 [m_toLocalPos]");
                EditorGUILayout.LabelField("         [m_fromLocalPos] defines a end position for slider.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.1.6 [m_answerCharacterIndex]");
                EditorGUILayout.LabelField("         [m_answerCharacterIndex] defines an index of the answer. (from 0)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   6.2 [SubmitSliderScript]");
                EditorGUILayout.LabelField("       [SubmitSliderScript] checks [SliderInputScript]s' input.");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("      6.2.1 [m_correctAnswer]");
                EditorGUILayout.LabelField("         The correct answer.");

                GUILayout.Space(this.m_space);

            }

            GUILayout.Space(this.m_space);

        }

        /// <summary>
        /// OnGUI How to save 
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIHowToSaveUserProgressData()
        {


            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("セーブ機能について", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. セーブ方法");
                EditorGUILayout.LabelField("   [MainGameSceneScript.saveUserProgress]を呼び出してください。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. 自作のスクリプトの情報をセーブする");
                EditorGUILayout.LabelField("   [OpenCloseAnimScript]を例に、");
                EditorGUILayout.LabelField("   [onSceneChangeStateReceiver]と[onUserProgressDataSignal]を実装し、");
                EditorGUILayout.LabelField("   [Awake]か[Start]関数内で[CustomReduxManager]に登録してください。");

            }

            else
            {


                EditorGUILayout.LabelField("How to save user progress data", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. How to save");
                EditorGUILayout.LabelField("   Call [MainGameSceneScript.saveUserProgress].");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("2. How to save self-made script");
                EditorGUILayout.LabelField("   Based on [OpenCloseAnimScript],");
                EditorGUILayout.LabelField("   implement [onSceneChangeStateReceiver] and [onUserProgressDataSignal],");
                EditorGUILayout.LabelField("   add them by calling [CustomReduxManager] in [Awake] or [Start] functions.");

            }

            GUILayout.Space(this.m_space);

        }


        /// <summary>
        /// OnGUI How to customize UI
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        void OnGUIHowToCustomizeUi()
        {


            if (this.m_language == SystemLanguage.Japanese)
            {

                EditorGUILayout.LabelField("UIのカスタマイズ", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [MainGameSceneScript]");
                EditorGUILayout.LabelField("   UIは6種類あり、デフォルトでは以下のような所属ID名がつけられています。");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   MainGame               (メインゲームUI)");
                EditorGUILayout.LabelField("   ItemShowroom           (アイテム表示用UI)");
                EditorGUILayout.LabelField("   FieldObjectEvolution   (フィールドオブジェクト進化用UI)");
                EditorGUILayout.LabelField("   ItemEvolution          (アイテム進化用UI)");
                EditorGUILayout.LabelField("   Options                (オプションUI)");
                EditorGUILayout.LabelField("   HowToPlay              (プレイ方法UI)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   自作のUIを管理に追加したい場合は");
                EditorGUILayout.LabelField("   [SimpleUiControllerScript]をそのUIオブジェクトに追加し、");
                EditorGUILayout.LabelField("   [Ui Identifier List]に上記の所属名を追加してください。");

            }

            else
            {

                EditorGUILayout.LabelField("How to customize UI", EditorStyles.boldLabel);

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("1. [MainGameSceneScript]");
                EditorGUILayout.LabelField("   UIs have the following 6 group IDs ");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   MainGame               (Main game UI)");
                EditorGUILayout.LabelField("   ItemShowroom           (Item display UI)");
                EditorGUILayout.LabelField("   FieldObjectEvolution   (Field object evolution UI)");
                EditorGUILayout.LabelField("   ItemEvolution          (Item evolution UI)");
                EditorGUILayout.LabelField("   Options                (Options UI)");
                EditorGUILayout.LabelField("   HowToPlay              (How to play UI)");

                GUILayout.Space(this.m_space);

                EditorGUILayout.LabelField("   If you want to add seld-made UI contents,");
                EditorGUILayout.LabelField("   add [SimpleUiControllerScript]to your UI GameObject,");
                EditorGUILayout.LabelField("   and add the avobe group name ID to [Ui Identifier List].");

            }

            GUILayout.Space(this.m_space);

        }

        /// <summary>
        /// OnGUI
        /// </summary>
        // -----------------------------------------------------------------------------------------------
        protected virtual void OnGUI()
        {

            this.m_scrollPos = EditorGUILayout.BeginScrollView(this.m_scrollPos);

            if (this.m_type == HelpMessageType.Contents)
            {

                // OnGUIContents
                {
                    this.OnGUIContents();
                }

                GUILayout.Space(this.m_space);

            }

            else
            {

                this.backButton();

                GUILayout.Space(this.m_space);

                if (this.m_type == HelpMessageType.Pdf)
                {
                    this.OnGUIPdf();
                }

                else if(this.m_type == HelpMessageType.HowToPrepareYourProjectScenes)
                {
                    this.OnGUIHowToPrepareYourProjectScenes();
                }

                else if (this.m_type == HelpMessageType.HowToClickAndMove)
                {
                    this.OnGUIHowToClickAndMove();
                }

                else if (this.m_type == HelpMessageType.HowToChangeRooms)
                {
                    this.OnGUIHowToChangeRooms();
                }

                else if (this.m_type == HelpMessageType.HowToLockAndUnlockWithoutItems)
                {
                    this.OnGUIHowToLockAndUnlockWithoutItems();
                }

                else if (this.m_type == HelpMessageType.HowToExit)
                {
                    this.OnGUIHowTExit();
                }

                else if (this.m_type == HelpMessageType.AboutItem)
                {
                    this.OnGUIAboutItem();
                }

                else if (this.m_type == HelpMessageType.HowToLockAndUnlockWithItems)
                {
                    this.OnGUIHowToLockAndUnlockWithItems();
                }

                else if (this.m_type == HelpMessageType.AboutGimmick)
                {
                    this.OnGUIAboutGimmick();
                }

                else if (this.m_type == HelpMessageType.HowToSaveUserProgressData)
                {
                    this.OnGUIHowToSaveUserProgressData();
                }

                else if (this.m_type == HelpMessageType.HowToCustomizeUi)
                {
                    this.OnGUIHowToCustomizeUi();
                }

            }


            EditorGUILayout.EndScrollView();

        }

    }

}
