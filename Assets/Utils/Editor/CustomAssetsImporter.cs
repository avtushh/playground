// -------------------------------------------------------------------
// - A custom asset importer for Unity 3D game engine by Sarper Soher-
// - http://www.sarpersoher.com                                      -
// -------------------------------------------------------------------
// - This script utilizes the file names of the imported assets      -
// - to change the import settings automatically as described        -
// - in this script                                                  -
// -------------------------------------------------------------------


using UnityEngine;
using UnityEditor;  // Most of the utilities we are going to use are contained in the UnityEditor namespace

// We inherit from the AssetPostProcessor class which contains all the exposed variables and event triggers for asset importing pipeline
internal sealed class CustomAssetsImporter : AssetPostprocessor {
	// Couple of constants used below to be able to change from a single point, you may use direct literals instead of these consts to if you please
	private const int webTextureSize = 2048;
	private const int standaloneTextureSize = 4096;
	private const int iosTextureSize = 1024;
	private const int androidTextureSize = 1024;

	#region Methods

	//-------------Pre Processors

	// This event is raised when a texture asset is imported
	private void OnPreprocessTexture() {
		
		// Get the reference to the assetImporter (From the AssetPostProcessor class) and unbox it to a TextureImporter (Which is inherited and extends the AssetImporter with texture specific utilities)
		var importer = assetImporter as TextureImporter;

		// The options for the platform string are "Web", "Standalone", "iPhone", "Android"
		// Unity API provides neat single function settings for the most import settings as SetPlatformTextureSettings
		// Parameters are: platform, maxTextureSize, textureFormat, compressionQuality
		// I also change the format based on if the texture has an alpha channel or not because not all formats support an alpha channel
		//    SetAlphaChannelByPlatformType (importer);

		// Set the texture import type drop-down to advanced so our changes reflect in the import settings inspector
		importer.textureType = TextureImporterType.Sprite;
		importer.mipmapEnabled = false;
		importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;

		// If you are only using the alpha channel for transparency, uncomment the below line. I commented it out because I use the alpha channel for various shaders (e.g. specular map or various other masks)
		//importer.alphaIsTransparency = importer.DoesSourceTextureHaveAlpha();
	}

	void SetAlphaChannelByPlatformType (TextureImporter importer)
	{
		importer.SetPlatformTextureSettings ("Web", webTextureSize, importer.DoesSourceTextureHaveAlpha () ? TextureImporterFormat.DXT5 : TextureImporterFormat.DXT1, 100, false);
		importer.SetPlatformTextureSettings ("Standalone", standaloneTextureSize, importer.DoesSourceTextureHaveAlpha () ? TextureImporterFormat.DXT5 : TextureImporterFormat.DXT1, 100, false);
		importer.SetPlatformTextureSettings ("iPhone", iosTextureSize, importer.DoesSourceTextureHaveAlpha () ? TextureImporterFormat.PVRTC_RGBA4 : TextureImporterFormat.PVRTC_RGB4, 100, false);
		importer.SetPlatformTextureSettings ("Android", androidTextureSize, TextureImporterFormat.ETC_RGB4, 100, true);
	}

	//-------------Post Processors

	// This event is called as soon as the texture asset is imported successfully
	// Does nothing currently, just here for future possibilities
	private void OnPostprocessTexture(Texture2D import) {}



	#endregion
}