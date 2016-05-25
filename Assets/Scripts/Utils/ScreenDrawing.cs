using System;
using UnityEngine;

namespace Utils {
	public static class ScreenDrawing {
		static Texture2D _whiteTexture;
		static Texture2D WhiteTexture {
			get {
				if (_whiteTexture == null) {
					_whiteTexture = TextureGenerator.TextureFromColor (Color.white);
				}
				return _whiteTexture;
			}
		}

		public static void DrawRect(Rect rect, Color c) {
			GUI.color = c;
			GUI.DrawTexture (rect, WhiteTexture);
			GUI.color = Color.white;
		}

		public static void DrawBox(Rect rect, float thickness, Color color) {
			// Top
			DrawRect( new Rect( rect.xMin, rect.yMin, rect.width, thickness ), color );
			// Left
			DrawRect( new Rect( rect.xMin, rect.yMin, thickness, rect.height ), color );
			// Right
			DrawRect( new Rect( rect.xMax - thickness, rect.yMin, thickness, rect.height ), color);
			// Bottom
			DrawRect( new Rect( rect.xMin, rect.yMax - thickness, rect.width, thickness ), color );
		}

		public static Rect GetScreenRect(Vector2 v1, Vector2 v2) {
			// Move origin from bottom left to top left
			v1.y = Screen.height - v1.y;
			v2.y = Screen.height - v2.y;

			// Calculate corners
			var topLeft = Vector2.Min( v1, v2 );
			var bottomRight = Vector2.Max( v1, v2 );

			// Create Rect
			return Rect.MinMaxRect( topLeft.x, topLeft.y, bottomRight.x, bottomRight.y );
		}
	}
}

