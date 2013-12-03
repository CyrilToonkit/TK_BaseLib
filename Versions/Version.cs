using System;
using System.Collections.Generic;
using System.Text;

//COMMENTS OK
namespace TK.BaseLib.Versions
{
    /// <summary>
    /// Class to hold version info
    /// </summary>
    public class Version
    {
        // == CONSTRUCTORS ================================================================

        // == MEMBERS =====================================================================

        /// <summary>
        /// Major version number
        /// </summary>
        public int Major = 0;
        /// <summary>
        /// Minor version number
        /// </summary>
        public int Minor = 0;
        /// <summary>
        /// Build version number
        /// </summary>
        public int Build = 0;

        /// <summary>
        /// Build date of the version
        /// </summary>
        public DateTime BuildDate = DateTime.Now;

        /// <summary>
        /// Details of what was done in this version
        /// </summary>
        public string BuildComment = "";

        /// <summary>
        /// A path where the version is backuped
        /// </summary>
        public string BuildBackup = "";

        // == METHODS =====================================================================

        /// <summary>
        /// Get the version as formatted string (Major.Minor.Build)
        /// </summary>
        /// <returns>The formatted string</returns>
        internal string GetVersion()
        {
            return (Major == 0 ? "BETA " : "V " + Major.ToString() + ".") + Minor.ToString();
        }

        /// <summary>
        /// Get the version Date as formatted string (ShortDate ShortTime)
        /// </summary>
        /// <returns>The formatted string</returns>
        internal string GetDate()
        {
            return BuildDate.ToShortDateString() + " " + BuildDate.ToShortTimeString();
        }
    }
}