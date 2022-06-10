using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using static BlockChainExample.Block;

namespace BlockChainExample
{
    class Program2
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");


            Blockchain phillyCoin = new Blockchain();
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Henry,receiver:MaHesh,amount:10}"));
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:MaHesh,receiver:Henry,amount:5}"));
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Mahesh,receiver:Henry,amount:5}"));
            //Console.WriteLine(JsonConvert.SerializeObject(phillyCoin));
            phillyCoin.DisplayAll();

            phillyCoin.VerifyChain();
            
        }
    }








    public class Block
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
            bool verified = BCrypt.Net.BCrypt.Verify(InputHash(NewData), Hash);
            if (verified) return true;
            return false;
        }






        public class Blockchain
        {
            public IList<Block> Chain { set; get; }

            public Blockchain()
            {
                InitializeChain();
                AddGenesisBlock();
            }


            public void InitializeChain()
            {
                Chain = new List<Block>();
            }

            public Block CreateGenesisBlock()
            {
                return new Block(DateTime.Now, null, "{}");
            }

            public void AddGenesisBlock()
            {
                Chain.Add(CreateGenesisBlock());
            }

            public Block GetLatestBlock()
            {
                return Chain[Chain.Count - 1];
            }

            public void AddBlock(Block block)
            {
                Block latestBlock = GetLatestBlock();
                block.Index = latestBlock.Index + 1;
                block.PreviousHash = latestBlock.Hash;
                block.Hash = block.CalculateHash();
                Chain.Add(block);
            }

            public void DisplayAll()
            {
                foreach (var item in Chain)
                {
                    Console.WriteLine("Index: {0}" +
                        "\n" +
                        "TimeStamp: {1}" +
                        "\n" +
                        "Current Hash: {2}" +
                        "\n" +
                        "Previous Hash: {3}" +
                        "\n" +
                        "Data: {4}" +
                        "\n\n", 
                        item.Index, item.TimeStamp, item.Hash, item.PreviousHash, item.Data);
                }
            }

            public void VerifyChain()
            {
                Console.WriteLine();
                for (int i = 0; i < Chain.Count; i++)
                {
                    Console.WriteLine(Chain[i].VerifyHash(Chain[i].Data));
                }
            }


            


        }
    }
}
