using Life;
using Life.AreaSystem;
using Life.DB;
using Life.InventorySystem;
using Life.Network;
using Life.UI;
using System;
using System.Collections.Generic;
using UIPanelManager;

namespace MyGiveway.Panels
{
    abstract class PlayerPanels
    {
        public static void Open(Player player)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"Entrer un code cadeau");

            panel.AddButton("Confirmer", ui =>
            {
                if (Utils.IsExistGiveway(ui.inputText, out Giveway giveway))
                {
                    if (!Utils.IsExpiredGiveway(giveway))
                    {
                        if (!Utils.IsConsumedGiveway(giveway, player))
                        {
                            string report = "\n<color=green>Vous venez d'obtenir:</color>";

                            //items
                            int slotAvailable = (12 - player.setup.inventory.CheckForEmptySlot());
                            int slotRequired = 0;
                            foreach (KeyValuePair<int, int> kvp in giveway.ItemsId)
                            {
                                int itemId = kvp.Key;
                                int quantity = kvp.Value;
                                Item item = Nova.man.item.GetItem(itemId);                                
                                slotRequired += item.maxSlotCount == 0 ? 1 : (int)Math.Ceiling((double)quantity / item.maxSlotCount);
                            }

                            if(slotAvailable >= slotRequired)
                            {
                                foreach (KeyValuePair<int, int> kvp in giveway.ItemsId)
                                {
                                    int itemId = kvp.Key;
                                    int quantity = kvp.Value;
                                    Item item = Nova.man.item.GetItem(itemId);
                                    player.setup.inventory.AddItem(itemId, quantity, Utils.itemWithData(itemId));
                                    report += $"\n<color=orange>{item.itemName}</color> x <color=yellow>{quantity}</color>";
                                }
                            }
                            else PanelManager.Notification(player, "Oups !", $"Vous devez libérer d'avantage d'espace dans votre inventaire", NotificationManager.Type.Info);

                             //money
                            if (giveway.Money != 0)
                            {
                                player.AddBankMoney(giveway.Money);
                                report += $"\n<color=yellow>{giveway.Money}€ sur votre compte en banque</color>";
                            }
                                

                            //vehicle
                            foreach (KeyValuePair<int, int> kvp in giveway.VehiclesId)
                            {
                                int vehicleId = kvp.Key;
                                int quantity = kvp.Value;
                                
                                for (int i = 0; i < quantity; i++)
                                {
                                    LifeDB.CreateVehicle(vehicleId, $"{{\"owner\":{{\"groupId\":0,\"characterId\":{player.character.Id}}},\"coOwners\":[]}}");
                                }

                                report += $"\n<color=orange>{Nova.v.vehicleModels[vehicleId].vehicleName}</color> x <color=yellow>{quantity}</color>";
                            }

                            //area
                            foreach (int areaId in giveway.AreasId)
                            {
                                LifeArea area = Nova.a.GetAreaById((uint)areaId);
                                area.permissions.owner.characterId = player.character.Id;
                                area.Save();
                                report += $"\nla propriété du terrain n°<color=orange>{areaId}</color>";
                            }
                            
                            giveway.PlayersId.Add(player.account.id);
                            giveway.Update();
                            PanelManager.NextPanel(player, ui, () => Success(player, report)); 
                        }
                        else PanelManager.Notification(player, "Erreur", "Vous avez déjà utilisé ce code", NotificationManager.Type.Error);
                    }
                    else PanelManager.Notification(player, "Erreur", "Ce code est expiré", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Ce code n'existe pas", NotificationManager.Type.Error);
            });

            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }

        public static void Success(Player player, string report)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Text).SetTitle($"Code validé");

            panel.text = report;

            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
