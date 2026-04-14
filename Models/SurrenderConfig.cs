using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UCombat.Models
{
    public class SurrenderConfig
    {
        public ushort WeaponID { get; set; }
        public bool CancelDamage { get; set; }
        public int ForceSurrenderDuration { get; set; }
        public SurrenderConfig()
        {
            
        }
        public SurrenderConfig(ushort weaponID, bool cancelDamage, int forceSurrenderDuration)
        {
            WeaponID = weaponID;
            CancelDamage = cancelDamage;
            ForceSurrenderDuration = forceSurrenderDuration;
        }
    }
}
