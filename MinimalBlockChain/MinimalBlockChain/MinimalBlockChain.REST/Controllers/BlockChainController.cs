using Microsoft.AspNetCore.Mvc;
using MinimalBlockChain.BlockChain;
using MinimalBlockChain.BlockChain.DTOs;
using System.Text.Json;

namespace MinimalBlockChain.REST.Controllers
{
    [ApiController]
    public class BlockChainController : Controller
    {
        private readonly BlockChain.BlockChain blockChain;

        public BlockChainController(BlockChain.BlockChain blockChain)
        {
            this.blockChain = blockChain;
        }

        [HttpPost("transactions/new")]
        public IActionResult AddTransaction([FromBody]Transaction transaction)
        {
            if(transaction == null)
            {
                return BadRequest("Missing values");
            }

            int index = blockChain.NewTransaction(transaction);

            var message = $"Transaction will be added to Block {index}";
            return new JsonResult(new { message });
        }

        [HttpGet("chain")]
        public ActionResult<List<Block>> GetBlockChain()
        {
            ChainDTO chaindto = new ChainDTO
            {
                Chain = blockChain.Chain,
                Length = blockChain.Chain.Count
            };
            return Ok(JsonSerializer.Serialize(chaindto));
        }

        [HttpGet("mine")]
        public IActionResult Mine()
        {
            var lastBlock = blockChain.LastBlock();
            var lastProof = lastBlock.Proof;
            var proof = blockChain.ProofOfWork(lastProof);

            blockChain.NewTransaction(new Transaction { Sender = "0", Recipient = blockChain.NodeIdentifier, Amount = 1 });

            var previousHash = BlockChain.BlockChain.Hash(blockChain.LastBlock());
            var block = blockChain.NewBlock(proof, previousHash);

            var response = new
            {
                Message = "New Block Forged",
                Index = block.Index,
                Transactions = block.Transactions,
                Proof = block.Proof,
                Previous = block.PreviousHash
            };
            return Ok(JsonSerializer.Serialize(response));
        }

        [HttpGet("nodes/resolve")]
        public async Task<IActionResult> NodesResolve()
        {
            bool wasReplaced = await blockChain.ResolveConflicts();
            if (wasReplaced)
            {
                var response = new
                {
                    Message = "Our chain was replaced",
                    NewChain = blockChain.Chain
                };

                return Ok(JsonSerializer.Serialize(response));
            }
            else
            {
                var response = new
                {
                    Message = "Chain is authoritative",
                    Chain = blockChain.Chain
                };

                return Ok(JsonSerializer.Serialize(response));
            }
        }

        [HttpPost("nodes/register")]
        public ActionResult RegisterNodes([FromBody]NodesDTO nodesdto)
        {
            if(nodesdto == null)
            {
                return BadRequest("Error: Invalid data");
            }

            List<string> nodes = blockChain.RegisterNodes(nodesdto.Nodes);

            var response = new
            {
                Message = "New nodes have been added",
                TotalNodes = nodes
            };


            return Created("", JsonSerializer.Serialize(response));
        }

        [HttpGet("chainhealth")]
        public ActionResult<bool> CheckIfChainIsHealthy()
        {
            return Ok(BlockChain.BlockChain.IsChainValid(blockChain.Chain));
        }
    }
}
