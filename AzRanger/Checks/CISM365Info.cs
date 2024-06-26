﻿using System;

namespace AzRanger.Checks
{
    internal enum CISLevel
    {
        L1,
        L2,
    }

    internal class CISM365Info
    {
        public string Title { get; private set; }
        public string Section { get; private set; }
        public CISLevel Level { get; private set; }
        public string Version { get; private set; }

        public static bool TryGet(string identifier, out CISM365Info info)
        {
            if (identifier == null || identifier.Length == 0)
            {
                info = null;
                return false;
            }

            var section = CISM365InfoData.GetSectionOrNull(identifier);
            if (section == null)
            {
                info = null;
                return false;
            }

            info = new CISM365Info();
            info.Title = section.GetStringOrThrow("title");
            info.Section = section.GetStringOrThrow("section");
            info.Version = section.GetStringOrThrow("version");

            if (!Enum.TryParse(section.GetStringOrThrow("level"), out CISLevel level))
            {
                info = null;
                return false;
            }

            info.Level = level;
            return true;
        }
    }
}
