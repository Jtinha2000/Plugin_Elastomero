using Newtonsoft.Json;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Extensions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UCombat.Models
{
    public class DebilitiesModel
    {
        public Coroutine CountdownController { get; set; }
        public UnturnedPlayer Player { get; set; }

        public float SpeedModifier { get; set; }
        public float JumpModifier { get; set; }
        public float GravityModifier { get; set; }
        public uint Duration { get; set; }
        public DebilitiesModel()
        {
            SpeedModifier = 1;
            JumpModifier = 1;
            GravityModifier = 1;
            Duration = 0;
        }
        public DebilitiesModel(UnturnedPlayer player, float speedModifier, float jumpModifier, float gravityModifier, uint duration) : this()
        {
            Player = player;
            ImplementDebilities(speedModifier, jumpModifier, gravityModifier, duration);
        }

        public void ImplementDebilities(float speedModifier, float jumpModifier, float gravityModifier, uint duration)
        {
            SpeedModifier *= speedModifier;
            JumpModifier *= jumpModifier;
            GravityModifier *= gravityModifier;

            Player.Player.movement.sendPluginSpeedMultiplier(Player.Player.movement.pluginSpeedMultiplier * speedModifier);
            Player.Player.movement.sendPluginJumpMultiplier(Player.Player.movement.pluginJumpMultiplier * jumpModifier);
            Player.Player.movement.sendPluginGravityMultiplier(Player.Player.movement.pluginGravityMultiplier * gravityModifier);
            Duration += duration;

            if (CountdownController == null)
                CountdownController = Core.Instance.StartCoroutine(DebilitieCountdown());
        }
        public void ClearDebilities()
        {
            if (CountdownController != null)
                Core.Instance.StopCoroutine(CountdownController);

            Player.Player.movement.sendPluginSpeedMultiplier(Player.Player.movement.pluginSpeedMultiplier / SpeedModifier);
            Player.Player.movement.sendPluginJumpMultiplier(Player.Player.movement.pluginJumpMultiplier / JumpModifier);
            Player.Player.movement.sendPluginGravityMultiplier(Player.Player.movement.pluginGravityMultiplier / GravityModifier);

            SpeedModifier = 1;
            JumpModifier = 1;
            GravityModifier = 1;
            Duration = 0;

            Core.Instance.Debilities.Remove(this);
        }
        public IEnumerator DebilitieCountdown()
        {
            while (Duration > 0)
            {
                yield return new WaitForSeconds(1);
                Duration -= 1;
            }

            CountdownController = null;
            ClearDebilities();
        }
    }
}
