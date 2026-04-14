using SDG.Unturned;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UCombat.Models
{
    public class SurrenderModel
    {
        public Player Rendido { get; set; }
        public Coroutine Controller { get; set; }
        public int Duration { get; set; }
        public SurrenderModel()
        {

        }
        public SurrenderModel(Player rendido, int duration)
        {
            Rendido = rendido;
            Duration = duration;

            if (Rendido.movement.getVehicle() != null && !Rendido.movement.forceRemoveFromVehicle())
                return;

            Controller = Core.Instance.StartCoroutine(Countdown());
            Core.Instance.Surrenders.Add(this);
            Rendido.animator.sendGesture(EPlayerGesture.SURRENDER_START, true);
        }
        public void ClearSurrender()
        {
            if (Controller != null)
                Core.Instance.StopCoroutine(Controller);

            Core.Instance.Surrenders.Remove(this);
            if (Rendido.animator.gesture != EPlayerGesture.ARREST_START)
                Rendido.animator.sendGesture(EPlayerGesture.NONE, true);
        }
        public IEnumerator Countdown()
        {
            while (Duration > 0)
            {
                yield return new WaitForSeconds(1);
                Duration--;
            }

            Controller = null;
            ClearSurrender();
        }
    }
}
