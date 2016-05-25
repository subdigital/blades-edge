using System;
using UnityEngine;

namespace Utils {
	public static class TextureGenerator {
		public static Texture2D TextureFromColor(Color c) {
			var tex = new Texture2D (1, 1);
			tex.SetPixel (0, 0, c);
			tex.Apply ();
			return tex;
		}
	}
}
