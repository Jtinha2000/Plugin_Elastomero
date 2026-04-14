using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCombat.Models
{
    public class ElastomerConfig
    {
        public ushort ItemID { get; set; }
        public uint Duration { get; set; }
        public float SpeedMultiplier { get; set; }
        public float JumpMultiplier { get; set; }
        public float GravityMultiplier { get; set; }
        public bool CancelDamage { get; set; }
        public ElastomerConfig()
        {
            
        }
        public ElastomerConfig(ushort itemID, uint duration, float speedMultiplier, float jumpMultiplier, float gravityMultiplier, bool cancelDamage)
        {
            ItemID = itemID;
            Duration = duration;
            SpeedMultiplier = speedMultiplier;
            JumpMultiplier = jumpMultiplier;
            GravityMultiplier = gravityMultiplier;
            CancelDamage = cancelDamage;
        }
    }
}
