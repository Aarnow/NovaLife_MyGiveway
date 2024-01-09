using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MyGiveway
{
    public class Giveway
    {
        public string Slug { get; set; }
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

        public void Save()
        {
            int number = 0;
            string filePath;

            do
            {
                Slug = $"{Name}_{number}";
                filePath = Path.Combine(Main.directoryPath, $"{Slug}.json");
                number++;
            } while (File.Exists(filePath));

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
