using System;
using System.Collections.Generic;
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
        public List<int> VehiclesId { get; set; } = new List<int>();
        public List<int> AreasId  { get; set; } = new List<int>();
        public List<int> ItemsId { get; set; } = new List<int>();

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
    }
}
