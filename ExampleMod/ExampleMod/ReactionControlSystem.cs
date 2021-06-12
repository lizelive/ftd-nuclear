using BrilliantSkies.Core.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CultOfClang.NuclearReactor
{
    public class ReactionControlSystem: Block
    {

        public override void StateChanged(IBlockStateChange change)
        {
            base.StateChanged(change);
            if (change.IsAvailableToConstruct)
            {
                this.MainConstruct.SchedulerRestricted.RegisterForFixedUpdate(Update);

            }
            else
            {
                if (!change.IsLostToConstructOrConstructLost)
                    return;
                this.MainConstruct.SchedulerRestricted.UnregisterForFixedUpdate(Update);

            }
        }
        private void Update(ISectorTimeStep obj)
        {
            var rot = this.GetConstructableOrSubConstructable().MainThreadRotation;
            var angles = rot.eulerAngles;
            angles.x = 0;
            angles.z = 0;
            rot.eulerAngles = angles;
            this.GetConstructableOrSubConstructable().MainThreadRotation = rot;
        }
    }
}
