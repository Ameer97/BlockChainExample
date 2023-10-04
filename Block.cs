using System;

namespace BlockChainExample
{
    public partial class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }

        public Block(DateTime timeStamp, string previousHash, string data)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Hash = CalculateHash();
        }

        private string InputHash(string newData = null)
        {
            var AData = (string.IsNullOrEmpty(newData)) ? Data : newData;

            var input = $"{TimeStamp}-{PreviousHash ?? ""}-{AData}";

            return input;
            //return Encoding.ASCII.GetBytes();
        }

        public string CalculateHash()
        {
            var outputBytes = BCrypt.Net.BCrypt.HashPassword(InputHash(), 12);

            //return Convert.ToBase64String(outputBytes);
            return outputBytes;
        }

        public bool VerifyHash(string NewData)
        {
            try
            {
                bool verified = BCrypt.Net.BCrypt.Verify(InputHash(NewData), Hash);
                if (verified) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
