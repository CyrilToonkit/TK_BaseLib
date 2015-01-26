using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation
{
    public class AdvOptions
    {
        bool autoKeyPose = false;
        public bool AutoKeyPose
        {
            get { return autoKeyPose; }
            set { autoKeyPose = value; }
        }

        bool retarget = true;
        public bool Retarget
        {
            get { return retarget; }
            set { retarget = value; }
        }

        bool openRemapper = true;
        public bool OpenRemapper
        {
            get { return openRemapper; }
            set { openRemapper = value; }
        }

        bool alwaysOpenRemapper = false;
        public bool AlwaysOpenRemapper
        {
            get { return alwaysOpenRemapper; }
            set { alwaysOpenRemapper = value; }
        }

        bool resize = true;
        public bool Resize
        {
            get { return resize; }
            set { resize = value; }
        }

        bool clean = false;
        public bool Clean
        {
            get { return clean; }
            set { clean = value; }
        }

        int cleanMargin = 5;
        public int CleanMargin
        {
            get { return cleanMargin; }
            set { cleanMargin = value; }
        }

        bool forceResize = false;
        public bool ForceResize
        {
            get { return forceResize; }
            set { forceResize = value; }
        }

        double forcedResizing = 1.0;
        public double ForcedResizing
        {
            get { return forcedResizing; }
            set { forcedResizing = value; }
        }

        bool useNativeFormats = false;
        public bool UseNativeFormats
        {
            get { return useNativeFormats; }
            set { useNativeFormats = value; }
        }
    }
}
