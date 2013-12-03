using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TK.BaseLib
{
    public enum PresetTypes
    {
        Default, User, Shared
    }

    public class SaveablePreset : SaveableData
    {
        public SaveablePreset() : base()
        {
        }

        public SaveablePreset(string inName, PresetTypes inType)
        {
            _name = inName;
            _presetType = inType;
        }

        PresetTypes _presetType = PresetTypes.Default;

        string _path = "";

        DateTime _fileDate = new DateTime(0);

        DateTime _classDate = new DateTime(0);

        string _name = "";

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Path
        {
          get { return _path; }
          set { _path = value; }
        }

        public PresetTypes PresetType
        {
            get { return _presetType; }
            set { _presetType = value; }
        }

        public bool Obsolete
        {
            get { return (_fileDate.Ticks == 0 || _classDate.Ticks == 0 || _fileDate > _classDate); }
        }

        internal object GetPropertyValue(string inPropertyName)
        {
            return ReflectionHelper.GetProperty(this, inPropertyName, false);
        }

        internal object SetPropertyValue(string inPropertyName, object inValue)
        {
            return ReflectionHelper.SetProperty(this, inPropertyName, inValue, true);
        }

        public override void Clone(SaveableData Data)
        {
            SaveablePreset preset = Data as SaveablePreset;

            if (preset != null)
            {
                _name = preset.Name;
                _presetType = preset.PresetType;
            }
        }

        internal bool LoadPreset(string inPath)
        {
            FileInfo info = new FileInfo(inPath);

            if (info.Exists)
            {
                _path = inPath;
                _fileDate = info.LastWriteTime;

                Load(inPath);

                _classDate = DateTime.Now;

                return true;
            }

            return false;
        }

        internal void SavePreset()
        {
            Save(_path);

            FileInfo info = new FileInfo( _path);

            _fileDate = info.LastWriteTime;
        }
    }
}
