﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalBlockChain.BlockChain.DTOs
{
    public class ChainDTO
    {
        public List<Block> Chain { get; set; }
        public int Length { get; set; }
    }
}
