using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.Principal;
using System.Xml.Serialization;
using TK.BaseLib.Picture;

namespace TK.BaseLib.Animation
{
    public enum ApplyMode
    {
        All, OnSelection, IgnoreSelection
    }

    public enum ActionTypes
    {
        Pose, Anim, Cycle
    }

    public class TK_Action
    {
        public TK_Action()
        {

        }

        public TK_Action(string inName, string inModel, ActionTypes inType, string inProject, string inCategory, double inStartFrame, double inEndFrame, TK_ActionLibrary inLib)
        {
            Name = inName;
            Type = inType;
            Project = inProject;
            Category = inCategory;
            StartFrame = inStartFrame;
            EndFrame = inEndFrame;
            Library = inLib;
            Model = inModel;

            mCreatorName = WindowsIdentity.GetCurrent().Name.Split("\\".ToCharArray())[1];
            Birth = DateTime.Now;
        }

        [XmlIgnore]
        public TK_ActionLibrary Library = null;

        private ActionTypes mType = ActionTypes.Pose;
        public ActionTypes Type
        {
            get { return mType; }
            set { mType = value; }
        }

        private string mProject = "";
        public string Project
        {
            get { return mProject; }
            set { mProject = value; }
        }

        private string mCategory = "Undefined";
        public string Category
        {
            get { return mCategory; }
            set { mCategory = value; }
        }

        private string mCreatorName = "" ;
        public string CreatorName
        {
            get { return mCreatorName; }
            set { mCreatorName = value; }
        }

        private DateTime mBirth = new DateTime(0);
        public DateTime Birth
        {
            get { return mBirth; }
            set { mBirth = value; }
        }

        private string mName = "";
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        private string mModel = "";
        public string Model
        {
            get { return mModel; }
            set { mModel = value; }
        }

        private double mStartFrame = 0;
        public double StartFrame
        {
            get { return mStartFrame; }
            set { mStartFrame = value; }
        }

        private double mEndFrame = 0;
        public double EndFrame
        {
            get { return mEndFrame; }
            set { mEndFrame = value; }
        }

        [XmlIgnore]
        public double Duration
        {
            get { return mEndFrame - mStartFrame + 1; }
        }

        [XmlIgnore]
        public Image Thumbnail = null;

        public bool Validate()
        {
            DirectoryInfo actionDir = new DirectoryInfo(GetPath());
            if (actionDir.Exists)
            {
                FileInfo thumbFile = new FileInfo(GetThumbPath());
                if (thumbFile.Exists)
                {
                    FileStream fs = new FileStream(thumbFile.FullName, FileMode.Open);
                    Image Thumb = Image.FromStream(fs);
                    Thumbnail = (Image)new Bitmap(Thumb);
                    fs.Close();
                }
                else
                {
                    Thumbnail = (Image)PictureHelper.CreateBitmapImage(TK_ActionLibrary.THUMBSIZE, TK_ActionLibrary.THUMBSIZE, "Image not found", Color.Gray, Color.White);
                }
            }

            return Thumbnail != null;
        }

        public string GetThumbPath()
        {
            return GetPath() + "\\" + Name + "_thumb.jpg";
        }

        public string GetSeqPath()
        {
            return GetPath() + "\\Sequence";
        }

        public string GetPath()
        {
            return Library.GetRepository() + "\\" + Name;
        }
    }
}
