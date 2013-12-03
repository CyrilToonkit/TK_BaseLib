using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TK.BaseLib.Presets
{
    public class PresetSystem
    {
        public PresetSystem()
        {

        }

        public PresetSystem(string inName, Type inType)
        {
            _name = inName;
            _presetType = inType;
        }

        private Dictionary<string, SaveablePreset> mPresets = new Dictionary<string,SaveablePreset>();

        protected string _name = "";
        protected Type _presetType = typeof(SaveablePreset);
        protected string _userPath = "";
        protected string _defaultPath = "";
        protected string _sharedPath = "";

        public virtual string UserPath
        {
            get
            {
                return _userPath;
            }
            set { _userPath = value; }
        }

        public virtual string DefaultPath
        {
            get
            {
                return _defaultPath;
            }
            set { _defaultPath = value; }
        }

        public virtual string SharedPath
        {
            get { return _sharedPath; }
            set { _sharedPath = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public SaveablePreset AddPreset(SaveablePreset inPreset)
        {
            if (!mPresets.ContainsKey(inPreset.Name))
            {
                mPresets.Add(inPreset.Name, inPreset);
                inPreset.Path = GetCurrentPath(inPreset);

                return inPreset;
            }

            return null;
        }

        public string SavePreset(string inName)
        {
            string error = "";

            if (mPresets.ContainsKey(inName))
            {
                if (!SavePreset(mPresets[inName]))
                {
                    error += "Error in object serialization !";
                }
            }
            else
            {
                error += string.Format("Preset {0} cannot be found!", GetFullName(inName));
            }

            return error;
        }

        public string LoadPresets()
        {
            string error = LoadPresets(GetCurrentRoot(PresetTypes.Default));
            error += (error == "" ? "" : "\n") + LoadPresets(GetCurrentRoot(PresetTypes.Shared));
            error += (error == "" ? "" : "\n") + LoadPresets(GetCurrentRoot(PresetTypes.User));

            return error;
        }

        public bool HasPreset(string p)
        {
            return mPresets.ContainsKey(p); 
        }

        private string LoadPresets(string inPath)
        {
            if (inPath == "")
            {
                return "Invalid path (" + inPath + ")";
            }

            DirectoryInfo RootDir = new DirectoryInfo(inPath);

            string error = "";

            if (!RootDir.Exists)
            {
                try
                {
                    Directory.CreateDirectory(RootDir.FullName);
                }
                catch (Exception e) { error += "Cannot create directory " + RootDir.FullName + "! (" + e.Message + ")"; }
            }

            if (error == "")
            {
                FileInfo[] inFiles = RootDir.GetFiles();

                foreach (FileInfo CurPref in inFiles)
                {
                    if (CurPref.Extension.Equals(".xml"))
                    {
                        SaveablePreset preset = GetPresetFromPath(CurPref.FullName);
                        if (preset == null || preset.Obsolete)
                        {
                            error += CreateInstance(out preset, CurPref.Name.Substring(0, CurPref.Name.Length - CurPref.Extension.Length));
                        }
                    }
                }
            }

            return error;
        }

        private SaveablePreset GetPresetFromPath(string p)
        {
            foreach (SaveablePreset preset in mPresets.Values)
            {
                if (preset.Path == p)
                {
                    return preset;
                }
            }

            return null;
        }

        public string CreateInstance(out SaveablePreset inPreset, string inName)
        {
            string error = "";

            try
            {
                inPreset = (SaveablePreset)Activator.CreateInstance(_presetType, new object[0]);
                inPreset.Name = inName;
                mPresets.Add(inName, inPreset);
                LoadPreset(inPreset);
            }
            catch (Exception e) { error += "Cannot create instance of type " + _presetType.Name + "! (" + e.Message + ")"; inPreset = null; }

            return error;
        }

        public string LoadPreset(string inName)
        {
            string error = "";

            if (mPresets.ContainsKey(inName))
            {
                if (!LoadPreset(mPresets[inName]))
                {
                    error += "Error in object deserialization !";
                }
            }
            else
            {
                error += string.Format("Preset {0} cannot be found!", GetFullName(inName));
            }

            return error;
        }

        public object GetPresetValue(string inKey)
        {
            string[] keySplit = inKey.Split('.');

            if (!mPresets.ContainsKey(keySplit[0]))
            {
                //see if there is a corresponding file
                SaveablePreset preset = LoadPresetFile(GetCurrentRoot(PresetTypes.Default) + "\\" + keySplit[0] + ".xml");
                if (preset != null)
                {
                    return preset.GetPropertyValue(keySplit[1]);
                }
            }
            else
            {
                return mPresets[keySplit[0]].GetPropertyValue(keySplit[1]);
            }

            return null;
        }

        private SaveablePreset LoadPresetFile(string inPath)
        {
            FileInfo info = new FileInfo(inPath);

            if (info.Exists && info.Extension.Equals(".xml"))
            {
                SaveablePreset preset = null;
                if (CreateInstance(out preset, info.Name.Substring(0, info.Name.Length - info.Extension.Length)) == "")
                {
                    return preset;
                }
            }

            return null;
        }

        public bool SetPresetValue(string inKey, object inValue)
        {
            string[] keySplit = inKey.Split('.');
            SaveablePreset preset = null;

            if (!mPresets.ContainsKey(keySplit[0]))
            {
                //see if there is a corresponding file
                preset = LoadPresetFile(GetCurrentRoot(PresetTypes.Default) + "\\" + keySplit[0] + ".xml");
            }
            else
            {
                preset = mPresets[keySplit[0]];
            }

            if (preset != null)
            {
                preset.SetPropertyValue(keySplit[1], inValue);
            }

            return preset != null;
        }

        private bool LoadPreset(SaveablePreset saveablePreset)
        {
            return saveablePreset.LoadPreset(GetCurrentPath(saveablePreset));
        }

        private bool SavePreset(SaveablePreset saveablePreset)
        {
            return saveablePreset.Save(GetCurrentPath(saveablePreset));
        }

        public void SavePresets()
        {
            foreach (SaveablePreset preset in mPresets.Values)
            {
                preset.SavePreset();
            }
        }

        private string GetCurrentPath(SaveablePreset saveablePreset)
        {
            return GetCurrentRoot(saveablePreset.PresetType) + "\\" + saveablePreset.Name + ".xml";
        }

        private string GetCurrentRoot(PresetTypes inType)
        {
            switch (inType)
            {
                case PresetTypes.Default:
                    return DefaultPath + "\\" + Name;
                case PresetTypes.Shared:
                    return SharedPath + "\\" + Name;
            }

            return UserPath + "\\" + Name;
        }

        private string GetFullName(SaveablePreset inPreset)
        {
            return _name + "_" + inPreset.Name;
        }

        private string GetFullName(string inPresetName)
        {
            return _name + "_" + inPresetName;
        }
    }
}
