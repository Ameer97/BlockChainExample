using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            //Console.WriteLine(phillyCoin.GetLatestBlock());
            Console.WriteLine(JsonConvert.SerializeObject(phillyCoin));
            Console.WriteLine();

            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:MaHesh,receiver:Henry,amount:5}"));
            Console.WriteLine(JsonConvert.SerializeObject(phillyCoin));
            Console.WriteLine();
            //Console.WriteLine(phillyCoin.GetLatestBlock());
            //Console.WriteLine(JsonConvert.SerializeObject(phillyCoin));

            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Mahesh,receiver:Henry,amount:5}"));
            //Console.WriteLine(JsonConvert.SerializeObject(phillyCoin));
            //Console.WriteLine(phillyCoin.GetLatestBlock());
            Console.WriteLine(JsonConvert.SerializeObject(phillyCoin));
            Console.WriteLine();

            foreach (var item in phillyCoin.Chain)
            {
                Console.WriteLine(item);
                Console.WriteLine(item.Data);

            }
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

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
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
        }
    }
}
