using MinimalBlockChain.BlockChain.DTOs;
using MinimalBlockChain.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace MinimalBlockChain.BlockChain
{
    public class BlockChain
    {
        public string NodeIdentifier { get; }
        private List<Block> chain;
        private HashSet<string> nodes;
        public List<Block> Chain
        {
            get
            {
                return chain;
            }
        }
        private List<Transaction> currentTransactions;

        public BlockChain()
        {
            nodes = new();
            chain = new List<Block>();
            currentTransactions = new List<Transaction>();
            NodeIdentifier = Guid.NewGuid().ToString().Replace("-", "");
            NewBlock(100, "1");
        }

        public int NewTransaction(Transaction transaction)
        {
            currentTransactions.Add(transaction);
            return chain.Count;
        }

        public Block NewBlock(int proof, string previousHash)
        {
            Block block = new Block
            {
                Index = chain.Count,
                Timestamp = GetCurrentUnixTimestamp(),
                Proof = proof,
                PreviousHash = previousHash.Equals("1") ? previousHash : Hash(chain.Last()),
                Transactions = currentTransactions
            };

            currentTransactions = new List<Transaction>();
            chain.Add(block);

            return block;
        }

        public static string Hash(Block block)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new AlphabeticalContractResolver(),
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(block, settings);
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(json));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public int ProofOfWork(int lastProof)
        {
            int proof = 0;
            while (!ValidProof(lastProof, proof))
            {
                proof++;
            }
            return proof;
        }

        public static bool ValidProof(int lastProof, int proof)
        {
            string guess = $"{lastProof}{proof}";
            byte[] guessBytes = Encoding.UTF8.GetBytes(guess);
            byte[] hashBytes = SHA256.Create().ComputeHash(guessBytes);
            string guessHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            bool correctGuess = guessHash.Substring(0, 4).Equals("0000");
            if (correctGuess)
            {
                Console.WriteLine(guessHash);
            }
            return correctGuess;
        }

        public Block LastBlock()
        {
            return chain.Last();
        }

        private long GetCurrentUnixTimestamp()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epoch;
            return (long)timeSpan.TotalSeconds;
        }

        public static bool IsChainValid(List<Block> chain)
        {
            for (int i = 1; i < chain.Count; i++)
            {
                Block currentBlock = chain[i];
                Block previousBlock = chain[i - 1];

                // Check that the hash of the previous block matches the PreviousHash field of the current block
                if (currentBlock.PreviousHash != Hash(previousBlock))
                {
                    return false;
                }

                // Check that the proof of work for the current block is valid
                if (!ValidProof(previousBlock.Proof, currentBlock.Proof))
                {
                    return false;
                }
            }

            // All checks passed, so the chain is valid
            return true;
        }

        public async Task<bool> ResolveConflicts()
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();

            bool otherChainIsNewer = false;
            foreach (string node in nodes)
            {
                var chaindto = await GetChainFromNode(client, node + "/chain");
                if (chaindto != null)
                {
                    if(chaindto.Length > chain.Count && IsChainValid(chaindto.Chain))
                    {
                        chain = chaindto.Chain;
                        otherChainIsNewer = true;
                    }
                }
            }

            return otherChainIsNewer;
        }

        public List<string> RegisterNodes(List<string> nodesToBeAdded)
        {
            nodesToBeAdded.ForEach(node =>
            {
                nodes.Add(node);
            });

            return nodes.ToList();
        }

        private async Task<ChainDTO?> GetChainFromNode(HttpClient client, string url)
        {
            return JsonConvert.DeserializeObject<ChainDTO>(await client.GetStringAsync(url));
        }

        //@Deprecated - used hard coded urls in environment variables instead of web request
        private List<string> GetNodes()
        {
            List<string> nodes = new List<string>();
            string? url1 = Environment.GetEnvironmentVariable("url1");
            string? url2 = Environment.GetEnvironmentVariable("url2");
            if (!string.IsNullOrEmpty(url1))
            {
                nodes.Add(url1);
            }
            if (!string.IsNullOrEmpty(url2))
            {
                nodes.Add(url2);
            }

            if (nodes.Count == 0)
            {
                nodes.Add("http://localhost:5001");
            }

            return nodes;
        }
    }
}