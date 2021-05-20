using System;

namespace RksAdventure.Editors
{
    public class Utility
    {
        public static string GetTimeName()
        => Convert.ToInt64($"{DateTime.Now:yyMMddHHmmssff}").ToString("X");
    }
}
