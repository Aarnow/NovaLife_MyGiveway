using Life.VehicleSystem;
using Life;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MyGiveway
{
    public class Giveway
    {
        public string Name { set; get; }
        public bool IsActive { get; set; }
        public bool IsSingleUse { get; set; }
        public string Code { get; set; }    
        public DateTime ExpirationDate { get; set; }
        public List<int> PlayersId { get; set; } = new List<int>();

        public double Money { get; set; }
        public Dictionary<int, int> VehiclesId { get; set; } = new Dictionary<int, int>();
        public List<uint> AreasId  { get; set; } = new List<uint>();
        public Dictionary<int, int> ItemsId { get; set; } = new Dictionary<int, int>();

        public Giveway()
        {
            Name = "Default";
            IsActive = false;
            IsSingleUse = false;
            Code = GenerateCode();
            ExpirationDate = new DateTime(2050, 01, 01, 00, 00, 00);
            Money = 0;
        }

        private string GenerateCode()
        {
            const int codeLength = 8;
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            StringBuilder sb = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < codeLength; i++)
            {
                int index = random.Next(0, allowedChars.Length);
                sb.Append(allowedChars[index]);
            }

            return sb.ToString();
        }

        public bool TryParseDateTime(string input, out DateTime result)
        {
            string[] format = { "dd/MM/yyyy - HH'h'mm", "dd/MM/yyyy - HH'h'mm" };
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        public int getVehicleIconId(int modelId)
        {
            Vehicle vehicle = Nova.v.vehicleModels[modelId];
            int iconId = Array.IndexOf(LifeManager.instance.icons, vehicle.icon);
            return iconId >= 0 ? iconId : Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(1112).icon);
        }

        public int getItemIconId(int itemId)
        {
            int iconId = Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(itemId).icon);
            return iconId >= 0 ? iconId : Array.IndexOf(LifeManager.instance.icons, LifeManager.instance.item.GetItem(1112).icon);
        }
    }
}
