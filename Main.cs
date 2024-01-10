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
                Section adminSection = new Section("Admin_"+Section.GetSourceName(), Section.GetSourceName(), "v1.0.0", "Aarnow");
                Action<UIPanel> adminAction = ui => AdminPanels.Open(adminSection.GetPlayer(ui));
                adminSection.OnlyAdmin = true;
                adminSection.Line = new UITabLine(adminSection.Title, adminAction);
                adminSection.Insert(true);

                Section playerSection = new Section("Player_"+Section.GetSourceName(), Section.GetSourceName(), "v1.0.0", "Aarnow");
                Action<UIPanel> playerAction = ui => PlayerPanels.Open(playerSection.GetPlayer(ui));
                playerSection.OnlyAdmin = false;
                playerSection.Line = new UITabLine(playerSection.Title, playerAction);
                playerSection.Insert(false);
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