using System;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace {
	[CustomEditor(typeof(ObjectSpace))]
	public class ObjectSpaceEditor : Editor {
		public int intValue;
		public float floatValue;
		private bool box1Foldout;
		private HelpBox.Params param = new HelpBox.Params("Help box");

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			GUILayout.BeginVertical(EditorStyles.helpBox);
			float originalLabelWidth = EditorGUIUtility.labelWidth;
			float originalFieldWidth = EditorGUIUtility.fieldWidth;
			GUILayout.BeginHorizontal();
			string intFieldLabel = "int field";
			EditorGUIUtility.labelWidth = intFieldLabel.Length * 5;
			EditorGUIUtility.fieldWidth = originalLabelWidth - EditorGUIUtility.labelWidth - EditorStyles.inspectorDefaultMargins.padding.left;
			intValue = EditorGUILayout.IntField(intFieldLabel, intValue, GUILayout.ExpandWidth(false));
			string floatFieldLabel = "float field";
			EditorGUIUtility.labelWidth = floatFieldLabel.Length * 5;
			floatValue = EditorGUILayout.FloatField(floatFieldLabel, floatValue, GUILayout.ExpandWidth(false));
			GUILayout.EndHorizontal();
			EditorGUIUtility.labelWidth = originalLabelWidth;
			EditorGUIUtility.fieldWidth = originalFieldWidth;
			GUILayout.EndVertical();

			if (param == null) {
				param = new HelpBox.Params("Help box");
			}
			using (new HelpBox(param)) {
				if (!param.Foldout) return;
				intValue = EditorGUILayout.IntField(intFieldLabel, intValue);
				floatValue = EditorGUILayout.FloatField(floatFieldLabel, floatValue);

				using (new IndentPadding(10)) {
					intValue = EditorGUILayout.IntField(intFieldLabel, intValue);
					floatValue = EditorGUILayout.FloatField(floatFieldLabel, floatValue);
				}

				using (new GroupBox(new GroupBox.Params("Group box"))) {
					intValue = EditorGUILayout.IntField(intFieldLabel, intValue);
					floatValue = EditorGUILayout.FloatField(floatFieldLabel, floatValue);
				}
			}
			
			int column = 3;
			Rect r = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.labelWidth = r.width / column / 2;
			EditorGUILayout.BeginHorizontal();
			intValue = EditorGUILayout.IntField(intFieldLabel, intValue);
			floatValue = EditorGUILayout.FloatField(floatFieldLabel, floatValue);
			floatValue = EditorGUILayout.FloatField(floatFieldLabel, floatValue);
			EditorGUILayout.EndHorizontal();
			EditorGUIUtility.labelWidth = originalLabelWidth;
			EditorGUIUtility.fieldWidth = originalFieldWidth;
		}

		private class HelpBox : IDisposable {
			private Params param;

			public HelpBox(Params param) {
				this.param = param;
				
				GUIStyle gs = new GUIStyle(EditorStyles.helpBox);
				gs.padding = param.Padding;
				GUILayout.BeginVertical(gs);
				EditorGUILayout.LabelField("");
				Rect lastRect = GUILayoutUtility.GetLastRect();
				param.Foldout = EditorGUI.Foldout(lastRect, param.Foldout, param.Title, true);
				EditorGUIUtility.labelWidth -= param.Padding.left;
			}

			public void Dispose() {
				EditorGUIUtility.labelWidth += param.Padding.left;
				GUILayout.EndVertical();
			}

			public class Params {
				private RectOffset padding = new RectOffset(20, 10, 10, 10);
				private bool foldout = true;
				private string title = string.Empty;

				public Params(string title) : this(title, null) {
				}

				public Params(string title, RectOffset padding) {
					if (padding != null) {
						this.padding = padding;
					}

					if (!string.IsNullOrEmpty(title)) {
						this.title = title;
					}
				}

				public RectOffset Padding {
					get { return padding; }
					set { padding = value; }
				}

				public bool Foldout {
					get { return foldout; }
					set { foldout = value; }
				}

				public string Title {
					get { return title; }
					set { title = value; }
				}
			}
		}
		private class GroupBox : IDisposable {
			private Params param;

			public GroupBox(Params param) {
				this.param = param;
				
				GUIStyle gs = new GUIStyle("GroupBox");
				gs.padding = param.Padding;
				GUILayout.BeginVertical(gs);
				EditorGUILayout.LabelField("");
				Rect lastRect = GUILayoutUtility.GetLastRect();
				param.Foldout = EditorGUI.Foldout(lastRect, param.Foldout, param.Title, true);
				EditorGUIUtility.labelWidth -= param.Padding.left;
			}

			public void Dispose() {
				EditorGUIUtility.labelWidth += param.Padding.left;
				GUILayout.EndVertical();
			}

			public class Params {
				private RectOffset padding = new RectOffset(20, 10, 10, 10);
				private bool foldout = true;
				private string title = string.Empty;

				public Params(string title) : this(title, null) {
				}

				public Params(string title, RectOffset padding) {
					if (padding != null) {
						this.padding = padding;
					}

					if (!string.IsNullOrEmpty(title)) {
						this.title = title;
					}
				}

				public RectOffset Padding {
					get { return padding; }
					set { padding = value; }
				}

				public bool Foldout {
					get { return foldout; }
					set { foldout = value; }
				}

				public string Title {
					get { return title; }
					set { title = value; }
				}
			}
		}

		private class IndentPadding : IDisposable {
			private RectOffset padding = new RectOffset(10, 0, 0, 0);

			public IndentPadding(int indentWidth) {
				padding.left = indentWidth;
				GUIStyle gs = new GUIStyle();
				gs.padding = padding;
				EditorGUILayout.BeginVertical(gs);
				EditorGUIUtility.labelWidth -= padding.left;
			}

			public void Dispose() {
				EditorGUIUtility.labelWidth += padding.left;
				EditorGUILayout.EndVertical();
			}
		}
	}
}