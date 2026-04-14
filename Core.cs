using Newtonsoft.Json;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Extensions;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UCombat.Models;
using UnityEngine;

namespace UCombat
{
    public class Core : RocketPlugin<Configuration>
    {
        public static Core Instance { get; set; }
        public List<DebilitiesModel> Debilities { get; set; }
        public List<SurrenderModel> Surrenders { get; set; }
        protected override void Load()
        {
            Instance = this;
            Debilities = new List<DebilitiesModel>();
            Surrenders = new List<SurrenderModel>();
            base.Load();
            VehicleManager.onEnterVehicleRequested += VehicleManager_onEnterVehicleRequested;
            UnturnedPlayerEvents.OnPlayerUpdateGesture += UnturnedPlayerEvents_OnPlayerUpdateGesture;
            DamageTool.damagePlayerRequested += DamageTool_damagePlayerRequested;
            U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
            UnturnedPlayerEvents.OnPlayerDeath += UnturnedPlayerEvents_OnPlayerDeath;
        }
        protected override void Unload()
        {
            base.Unload();
            while (Surrenders.Count > 0)
                Surrenders.First().ClearSurrender();
            while (Debilities.Count > 0)
                Debilities.First().ClearDebilities();
            VehicleManager.onEnterVehicleRequested -= VehicleManager_onEnterVehicleRequested;
            UnturnedPlayerEvents.OnPlayerUpdateGesture -= UnturnedPlayerEvents_OnPlayerUpdateGesture;
            U.Events.OnPlayerDisconnected -= Events_OnPlayerDisconnected;
            UnturnedPlayerEvents.OnPlayerDeath -= UnturnedPlayerEvents_OnPlayerDeath;
            DamageTool.damagePlayerRequested -= DamageTool_damagePlayerRequested;
        }

        public void VehicleManager_onEnterVehicleRequested(Player player, InteractableVehicle vehicle, ref bool shouldAllow)
        {
            shouldAllow = shouldAllow && !Surrenders.Any(X => X.Rendido == player);
        }
        public void UnturnedPlayerEvents_OnPlayerUpdateGesture(Rocket.Unturned.Player.UnturnedPlayer player, UnturnedPlayerEvents.PlayerGesture gesture)
        {
            SurrenderModel Surrender = Surrenders.FirstOrDefault(X => X.Rendido == player.Player);
            if (Surrender == null || player.Player.animator.gesture == EPlayerGesture.SURRENDER_START)
                return;

            if (player.Player.animator.gesture == EPlayerGesture.ARREST_START)
                Surrender.ClearSurrender();
            else
                player.Player.animator.sendGesture(EPlayerGesture.SURRENDER_START, true);
        }
        public void DamageTool_damagePlayerRequested(ref DamagePlayerParameters parameters, ref bool shouldAllow)
        {
            Player Killer = PlayerTool.getPlayer(parameters.killer);
            if (Killer == null)
                return;

            ElastomerConfig Config = Configuration.Instance.ElastomerWeapons.FirstOrDefault(X => X.ItemID == Killer.equipment.itemID);
            SurrenderConfig SurrenderConfig = Configuration.Instance.SurrenderWeapons.FirstOrDefault(X => X.WeaponID == Killer.equipment.itemID);
            Player Caller = parameters.player;
            if (Config != null)
            {
                DebilitiesModel Debilitie = Debilities.FirstOrDefault(X => X.Player.Player == Caller);
                if (Debilitie == null)
                    Debilities.Add(new DebilitiesModel(Caller.channel.owner.ToUnturnedPlayer(), Config.SpeedMultiplier, Config.JumpMultiplier, Config.GravityMultiplier, Config.Duration));
                else
                    Debilitie.Duration = Config.Duration;

                if (Config.CancelDamage)
                {
                    parameters.damage = 0;
                    if (Configuration.Instance.AlsoCancelEffect)
                        shouldAllow = false;
                }
            }
            if (SurrenderConfig != null)
            {
                SurrenderModel Surrender = Surrenders.FirstOrDefault(X => X.Rendido == Caller);
                if (Surrender != null)
                    Surrender.Duration = SurrenderConfig.ForceSurrenderDuration;
                else
                    new SurrenderModel(Caller, SurrenderConfig.ForceSurrenderDuration);

                if (SurrenderConfig.CancelDamage)
                {
                    parameters.damage = 0;
                    if (Configuration.Instance.AlsoCancelEffect)
                        shouldAllow = false;
                }
            }
        }
        private void UnturnedPlayerEvents_OnPlayerDeath(Rocket.Unturned.Player.UnturnedPlayer player, EDeathCause cause, ELimb limb, Steamworks.CSteamID murderer)
            => Events_OnPlayerDisconnected(player);
        private void Events_OnPlayerDisconnected(Rocket.Unturned.Player.UnturnedPlayer player)
        {
            DebilitiesModel Debilitie = Debilities.FirstOrDefault(X => X.Player.Player == player.Player);
            if (Debilitie != null)
                Debilitie.ClearDebilities();
            SurrenderModel Surrender = Surrenders.FirstOrDefault(X => X.Rendido == player.Player);
            if (Surrender != null)
                Surrender.ClearSurrender();
        }
    }
}
