﻿using me.cqp.luohuaming.iKun.PublicInfos.PetAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.luohuaming.iKun.PublicInfos.Models.Results
{
    public class TransmogrifyResult
    {
        public bool Success { get; set; } = true;

        public double Decrement { get; set; }

        public double CurrentWeight { get; set; }

        public bool Dead { get; set; }

        public IPetAttribute CurrentAttributeA { get; set; }

        public IPetAttribute CurrentAttributeB { get; set; }

        public IPetAttribute OriginalAttributeA { get; set; }

        public IPetAttribute OriginalAttributeB { get; set; }

        public override string ToString()
        {
            return $"执行成功={Success}，体重减量={Decrement}，当前体重={CurrentWeight}，是否死亡={Dead}，" +
                $"新词条：{CurrentAttributeA.Name}[{CurrentAttributeA.ID}] {CurrentAttributeB.Name}[{CurrentAttributeB.ID}]，" +
                $"原始词条：{OriginalAttributeA.Name}[{OriginalAttributeA.ID}] {OriginalAttributeB.Name}[{OriginalAttributeB.ID}]";
        }
    }
}
