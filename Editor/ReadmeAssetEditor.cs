//Created by UnityTechnologies
//Modified by AoiKamishiro

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace USTL.ReadmeViewer.Editor
{
    [InitializeOnLoad]
    [CustomEditor(typeof(ReadmeAsset))]
    public class ReadmeAssetEditor : UnityEditor.Editor
    {
        static ReadmeAssetEditor()
        {
            AssetDatabase.importPackageCompleted += _ => SelectReadmeAutomatically();
        }

        private void OnEnable()
        {
            ReadmeAsset readme = (ReadmeAsset)target;
            readme._cachedIcon = null;
        }

        private static void SelectReadmeAutomatically()
        {
            foreach (string guid in AssetDatabase.FindAssets($"t:{nameof(ReadmeAsset)}"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (AssetDatabase.GetMainAssetTypeAtPath(path) != typeof(ReadmeAsset))
                {
                    continue;
                }

                ReadmeAsset asset = AssetDatabase.LoadAssetAtPath<ReadmeAsset>(path);
                if (!asset || asset.isLoaded)
                {
                    continue;
                }

                asset.isLoaded = true;
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Selection.activeObject = asset;
            }
        }

        protected override void OnHeaderGUI()
        {
        }

        public override VisualElement CreateInspectorGUI()
        {
            ReadmeAsset readme = (ReadmeAsset)target;

            VisualElement root = new();
            root.style.paddingLeft = 4f;
            root.style.paddingRight = 4f;
            root.style.paddingBottom = Space;

            root.Add(CreateHeader(readme));

            if (readme.chapters == null || readme.chapters.Length == 0)
            {
                return root;
            }

            root.Add(CreateDivider());

            foreach (ReadmeAsset.Chapter chapter in readme.chapters)
            {
                root.Add(CreateChapter(chapter));
            }

            return root;
        }

        /// <summary>
        ///     インスペクターの表題を構築します。
        /// </summary>
        private static VisualElement CreateHeader(ReadmeAsset readme)
        {
            VisualElement header = new();
            header.style.flexDirection = FlexDirection.Row;
            header.style.alignItems = Align.Center;
            header.style.marginTop = Space / 2f;
            header.style.marginBottom = Space / 2f;

            if (readme.Icon)
            {
                Image icon = new()
                {
                    image = readme.Icon,
                    scaleMode = ScaleMode.ScaleToFit,
                };
                icon.style.width = readme.iconMaxWidth;
                icon.style.height = readme.iconMaxWidth;
                icon.style.maxWidth = readme.iconMaxWidth;
                icon.style.maxHeight = readme.iconMaxWidth;
                icon.style.flexShrink = 1f;
                icon.style.marginRight = 12f;
                header.Add(icon);
            }

            Label title = CreateLabel(readme.title, TitleFontSize, FontStyle.Normal);
            title.style.flexGrow = 1f;
            title.style.flexShrink = 1f;
            header.Add(title);

            return header;
        }

        private static VisualElement CreateDivider()
        {
            VisualElement divider = new();
            divider.style.height = 1f;
            divider.style.marginBottom = Space / 2f;
            divider.style.flexShrink = 0f;
            divider.style.backgroundColor = DividerColor;
            return divider;
        }

        private static VisualElement CreateChapter(ReadmeAsset.Chapter chapter)
        {
            VisualElement chapterElement = new();
            chapterElement.style.marginTop = Space / 2f;
            chapterElement.style.marginBottom = Space;

            if (!string.IsNullOrEmpty(chapter.chapterTitle))
            {
                chapterElement.Add(CreateLabel(chapter.chapterTitle, ChapterTitleFontSize, FontStyle.Bold));
            }

            if (!string.IsNullOrEmpty(chapter.chapterText))
            {
                chapterElement.Add(CreateLabel(chapter.chapterText, ChapterTextFontSize, FontStyle.Normal));
            }

            if (chapter.sections == null || chapter.sections.Length == 0)
            {
                return chapterElement;
            }

            foreach (ReadmeAsset.Section section in chapter.sections)
            {
                chapterElement.Add(CreateSection(section));
            }

            return chapterElement;
        }

        private static VisualElement CreateSection(ReadmeAsset.Section section)
        {
            VisualElement sectionElement = new();
            sectionElement.style.marginTop = Space / 4f;

            if (!string.IsNullOrEmpty(section.sectionTitle))
            {
                sectionElement.Add(CreateLabel(section.sectionTitle, SectionTitleFontSize, FontStyle.Bold));
            }

            if (section.sentences == null || section.sentences.Length == 0)
            {
                return sectionElement;
            }

            foreach (ReadmeAsset.Sentence sentence in section.sentences)
            {
                if (string.IsNullOrEmpty(sentence.text))
                {
                    continue;
                }

                sectionElement.Add(CreateSentence(sentence));
            }

            return sectionElement;
        }

        private static VisualElement CreateSentence(ReadmeAsset.Sentence sentence)
        {
            VisualElement sentenceElement = new();
            sentenceElement.style.flexDirection = FlexDirection.Row;
            sentenceElement.style.marginLeft = Mathf.Max(0, sentence.indent) * Tab;

            sentenceElement.Add(sentence.isLink ? CreateLink(sentence.text, sentence.url) : CreateLabel(sentence.text, LineFontSize, FontStyle.Normal));

            return sentenceElement;
        }

        private static Label CreateLink(string text, string url)
        {
            Label label = CreateLabel(text, LineFontSize, FontStyle.Normal);
            label.style.color = LinkColor;
            label.tooltip = url;
            label.RegisterCallback<MouseUpEvent>(evt =>
            {
                if (evt.button != 0)
                {
                    return;
                }

                Application.OpenURL(url);
                evt.StopPropagation();
            });

            return label;
        }

        private static Label CreateLabel(string text, int fontSize, FontStyle fontStyle)
        {
            Label label = new(text);
            label.style.fontSize = fontSize;
            label.style.unityFontStyleAndWeight = fontStyle;
            label.style.whiteSpace = WhiteSpace.Normal;
            label.style.flexGrow = 1f;
            label.style.flexShrink = 1f;
            return label;
        }


        #region Properties/Fields

        private const float Space = 16f;
        private const int Tab = 8;
        private const int TitleFontSize = 26;
        private const int ChapterTitleFontSize = 18;
        private const int ChapterTextFontSize = 16;
        private const int SectionTitleFontSize = 14;
        private const int LineFontSize = 14;
        private static readonly Color DividerColor = new(0.5f, 0.5f, 0.5f, 0.35f);
        private static readonly Color LinkColor = new(0.30980393f, 0.5019608f, 0.972549f);

        #endregion
    }
}
