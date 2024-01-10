using Life.Network;
using Life.UI;
using System.Linq;
using UIPanelManager;
using UnityEngine;

namespace MyGiveway
{
    abstract class PlayerPanels
    {
        public static void Open(Player player)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"Entrer un code cadeau");

            panel.inputPlaceholder = "_ _ _ _ _ _ _ _";

            panel.AddButton("Confirmer", ui =>
            {
                if (Utils.IsValidGiveway(ui.inputText))
                {
                    if (Utils.IsExistGiveway(ui.inputText, out Giveway giveway))
                    {
                        if (!Utils.IsExpiredGiveway(giveway))
                        {
                            if (!Utils.IsConsumedGiveway(giveway, player))
                            {


                                // attribuer les objets (vérifier slots)
                                // attribuer l'argent
                                // attribuer les véhicules
                                // attribuer les terrains
                                // Ajouter le joueur à la liste
                            }
                            else PanelManager.Notification(player, "Erreur", "Vous avez déjà utilisé ce code", Life.NotificationManager.Type.Error);
                        }
                        else PanelManager.Notification(player, "Erreur", "Ce code est expiré", Life.NotificationManager.Type.Error);
                    }
                    else PanelManager.Notification(player, "Erreur", "Ce code n'existe pas", Life.NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Format du code invalide", Life.NotificationManager.Type.Error);
            });

            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
