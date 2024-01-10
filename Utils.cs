using Life.VehicleSystem;
using Life;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using Life.Network;
using System.Linq;

namespace MyGiveway
{
    abstract class Utils
    {
        public static int codeLength = 8;
        public static string allowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public static bool TryParseDateTime(string input, out DateTime result)
        {
            string[] format = { "dd/MM/yyyy - HH'h'mm", "dd/MM/yyyy - HH'h'mm" };
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        public static int getVehicleIconId(int modelId)
        {
            Vehicle vehicle = Nova.v.vehicleModels[modelId];
            int iconId = Array.IndexOf(LifeManager.instance.icons, vehicle.icon);
            return iconId >= 0 ? iconId : Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(1112).icon);
        }

        public static int getItemIconId(int itemId)
        {
            int iconId = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(itemId).icon);
            return iconId >= 0 ? iconId : Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(1112).icon);
        }

        public static string itemWithData(int itemId)
        {
            int[] foodItemIDs = { 1, 2 };
            int[] mechanicItemIDs = { 3, 4, 5 };
            int[] clothItemIDs = { 85, 102, 103, 104, 105, 106, 107, 125, 126, 153, 154, 1073, 1074, 1127, 1128, 1129, 1130 };
            int[] weaponItemIDs = { 6, 1622, 1629 };
            if (foodItemIDs.Contains(itemId)) return "{\"cookedPercentage\":0,\"expireTimestamp\":0}";
            else if (mechanicItemIDs.Contains(itemId)) return "{\"statePercentage\":100.0}";
            else if (clothItemIDs.Contains(itemId)) return "";
            else if (weaponItemIDs.Contains(itemId)) return "{\"currentAmmo\":0}";
            else return null;
        }

        public static string GenerateCode()
        {
            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < codeLength; i++)
            {
                int index = random.Next(0, allowedCharacters.Length);
                sb.Append(allowedCharacters[index]);
            }

            return sb.ToString();
        }

        public static bool IsExistGiveway(string code, out Giveway giveway)
        {
            giveway = Main.GivewayList.Where(gw => gw.Code == code).FirstOrDefault();
            return giveway != null;
        }

        public static bool IsExpiredGiveway(Giveway giveway)
        {
            return (giveway.ExpirationDate < DateTime.Now || giveway.IsSingleUse && giveway.PlayersId.Count != 0) ? true : false;
        }

        public static bool IsConsumedGiveway(Giveway giveway, Player player)
        {
            return giveway.PlayersId.Contains(player.account.id);
        }
    }
}
