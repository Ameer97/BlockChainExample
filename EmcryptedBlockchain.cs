using System;
using System.Collections.Generic;
using System.Linq;

namespace BlockChainExample
{
    public class EmcryptedBlockchain
    {
        public IList<Block> Chain { set; get; }

        public EmcryptedBlockchain()
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
                    item.Index + 1, item.TimeStamp, item.Hash, item.PreviousHash, item.Data);
            }
        }

        public void VerifyWithPreviousHash(int index)
        {
            var Block = Chain[index];
            var result = Block.Index + " ";

            var IsVerifyedHash = Block.VerifyHash(Block.Data);

            result += IsVerifyedHash;

            if (!IsVerifyedHash)
            {
                result += " " + false;
                Console.WriteLine(result);
                return;
            }

            if (index + 1 >= Chain.Count) return;

            var NextBlock = Chain[index + 1];
            var IsVerifyedWithNextHash = Block.Hash == NextBlock.PreviousHash;

            result += " " + IsVerifyedWithNextHash;
            Console.WriteLine(result);
        }

        public void VerifyChain()
        {
            Console.WriteLine("# Data Hash");
            Console.WriteLine();
            for (int i = 0; i < Chain.Count; i++)
            {
                VerifyWithPreviousHash(i);
            }
        }



        public static void Test()
        {
            EmcryptedBlockchain phillyCoin = new EmcryptedBlockchain();
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Henry,receiver:MaHesh,amount:10}"));
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Ameer,receiver:Mustafa,amount:15}"));
            phillyCoin.AddBlock(new Block(DateTime.Now, null, "{sender:Mahesh,receiver:Henry,amount:5}"));
            //Console.WriteLine(JsonConvert.SerializeObject(phillyCoin));
            phillyCoin.DisplayAll();

            phillyCoin.VerifyChain();



            phillyCoin.Chain.First().Data = "Change it";
            phillyCoin.Chain.Skip(1).First().PreviousHash = " $2a$12$dzX/18Rv2YM6qjuBn8D0vu7H19urfmMxz/wWLtWo2.UwP4dhaw53W";
            Console.WriteLine();
            Console.WriteLine();
            phillyCoin.DisplayAll();
            phillyCoin.VerifyChain();
        }




    }
}
