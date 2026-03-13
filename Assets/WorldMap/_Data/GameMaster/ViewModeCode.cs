using System;
using GOA.WorldMap;

namespace GOA.WorldMap
{
    public class ViewModeCodeParser
    {
        public static ViewModeCode FromString(string name)
        {
            try
            {
                //name = name.ToLower();
                name = name.Substring(0,1).ToLower() + name.Substring(1);
                return (ViewModeCode)Enum.Parse(typeof(ViewModeCode), name);
            }
            catch (System.Exception)
            {
                return ViewModeCode.unknown;
            }
        }
    }

    public enum ViewModeCode
    {
        unknown = 0,
        per = 1,
        god = 2
    
    }
}
