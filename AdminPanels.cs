using Life.Network;
using Life.UI;
using UIPanelManager;
using UnityEngine;

namespace MyGiveway
{
    abstract class AdminPanels
    {
        public static void Open(Player player)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Tab).SetTitle($"MyGiveway");

            if(Main.GivewayList.Count == 0)
            {
                panel.AddTabLine($"Aucun code", ui => Debug.Log("delete"));
            } else
            {
                foreach (Giveway giveway in Main.GivewayList)
                {
                    panel.AddTabLine($"{giveway.Name}", ui => Debug.Log("delete"));
                }
            }
            

            panel.AddButton("Ajouter", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player)));
            panel.AddButton("Supprimer", ui =>
            {
                ui.SelectTab();
                PanelManager.NextPanel(player, ui, () => Open(player));
            });
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetupGiveway(Player player, Giveway gw = null)
        {
            if(gw == null) gw = new Giveway();

            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Tab).SetTitle($"MyGiveway");
            
            panel.AddTabLine($"Nom: {gw.Name}", ui => PanelManager.NextPanel(player,ui,()=> SetName(player, gw)));
            panel.AddTabLine($"Code: {gw.Code}", ui => Debug.Log("edit property - input"));
            panel.AddTabLine($"Date d'expiration: {gw.ExpirationDate.ToString("dd/MM/yyyy - HH'h'mm")}", ui => Debug.Log("edit property - input"));
            panel.AddTabLine($"Etat: {(gw.IsActive ? "Actif" : "Inactif")}", ui => Debug.Log("edit property"));
            panel.AddTabLine($"Usage: {(gw.IsSingleUse ? "unique" : "une fois par joueur")}", ui => Debug.Log("edit property"));
            panel.AddTabLine($"Argent: {gw.Money}", ui => Debug.Log("définir/modifier la somme"));
            panel.AddTabLine($"Terrains: {gw.AreasId.Count}", ui => Debug.Log("voir la liste"));
            panel.AddTabLine($"Véhicules: {gw.VehiclesId.Count}", ui => Debug.Log("voir la liste"));
            panel.AddTabLine($"Objets: {gw.ItemsId.Count}", ui => Debug.Log("voir la liste"));

            panel.AddButton("Modifier", ui => ui.SelectTab());
            panel.AddButton("Sauvegarder", ui => Debug.Log("Save"));
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, ()=> Open(player)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void SetName(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"MyGiveway");

            panel.inputPlaceholder = "Nommer votre code";

            panel.AddButton("Valider", ui =>
            {
                if (ui.inputText.Length > 0)
                {
                    gw.Name = ui.inputText;
                    PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw));
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez renseigner une valeur", Life.NotificationManager.Type.Error);
            });
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
