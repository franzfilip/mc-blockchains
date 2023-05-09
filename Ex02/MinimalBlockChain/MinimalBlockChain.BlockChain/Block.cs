using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MinimalBlockChain.BlockChain {
    public class Block {
        public int Index { get; set; }
        public long Timestamp { get; set; }
        public List<Transaction> Transactions { get; set; }
        public string Proof { get; set; } //what exactly should the prove be ?
        public string PreviousHash { get; set; }
    }
}
