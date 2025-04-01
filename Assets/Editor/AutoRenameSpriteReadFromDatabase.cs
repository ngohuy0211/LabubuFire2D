using UnityEngine;
using UnityEditor;
using System.IO;

namespace Aord.Tools
{
    public class AutoRenameSpriteReadFromDatabase : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                if (Directory.Exists(assetPath))
                    continue;
                //
                string firstNameAsset = "";
                if (assetPath.Contains("_Pack/AvatarBackground") || assetPath.Contains("_Pack2/AvatarBackground"))
                    firstNameAsset = "aB_";
                else if (assetPath.Contains("_Pack/AvatarFrame") || assetPath.Contains("_Pack2/AvatarFrame"))
                    firstNameAsset = "aF_";
                else if (assetPath.Contains("_Pack/AvatarSpecial") || assetPath.Contains("_Pack2/AvatarSpecial"))
                    firstNameAsset = "aS_";
                else if (assetPath.Contains("_Pack/HeroHead") || assetPath.Contains("_Pack2/HeroHead"))
                    firstNameAsset = "hH_";
                else if (assetPath.Contains("_Pack/HeroArt") || assetPath.Contains("_Pack2/HeroArt"))
                    firstNameAsset = "hA_";
                else if (assetPath.Contains("_Pack/HeroThumb") || assetPath.Contains("_Pack2/HeroThumb"))
                    firstNameAsset = "hT_";
                else if (assetPath.Contains("_Pack/HeroBuf") || assetPath.Contains("_Pack2/HeroBuf"))
                    firstNameAsset = "hB_";
                else if (assetPath.Contains("_Pack/ItemArtifact") || assetPath.Contains("_Pack2/ItemArtifact"))
                    firstNameAsset = "iA_";
                else if (assetPath.Contains("_Pack/ItemConsumable") || assetPath.Contains("_Pack2/ItemConsumable"))
                    firstNameAsset = "iC_";
                else if (assetPath.Contains("_Pack/ItemEquipment") || assetPath.Contains("_Pack2/ItemEquipment"))
                    firstNameAsset = "iE_";
                else if (assetPath.Contains("_Pack/ItemShard") || assetPath.Contains("_Pack2/ItemShard"))
                    firstNameAsset = "iSh_";
                else if (assetPath.Contains("_Pack/SkillIcon") || assetPath.Contains("_Pack2/SkillIcon"))
                    firstNameAsset = "sI_";
                else if (assetPath.Contains("_Pack/ItemStone") || assetPath.Contains("_Pack2/ItemStone"))
                    firstNameAsset = "iSt_";
                else if (assetPath.Contains("_Pack/ItemGrindStone") || assetPath.Contains("_Pack2/ItemGrindStone"))
                    firstNameAsset = "iSt_gri_";
                else if (assetPath.Contains("_Pack/ItemGemStone") || assetPath.Contains("_Pack2/ItemGemStone"))
                    firstNameAsset = "iSt_ge_";

                if (!string.IsNullOrEmpty(firstNameAsset))
                {
                    string fileName = Path.GetFileNameWithoutExtension(assetPath);
                    if (!fileName.Contains(firstNameAsset))
                        RenameAsset(assetPath, firstNameAsset);
                }
            }
        }

        private static void RenameAsset(string assetPath, string firstName)
        {
            string fileName = Path.GetFileNameWithoutExtension(assetPath);

            string[] arrName = fileName.Split("_");
            string newName = firstName + arrName[arrName.Length - 1];
            AssetDatabase.RenameAsset(assetPath, newName);
            AssetDatabase.Refresh();
        }
    }
}