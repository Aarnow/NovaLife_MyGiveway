using Life;
using Life.UI;
using System;
using UnityEngine;
using MyMenu.Entities;
using System.Collections.Generic;
using System.IO;
using Life.InventorySystem;
using Newtonsoft.Json;

namespace MyGiveway
{
    public class Main : Plugin
    {
        public static string directoryPath;
        public static List<Giveway> GivewayList = new List<Giveway>();
        public Main(IGameAPI api) : base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();
            InitDirectory();
            InitGiveway();

            //MyMenu
            try
            {
                Section section = new Section(Section.GetSourceName(), Section.GetSourceName(), "v1.0.0", "Aarnow");
                Action<UIPanel> action = ui => AdminPanels.Open(section.GetPlayer(ui));
                section.OnlyAdmin = true;
                section.Line = new UITabLine(section.Title, action);
                section.Insert(true);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }

            Debug.Log($"Plugin \"MyGiveway\" initialisé avec succès.");
        }

        public void InitGiveway()
        {
            string[] jsonFiles = Directory.GetFiles(Main.directoryPath, "*.json");
            foreach (string file in jsonFiles)
            {
                string json = File.ReadAllText(file);
                Giveway gw = JsonConvert.DeserializeObject<Giveway>(json);
                GivewayList.Add(gw);
            }
        }

        public void InitDirectory()
        {
            directoryPath = pluginsPath + "/MyGiveway";
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }
    }
}