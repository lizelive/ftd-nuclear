using BrilliantSkies.Core.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CultOfClang.NuclearReactor
{
    public class NuclearCoolingTower : SteamTank
    {
        public float SteamPerSecond => 1000 * this.item.SizeInfo.ArrayPositionsUsed;
        public float HeatChange => -NuclearAllInOne.HeatPerVolume * this.item.SizeInfo.ArrayPositionsUsed;
        public float SteamToMats = 1 / SteamConstants.SteamPerMaterial;

        public override void StateChanged(IBlockStateChange change)
        {
            base.StateChanged(change);
            if (change.IsAvailableToConstruct)
            {
                this.MainConstruct.HotObjectsRestricted.AddASimpleSourceOfBodyHeat(HeatChange);
                this.MainConstruct.SchedulerRestricted.RegisterForFixedUpdate(Update);

            }
            else
            {
                if (!change.IsLostToConstructOrConstructLost)
                    return;
                this.MainConstruct.HotObjectsRestricted.RemoveASimpleSourceofBodyHeat(HeatChange);
                this.MainConstruct.SchedulerRestricted.UnregisterForFixedUpdate(Update);

            }
        }
        private void Update(ISectorTimeStep obj)
        {
            var steam = obj.DeltaTime * SteamPerSecond;
            steam = this.StorageModule.TakeSteam(steam);
            this.MainConstruct.GetForce().Material.Give(SteamToMats * steam);
            this.Stats.BoilerSteamCreated.Add(steam);
        }
    }
}
