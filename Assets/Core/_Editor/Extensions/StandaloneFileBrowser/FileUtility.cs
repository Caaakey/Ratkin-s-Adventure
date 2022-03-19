using System;
using System.IO;
using System.Linq;

using UnityEditor;
using SFB;

public static class UnityPath
{
    public static string Combine(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return path;

        string str = path.Replace("\\\\", "/");
        return str.Replace('\\', '/');
    }

    public static string LocalToAssetPath(string path)
    {
        string[] tmps = path.Split('/', '\\');
        int index = Array.FindIndex(tmps, x => x.Equals("Assets")) + 1;

        string result = "Assets";
        for (; index < tmps.Length; ++index)
            result = string.Concat(result, '/', tmps[index]);

        return result;
    }

    public static string AssetToLocalPath(string assetPath)
    {
        string directory = UnityEngine.Application.dataPath;
        string temp = assetPath.Remove(0, 6);   //  Scenes

        return string.Concat(directory, temp);
    }

    public static string Combine(string path, params string[] others)
    {
        string str = SplitSlash(Combine(path));
        
        foreach (var o in others)
        {
            string s = SplitSlash(Combine(o));

            if (s != null)
            {
                if (str == null) str = s;
                else str = string.Concat(str, '/', s);
            }
        }

        return str;

        string SplitSlash(string split)
        {
            if (string.IsNullOrWhiteSpace(split)) return null;
            if (split[split.Length - 1] == '/')
            {
                if (split.Length == 1) return null;
                else return split.Remove(split.Length - 1);
            }

            return split;
        }
    }
}

public class FileUtility
{
    #region OpenFile Function & Filter
    private static string LastOpenDirectory = null;
    private const string PrefsString = "SFB_LastOpenDirectory";
    public static string DirectoryHistory
    {
        get
        {
            if (string.IsNullOrEmpty(LastOpenDirectory))
                LastOpenDirectory = EditorPrefs.GetString(PrefsString);

            return LastOpenDirectory;
        }

        private set
        {
            string str = Path.GetDirectoryName(value);

            EditorPrefs.SetString(PrefsString, str);
            LastOpenDirectory = str;
        }
    }
    
    public static string OpenFilePanel(string title, params ExtensionFilter[] filter)
    {
        string[] paths = null;
        object _lock = new object();
        lock (_lock)
        {
            paths = StandaloneFileBrowser.OpenFilePanel(title, DirectoryHistory, filter, false);

            if (paths.Length != 0)
            {
                DirectoryHistory = paths[0];
                return paths[0];
            }
        }

        return null;
    }

    public static string[] OpenFilesPanel(string title, params ExtensionFilter[] filter)
    {
        string[] paths = null;
        object _lock = new object();
        lock (_lock)
        {
            paths = StandaloneFileBrowser.OpenFilePanel(title, DirectoryHistory, filter, true);

            if (paths.Length != 0)
            {
                DirectoryHistory = paths[paths.Length - 1];
                return paths;
            }
        }

        return null;
    }

    public static string OpenFolderPanel(string title)
    {
        object _lock = new object();
        lock (_lock)
        {
            string dir = EditorUtility.OpenFolderPanel(title, DirectoryHistory, null);

            if (!string.IsNullOrEmpty(dir))
            {
                DirectoryHistory = dir;
                return dir;
            }
        }

        return null;
    }

    public static string SaveFilePanel(string title, params ExtensionFilter[] filter)
    {
        object _lock = new object();
        lock (_lock)
        {
            string path = StandaloneFileBrowser.SaveFilePanel(title, DirectoryHistory, "", filter);
            if (!string.IsNullOrWhiteSpace(path))
            {
                DirectoryHistory = path;
                return path;
            }
        }

        return null;
    }

    public static string SaveFilePanel(string title, string openFolderDirectory, params ExtensionFilter[] filter)
    {
        object _lock = new object();
        lock (_lock)
        {
            string path = StandaloneFileBrowser.SaveFilePanel(title, openFolderDirectory, "", filter);
            if (!string.IsNullOrWhiteSpace(path))
                return path;
        }

        return null;
    }

#if UNITY_EDITOR_WIN
    public static string[] OpenFoldersPanel(string title)
    {
        string[] dirs = null;
        object _lock = new object();
        lock (_lock)
        {
            dirs = StandaloneFileBrowser.OpenFolderPanel(title, DirectoryHistory, false);

            if (dirs != null || dirs.Length != 0)
            {
                DirectoryHistory = dirs[0];
                return dirs;
            }

        }

        return null;
    }
#endif
    
    public static string[] GetFilesInFolder(string directory)
    {
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException(directory);

        DirectoryInfo info = new DirectoryInfo(directory);
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

        GetFiles(list, info);

        return list.ToArray();

        static void GetFiles(System.Collections.Generic.List<string> list, DirectoryInfo dirInfo)
        {
            foreach (var i in dirInfo.GetFiles())
                list.Add(i.FullName);

            foreach (var i in dirInfo.GetDirectories())
                GetFiles(list, i);
        }
    }

    public static string[] GetFilterForFilesInFolder(string directory, System.StringComparison comparison = System.StringComparison.CurrentCulture, params string[] extensions)
    {
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException(directory);

        DirectoryInfo info = new DirectoryInfo(directory);
        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

        GetFiles(list, info, comparison, extensions);

        return list.ToArray();

        static void GetFiles(System.Collections.Generic.List<string> list, DirectoryInfo dirInfo, System.StringComparison comparison, params string[] extensions)
        {
            foreach (var i in dirInfo.GetFiles())
            {
                foreach (var ext in extensions)
                {
                    if (i.FullName.EndsWith(ext, comparison))
                        list.Add(i.FullName);
                }
            }

            foreach (var i in dirInfo.GetDirectories())
                GetFiles(list, i, comparison, extensions);
        }
    }

    public static void CopyDirectory(string source, string dest)
    {
        DirectoryInfo sourceInfo = new DirectoryInfo(source);
        if (!sourceInfo.Exists) return;

        DirectoryInfo[] destInfos = sourceInfo.GetDirectories();
        if (!Directory.Exists(dest)) return;

        FileInfo[] info = sourceInfo.GetFiles();
        foreach (FileInfo f in info)
        {
            string tmp = UnityPath.Combine(dest, f.Name);
            f.CopyTo(tmp, true);
        }

        foreach (DirectoryInfo i in destInfos)
        {
            string tmp = UnityPath.Combine(dest, i.Name);
            CopyDirectory(i.FullName, tmp);
        }
    }
    #endregion

}