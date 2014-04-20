using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Malarkey
{
    class SettingsManager
    {
        private static SettingsManager manager = new SettingsManager();

        public Boolean fullscreen { get; private set; }

        public int screen_width { get; private set; }
        public int screen_height { get; private set; }

        public static SettingsManager GetInstance()
        {
            
            return manager;
        }

        public SettingsManager()
        {
            fullscreen = false;

            screen_width = 1024;
            screen_height = 768;

        }
    }
}
