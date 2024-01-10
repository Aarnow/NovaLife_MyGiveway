using Life.AreaSystem;
using Life;
using Life.Network;
using Life.UI;
using System;
using UIPanelManager;
using UnityEngine;
using Life.VehicleSystem;
using System.Collections.Generic;
using Life.InventorySystem;

namespace MyGiveway.Panels
{
    abstract class AdminPanels
    {
        public static void Open(Player player)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Tab).SetTitle($"MyGiveway");

            if (Main.GivewayList.Count == 0)
            {
                panel.AddTabLine($"Aucun code", ui => Debug.Log("delete"));
            }
            else
            {
                foreach (Giveway giveway in Main.GivewayList)
                {
                    panel.AddTabLine($"{giveway.Name} (<color=yellow>{giveway.Code}</color>)", ui => giveway.Delete(player));
                }
            }

            panel.AddButton("Ajouter", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player)));
            panel.AddButton("Modifier", ui =>
            {
                Giveway giveway = Main.GivewayList[ui.selectedTab];
                if (giveway != null) PanelManager.NextPanel(player, ui, () => SetupGiveway(player, giveway));
                else PanelManager.Notification(player, "Erreur", "Vous devez sélectionner un Giveway afin de le modifier", NotificationManager.Type.Error);
            });
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
            if (gw == null) gw = new Giveway();

            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Tab).SetTitle($"MyGiveway");

            panel.AddTabLine($"Nom: {gw.Name}", ui => PanelManager.NextPanel(player, ui, () => SetName(player, gw)));
            panel.AddTabLine($"Code: {gw.Code}", ui => PanelManager.NextPanel(player, ui, () => SetCode(player, gw)));
            panel.AddTabLine($"Date d'expiration: {gw.ExpirationDate.ToString("dd/MM/yyyy - HH'h'mm")}", ui => PanelManager.NextPanel(player, ui, () => SetDate(player, gw)));
            panel.AddTabLine($"Etat: {(gw.IsActive ? "Actif" : "Inactif")}", ui =>
            {
                gw.IsActive = !gw.IsActive;
                PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw));
            });
            panel.AddTabLine($"Usage: {(gw.IsSingleUse ? "unique" : "une fois par joueur")}", ui =>
            {
                gw.IsSingleUse = !gw.IsSingleUse;
                PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw));
            });
            panel.AddTabLine($"Argent: {gw.Money}", ui => PanelManager.NextPanel(player, ui, () => SetMoney(player, gw)));
            panel.AddTabLine($"Terrains: {(gw.AreasId.Count == 0 ? "aucun" : $"{gw.AreasId.Count}")}", ui => PanelManager.NextPanel(player, ui, () => AreaList(player, gw)));
            panel.AddTabLine($"Véhicules: {(gw.VehiclesId.Count == 0 ? "aucun" : $"{gw.VehiclesId.Count}")}", ui => PanelManager.NextPanel(player, ui, () => VehiclesList(player, gw)));
            panel.AddTabLine($"Objets: {(gw.ItemsId.Count == 0 ? "aucun" : $"{gw.ItemsId.Count}")}", ui => PanelManager.NextPanel(player, ui, () => ItemsList(player, gw)));

            panel.AddButton("Modifier", ui => ui.SelectTab());
            panel.AddButton("Sauvegarder", ui =>
            {
                if (gw.IsSaved) gw.Update();
                else
                {
                    gw.IsSaved = true;
                    gw.Save();
                    Main.GivewayList.Add(gw);
                }
                PanelManager.NextPanel(player, ui, () => Open(player));
            });
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, () => Open(player)));
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
                else PanelManager.Notification(player, "Erreur", "Vous devez renseigner une valeur", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void SetCode(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"MyGiveway");

            panel.inputPlaceholder = "Modifier le code (3 caractères minimum)";

            panel.AddButton("Valider", ui =>
            {
                if (ui.inputText.Length > 2)
                {
                    gw.Code = ui.inputText;
                    PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw));
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez renseigner un code de 3 caractères minimum", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void SetDate(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"MyGiveway");

            panel.inputPlaceholder = $"Exemple: {DateTime.Now.ToString("dd/MM/yyyy - HH'h'mm")}";

            panel.AddButton("Valider", ui =>
            {
                if (Utils.TryParseDateTime(ui.inputText, out DateTime dateTime))
                {
                    gw.ExpirationDate = dateTime;
                    PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw));
                }
                else
                {
                    PanelManager.NextPanel(player, ui, () => SetDate(player, gw));
                    PanelManager.Notification(player, "Erreur", "Veuillez respecter l'exemple", NotificationManager.Type.Error);
                }
            });
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void SetMoney(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"MyGiveway");

            panel.inputPlaceholder = "Définir un montant";

            panel.AddButton("Valider", ui =>
            {
                if (ui.inputText.Length > 0)
                {
                    if (double.TryParse(ui.inputText, out double money))
                    {
                        gw.Money = money;
                        PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw));
                    }
                    else PanelManager.Notification(player, "Erreur", "Vous ne devez utiliser que des chiffres (négatif autorisés)", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez renseigner une valeur", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void AreaList(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Tab).SetTitle($"MyGiveway");

            if (gw.AreasId.Count == 0)
            {
                panel.AddTabLine($"Aucun terrain", ui => Debug.Log("delete"));
            }
            else
            {
                foreach (uint areadId in gw.AreasId)
                {
                    panel.AddTabLine($"Terrain n°{areadId}", ui =>
                    {
                        gw.AreasId.Remove(areadId);
                    });
                }
            }

            panel.AddButton("Ajouter", ui => PanelManager.NextPanel(player, ui, () => AddAreaId(player, gw)));
            panel.AddButton("Supprimer", ui =>
            {
                ui.SelectTab();
                PanelManager.NextPanel(player, ui, () => AreaList(player, gw));
            });
            panel.AddButton("Confirmer", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void AddAreaId(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"MyGiveway");

            panel.inputPlaceholder = "ID d'un terrain";

            panel.AddButton("Valider", ui =>
            {
                if (ui.inputText.Length > 0)
                {

                    if (uint.TryParse(ui.inputText, out uint areaId))
                    {
                        if (!gw.AreasId.Contains(areaId))
                        {
                            LifeArea area = Nova.a.GetAreaById(areaId);
                            if (area != null)
                            {
                                gw.AreasId.Add(areaId);
                                PanelManager.NextPanel(player, ui, () => AreaList(player, gw));
                            }
                            else PanelManager.Notification(player, "Erreur", "Aucun terrain ne semble correspondre à votre identifiant.", NotificationManager.Type.Error);
                        }
                        else PanelManager.Notification(player, "Erreur", "Vous avez déjà ajouté ce terrain.", NotificationManager.Type.Error);
                    }
                    else PanelManager.Notification(player, "Erreur", "Vous ne devez utiliser que des chiffres (négatif autorisés)", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez renseigner une valeur", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, () => AreaList(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void VehiclesList(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.TabPrice).SetTitle($"MyGiveway");

            if (gw.VehiclesId.Count == 0)
            {
                panel.AddTabLine($"Aucun véhicules", ui => Debug.Log("delete"));
            }
            else
            {
                foreach (KeyValuePair<int, int> keyValue in gw.VehiclesId)
                {
                    Vehicle vehicle = Nova.v.vehicleModels[keyValue.Key];
                    panel.AddTabLine($"{vehicle.vehicleName}", $"quantité: {keyValue.Value}", Utils.getVehicleIconId(keyValue.Key), (ui) =>
                    {
                        gw.VehiclesId.Remove(keyValue.Key);
                    });
                }
            }

            panel.AddButton("Ajouter", ui => PanelManager.NextPanel(player, ui, () => AddVehicleId(player, gw)));
            panel.AddButton("Supprimer", ui =>
            {
                ui.SelectTab();
                PanelManager.NextPanel(player, ui, () => VehiclesList(player, gw));
            });
            panel.AddButton("Confirmer", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void AddVehicleId(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"MyGiveway");

            panel.inputPlaceholder = "ID véhicule [espace] Quantité";

            panel.AddButton("Valider", ui =>
            {
                string[] inputText = ui.inputText.Split(' ');
                if (inputText.Length == 2)
                {
                    if (int.TryParse(inputText[1], out int quantity) && quantity > 0)
                    {
                        if (int.TryParse(inputText[0], out int vehicleId) && vehicleId >= 0)
                        {
                            if (!gw.VehiclesId.ContainsKey(vehicleId))
                            {
                                Vehicle vehicle = Nova.v.vehicleModels[vehicleId];
                                if (vehicle != null)
                                {
                                    if (!Nova.v.vehicleModels[vehicleId].isDeprecated)
                                    {
                                        gw.VehiclesId.Add(vehicleId, quantity);
                                        PanelManager.NextPanel(player, ui, () => VehiclesList(player, gw));
                                    }
                                    else PanelManager.Notification(player, "Erreur", $"Vous ne pouvez pas importer un véhicule qui est déprécié.", NotificationManager.Type.Error);
                                }
                                else PanelManager.Notification(player, "Erreur", $"Nous n'avons aucun véhicule correspondant à l'ID: {vehicleId}", NotificationManager.Type.Error);
                            }
                            else PanelManager.Notification(player, "Erreur", "Vous avez déjà ajouté ce véhicule.", NotificationManager.Type.Error);
                        }
                        else PanelManager.Notification(player, "Erreur", "Format de la valeur \"ID Véhicule\" invalide", NotificationManager.Type.Error);
                    }
                    else PanelManager.Notification(player, "Erreur", "Format de la valeur \"quantité\" invalide", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez respecter le format.\nID VEHICULE [ESPACE] QUANTITE", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, () => VehiclesList(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void ItemsList(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.TabPrice).SetTitle($"MyGiveway");

            if (gw.ItemsId.Count == 0)
            {
                panel.AddTabLine($"Aucun objets", ui => Debug.Log("delete"));
            }
            else
            {
                foreach (KeyValuePair<int, int> keyValue in gw.ItemsId)
                {
                    Item item = Nova.man.item.GetItem(keyValue.Key);
                    panel.AddTabLine($"{item.itemName}", $"quantité: {keyValue.Value}", Utils.getItemIconId(keyValue.Key), (ui) =>
                    {
                        gw.ItemsId.Remove(keyValue.Key);
                    });
                }
            }

            panel.AddButton("Ajouter", ui => PanelManager.NextPanel(player, ui, () => AddItemId(player, gw)));
            panel.AddButton("Supprimer", ui =>
            {
                ui.SelectTab();
                PanelManager.NextPanel(player, ui, () => ItemsList(player, gw));
            });
            panel.AddButton("Confirmer", ui => PanelManager.NextPanel(player, ui, () => SetupGiveway(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
        public static void AddItemId(Player player, Giveway gw)
        {
            UIPanel panel = new UIPanel("MyGiveway", UIPanel.PanelType.Input).SetTitle($"MyGiveway");

            panel.inputPlaceholder = "ID Objet [espace] Quantité";

            panel.AddButton("Valider", ui =>
            {
                string[] inputText = ui.inputText.Split(' ');
                if (inputText.Length == 2)
                {
                    if (int.TryParse(inputText[1], out int quantity) && quantity > 0)
                    {
                        if (int.TryParse(inputText[0], out int itemId) && itemId >= 0)
                        {
                            if (!gw.ItemsId.ContainsKey(itemId))
                            {
                                Item item = Nova.man.item.GetItem(itemId);
                                if (item != null)
                                {
                                    gw.ItemsId.Add(itemId, quantity);
                                    PanelManager.NextPanel(player, ui, () => ItemsList(player, gw));
                                }
                                else PanelManager.Notification(player, "Erreur", $"Nous n'avons aucun objet correspondant à l'ID: {itemId}", NotificationManager.Type.Error);
                            }
                            else PanelManager.Notification(player, "Erreur", "Vous avez déjà ajouté cette objet.", NotificationManager.Type.Error);
                        }
                        else PanelManager.Notification(player, "Erreur", "Format de la valeur \"ID Objet\" invalide", NotificationManager.Type.Error);
                    }
                    else PanelManager.Notification(player, "Erreur", "Format de la valeur \"quantité\" invalide", NotificationManager.Type.Error);
                }
                else PanelManager.Notification(player, "Erreur", "Vous devez respecter le format.\nID Objet [ESPACE] QUANTITE", NotificationManager.Type.Error);
            });
            panel.AddButton("Retour", ui => PanelManager.NextPanel(player, ui, () => ItemsList(player, gw)));
            panel.AddButton("Fermer", ui => PanelManager.Quit(ui, player));

            player.ShowPanelUI(panel);
        }
    }
}
