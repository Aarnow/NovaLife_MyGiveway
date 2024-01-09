using Life.VehicleSystem;
using Life;
using System;
using System.Globalization;

namespace MyGiveway
{
    abstract class Utils
    {
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
    }
}
