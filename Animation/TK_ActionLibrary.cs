using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace TK.BaseLib.Animation
{
    public class TK_ActionLibrary
    {
        public static int THUMBSIZE = 200;
        public static int SEQWIDTH = 640;
        public static int SEQHEIGHT = 480;

        public TK_ActionLibrary()
        {
            Errors = Load();
        }

        public string Load()
        {
            string err = LoadActions();
            err += LoadMeta();

            return err;
        }

        public virtual string GetRepository()
        {
            return "";
        }

        XmlSerializer actionSerializer = new XmlSerializer(typeof(TK_Action));
        XmlSerializer modelInfosSerializer = new XmlSerializer(typeof(ModelInfo));
        XmlSerializer controlsMapSerializer = new XmlSerializer(typeof(ControlsMap));

        string errors = "";
        public string Errors
        {
            get { return errors; }
            set { errors = value; }
        }

        public AdvOptions Options = new AdvOptions();

        public ControlsMap Maps = new ControlsMap();

        public Dictionary<string, ModelInfo> ModelInfos = new Dictionary<string,ModelInfo>();

        public ModelInfo RefModelInfo = null;

        public List<TK_Action> Actions = new List<TK_Action>();

        public bool HasAction(string inName)
        {
            foreach (TK_Action action in Actions)
            {
                if (action.Name == inName)
                {
                    return true;
                }
            }

            return false;
        }

        public TK_Action GetAction(string inName)
        {
            foreach (TK_Action action in Actions)
            {
                if (action.Name == inName)
                {
                    return action;
                }
            }

            return null;
        }

        public string LoadActions()
        {
            string errors = "";

            Actions.Clear();

            string sRepo = GetRepository();

            if (!string.IsNullOrEmpty(sRepo))
            {
                DirectoryInfo repo = new DirectoryInfo(sRepo);
                if (repo.Exists)
                {
                    DirectoryInfo[] subDirs = repo.GetDirectories();
                    foreach (DirectoryInfo dir in subDirs)
                    {
                        TK_Action action = null;
                        FileInfo[] infos = dir.GetFiles("*.act");
                        if (infos.Length > 0)
                        {
                            FileStream stream = null;
                            stream = infos[0].OpenRead();
                            try
                            {
                                action = (TK_Action)actionSerializer.Deserialize(stream);
                                action.Library = this;
                                if (action.Validate())
                                {
                                    Actions.Add(action);
                                }
                            }
                            catch (Exception e) { errors += "Load xml " + e.Message; }
                            
                            stream.Close();
                        }
                    }
                }
            }

            return errors;
        }

        public string LoadMeta()
        {
            string errors = "";

            string sRepo = GetRepository();

            if (!string.IsNullOrEmpty(sRepo))
            {
                DirectoryInfo repo = new DirectoryInfo(sRepo);
                if (repo.Exists)
                {
                    //Control Maps
                    FileInfo MapsFile = new FileInfo(sRepo + "\\ControlMaps.xml");

                    if (MapsFile.Exists)
                    {
                        FileStream stream = null;
                        stream = MapsFile.OpenRead();
                        try
                        {
                            Maps = (ControlsMap)controlsMapSerializer.Deserialize(stream);
                        }
                        catch (Exception e) { errors += "Load xml " + e.Message; }

                        stream.Close();
                    }

                    if (!Maps.Initialize())
                    {
                        SaveMap();
                    }

                    //Model infos
                    ModelInfos.Clear();

                    DirectoryInfo modelinfosFile = new DirectoryInfo(sRepo + "\\ModelInfos");
                    if (modelinfosFile.Exists)
                    {
                        FileInfo[] infos = modelinfosFile.GetFiles("*.xml");
                        if (infos.Length > 0)
                        {
                            foreach (FileInfo info in infos)
                            {
                                FileStream stream = null;
                                stream = info.OpenRead();
                                try
                                {
                                    ModelInfo modelInfo = (ModelInfo)modelInfosSerializer.Deserialize(stream);
                                    modelInfo.Name = info.Name.Substring(0, info.Name.Length - 4);

                                    if (modelInfo.Name == "REF")
                                    {
                                        RefModelInfo = modelInfo;
                                    }
                                    else
                                    {
                                        ModelInfos.Add(modelInfo.Name, modelInfo);
                                    }
                                }
                                catch (Exception e) { errors += "Load Animation Meta " + e.Message; }

                                stream.Close();
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            modelinfosFile.Create();
                        }
                        catch (Exception e) { errors += "Action Library : Create folder error (" + e.Message + ")"; }
                    }

                    if (RefModelInfo == null)
                    {
                        //Create "Ref" modelInfos
                        RefModelInfo = new ModelInfo("REF");
                        Rescaler spine = new Rescaler("SpineScaler", "#SPINETOP", "#SPINEBOTTOM", 1);
                        spine.AffectedControls.Add("#SPINEIKTOP");
                        spine.AffectedControls.Add("#SPINEIKBOTTOM");
                        RefModelInfo.Scalers.Add(spine);

                        SaveModelInfo(RefModelInfo);
                    }
                }
            }

            return errors;
        }

        public string SaveMap()
        {
            string errors = "";
            string sRepo = GetRepository();

            if (sRepo != null)
            {
                DirectoryInfo repo = new DirectoryInfo(sRepo);
                if (repo.Exists)
                {
                    FileInfo MapsFile = new FileInfo(sRepo + "\\ControlMaps.xml");
                    FileStream stream = null;

                    try
                    {
                        stream = MapsFile.Create();
                        controlsMapSerializer.Serialize(stream, Maps);
                        stream.Close();
                    }
                    catch (Exception exept) { errors += "Save Map error : " + exept.Message; if (stream != null) { stream.Close(); } }

                }
            }

            return errors;
        }

        public string SaveModelInfo(ModelInfo info)
        {
            string errors = "";
            string sRepo = GetRepository();

            if (sRepo != null)
            {
                DirectoryInfo repo = new DirectoryInfo(sRepo);
                if (repo.Exists)
                {
                    DirectoryInfo modelinfosFile = new DirectoryInfo(sRepo + "\\ModelInfos");
                    if (!modelinfosFile.Exists)
                    {
                        modelinfosFile.Create();
                    }

                    FileInfo infosFile = new FileInfo(modelinfosFile.FullName + "\\" + info.Name + ".xml");
                    FileStream stream = null;

                    try
                    {
                        stream = infosFile.Create();
                        modelInfosSerializer.Serialize(stream, info);
                        stream.Close();
                    }
                    catch (Exception exept) { errors += "Save Map error : " + exept.Message; if (stream != null) { stream.Close(); } }
                }
            }

            return errors;
        }

        public string GetUniqueName(string inName)
        {
            bool unique = GetAction(inName) == null;

            while (!unique)
            {
                inName = Increment(inName);
                unique = GetAction(inName) == null;
            }

            return inName;
        }

        public string Increment(string inName)
        {
            string root = inName;
            Match m = Regex.Match(inName, "\\d+$");
            int number = 0;
            if (m.Success)
            {
                number = Convert.ToInt32(m.Value);
                root = inName.Substring(0, inName.Length - m.Value.Length);
            }
            number++;
            return string.Format("{0}{1}", root, number);
        }

        public virtual string SaveAction(TK_Action action)
        {
            string errors = "";
            string sRepo = GetRepository();

            if (sRepo != null)
            {
                DirectoryInfo repo = new DirectoryInfo(sRepo);
                if (repo.Exists)
                {
                    DirectoryInfo actionDir = new DirectoryInfo(GetRepository() + "\\" + action.Name);
                    if (!actionDir.Exists)
                    {
                        actionDir.Create();
                    }

                    StreamWriter myWriter = null;

                    try
                    {
                        //Export the xml
                        myWriter = new StreamWriter(actionDir.FullName + "\\" + action.Name + ".act");
                        actionSerializer.Serialize(myWriter, action);
                    }
                    catch (Exception e) { errors += "Save xml " + e.Message; }
                    myWriter.Close();
                }
            }

            return errors;
        }

        public void DeleteAction(TK_Action inAction)
        {
            DirectoryInfo dInfo = new DirectoryInfo(inAction.GetPath());
            if (dInfo.Exists)
            {
                dInfo.Delete(true);
            }

            if (Actions.Contains(inAction))
            {
                Actions.Remove(inAction);
            }
        }

        public virtual void ApplyActions(List<TK_Action> inActions, double inFrame, ApplyMode inMode, double inRetime)
        {

        }

        public virtual void StoreAction()
        {

        }

        public virtual void EditMaps()
        {
            
        }

        public virtual void EditModelInfo(string modelName)
        {

        }

        public virtual void ShowSequence(TK_Action inAction)
        {

        }

        public ModelInfo UpdateInfo(string modelName)
        {
            ModelInfo curInfo = null;
            bool updated = false;

            if (!ModelInfos.ContainsKey(modelName))
            {
                curInfo = new ModelInfo(modelName);
                ModelInfos.Add(modelName, curInfo);
                updated = true;
            }
            else
            {
                curInfo = ModelInfos[modelName];
            }

            //Sync modelInfo with Ref

            List<Rescaler> toCreate = new List<Rescaler>();

            foreach (Rescaler scaler in RefModelInfo.Scalers)
            {
                if (!curInfo.HasScaler(scaler))
                {
                    toCreate.Add(scaler);
                }
            }

            if(toCreate.Count > 0)
            {
                curInfo.Scalers.AddRange(CreateScalers(toCreate));
                updated = true;
            }

            if (updated)
            {
                SaveModelInfo(curInfo);
            }

            return curInfo;
        }

        public virtual List<Rescaler> CreateScalers(List<Rescaler> refScalers)
        {
            List<Rescaler> empty = new List<Rescaler>();

            foreach (Rescaler scaler in refScalers)
            {
                empty.Add(new Rescaler());
            }

            return empty;
        }

        public string GetName()
        {
            return "Actions Library (" + GetRepository() + ")";
        }
    }
}
