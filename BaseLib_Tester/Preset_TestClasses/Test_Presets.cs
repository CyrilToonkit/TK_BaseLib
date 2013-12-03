using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TK.BaseLib.Time;
using TK.BaseLib.Animation.KeysCurves;
using TK.BaseLib.Presets;
using BaseLib_Tester.Preset_TestClasses;
using TK.BaseLib;

namespace BaseLib_Tester
{
    static class Test_Presets
    {
        public static void Execute()
        {
            PresetSystem system = new PresetSystem("Test", typeof(Power));
            system.UserPath = "C:\\temp";

            system.LoadPresets();
            /*
            if (system.AddPreset(new Power("First", PresetTypes.User)) == null)
            {
                Console.WriteLine("Preset is null");
            }
            */

            string text = "Nothing";

            while (text != "" && text != "Exit")
            {
                text = Console.ReadLine();
                ParseCommand(text, system);
            }

            system.SavePresets();

            /*
            Console.WriteLine(system.GetPresetValue("First.Value").ToString());

            system.SetPresetValue("First.Value", 2);

            Console.WriteLine(system.GetPresetValue("First.Value").ToString());

            Console.WriteLine(system.GetPresetValue("Second.Value").ToString());

            Console.WriteLine(system.GetPresetValue("First.Value2").ToString());
             */
        }

        private static void ParseCommand(string text, PresetSystem inSystem)
        {
            if (text == "" || text == "Exit")
            {
                return;
            }

            string[] splitText = text.Split(' ');
            object value = null;
            if(splitText.Length > 1)
            {
                switch(splitText[0])
                {
                    case "Add":
                        if (!inSystem.HasPreset(splitText[1]))
                        {
                            inSystem.AddPreset(new Power(splitText[1], PresetTypes.User));
                        }
                        else
                        {
                            Console.WriteLine("Preset " + splitText[1] + " already exists");
                        }
                        
                        break;

                    case "Get":
                        value = inSystem.GetPresetValue(splitText[1]);

                        if (value == null)
                        {
                            Console.WriteLine("Can't find Preset : " + splitText[1]);
                        }
                        else
                        {
                            Console.WriteLine(splitText[1] + " = " + value.ToString());
                        }
                        

                        break;

                    case "Set" :
                        if(splitText.Length > 2)
                        {
                            if (inSystem.HasPreset(splitText[1].Split('.')[0]))
                            {
                                inSystem.SetPresetValue(splitText[1], splitText[2]);
                            }
                            else
                            {
                                Console.WriteLine("Can't find Preset : " + splitText[1]);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong number of arguments (at least 3 expected)");
                        }
                        break;
                }
            }
            else
            {
                Console.WriteLine("Wrong number of arguments (at least 2 expected)");
            }
        }
    }
}
