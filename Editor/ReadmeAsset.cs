//Created by UnityTechnologies
//Modified by AoiKamishiro

using System;
using UnityEditor;
using UnityEngine;

namespace USTL.ReadmeViewer.Editor
{
    [CreateAssetMenu(menuName = "Readme/ReadmeAsset", order = 100)]
    public class ReadmeAsset : ScriptableObject
    {
        public string iconGUID;
        public float iconMaxWidth = 64f;
        public string title;
        public Chapter[] chapters;
        public bool isLoaded;
        public bool showEditButton;

        [NonSerialized] internal Texture2D _cachedIcon;

        public Texture2D Icon
        {
            get
            {
                if (!_cachedIcon && !string.IsNullOrEmpty(iconGUID))
                {
                    _cachedIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(iconGUID));
                }

                return _cachedIcon;
            }
        }

        /// <summary>
        ///     ReadmeAssetの章を構成する構造体
        /// </summary>
        [Serializable]
        public struct Chapter
        {
            //章の表題と本文
            public string chapterTitle, chapterText;

            //節の配列
            public Section[] sections;
        }

        /// <summary>
        ///     ReadmeAssetの節を構成する構造体
        /// </summary>
        [Serializable]
        public struct Section
        {
            //節の表題
            public string sectionTitle;

            //文の配列
            public Sentence[] sentences;
        }

        /// <summary>
        ///     ReadmeAssetの文を構成する構造体
        /// </summary>
        [Serializable]
        public struct Sentence
        {
            //文の内容
            public string text;

            //文の字下げする文字数
            public int indent;

            //文が参照を持つかどうか
            public bool isLink;

            //文が参照を持つ場合の参照先
            public string url;
        }
    }
}
