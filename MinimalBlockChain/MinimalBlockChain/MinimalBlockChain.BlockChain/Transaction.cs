using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalBlockChain.BlockChain {
    public class Transaction {
        public string Sender { get; set; }
        public string Recipient { get; set; }
        public double Amount { get; set; }
    }
}
