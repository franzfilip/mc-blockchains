namespace MinimalBlockChain.BlockChain {
    public class BlockChain {
        private List<Block> chain;
        private List<Transaction> currentTransactions;

        public BlockChain() {
            chain = new List<Block>();
            currentTransactions = new List<Transaction>();

            NewBlock("100", "1");
        }

        public int NewTransaction(string sender, string recipient, double amount) {
            currentTransactions.Add(new Transaction { Sender = sender, Recipient = recipient, Amount = amount });
            return chain.Count;
        }

        public Block NewBlock(string proof, string previousHash) {
            Block block = new Block {
                Index = chain.Count,
                Timestamp = GetCurrentUnixTimestamp(),
                Proof = proof,
                PreviousHash = previousHash,
                Transactions = currentTransactions
            };

            currentTransactions = new List<Transaction>();
            chain.Add(block);

            return block;
        }

        private long GetCurrentUnixTimestamp() {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epoch;
            return (long)timeSpan.TotalSeconds;
        }
    }
}