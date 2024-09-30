﻿namespace me.cqp.luohuaming.iKun.PublicInfos.Items
{
    public class TransmogrifyPill : Models.Items
    {
        public TransmogrifyPill(int count = 1)
        {
            ID = Enums.Items.TransmogrifyPill;
            Name = "幻化丸";
            Description = "收益风险并存，能大幅强化鲲，但是一旦失败将灰飞烟灭";
            Stackable = true;
            Count = count;
        }
    }
}