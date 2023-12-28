using Life;
using Life.UI;
using System;
using UnityEngine;
using MyMenu.Entities;
using System.Collections.Generic;

namespace MyGiveway
{
    public class Main : Plugin
    {
        public static List<Giveway> GivewayList = new List<Giveway>();
        public Main(IGameAPI api) : base(api)
        {
        }

        public override void OnPluginInit()
        {
            base.OnPluginInit();

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
    }
}