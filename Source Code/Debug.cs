using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmotionalFix
{
    public class Debug
    {
        public static void Log(string message)
        {
            PathDebug("/Debug", PathType.Directory);
            File.AppendAllText(Harmony_Patch.modPath + "/Debug/Log.txt", message + "\n");
        }
        public static void ModPatchDebug()
        {
            PathDebug("/Debug", PathType.Directory);
            File.WriteAllText(Harmony_Patch.modPath + "/Debug/Log.txt", "ModPath: "+Harmony_Patch.modPath+"\n");
        }
        public static void Error(string type,Exception ex)
        {
            PathDebug("/Debug", PathType.Directory);
            File.WriteAllText(Harmony_Patch.modPath + "/Debug/"+type+"Error.txt", ex.Message + Environment.NewLine + ex.StackTrace);
        }
        public static void PathDebug(string path,PathType type)
        {
            if (type == PathType.Directory)
            {
                if (!Directory.Exists(Harmony_Patch.modPath + path))
                {
                    File.WriteAllText(Application.dataPath + "/BaseMods/ContingecyContractModPathError.txt", Harmony_Patch.modPath + path + " not found");
                }
            }
            if (type == PathType.File)
            {
                if (!File.Exists(Harmony_Patch.modPath + path))
                {
                    File.WriteAllText(Application.dataPath + "/BaseMods/ContingecyContractModPathError.txt", Harmony_Patch.modPath + path + " not found");
                }
            }
        }
    }
    public enum PathType
    {
        Directory,
        File
    }
}
