﻿using BattleTech;
using MechEngineer.Features.Engines;
using MechEngineer.Features.Engines.Helper;
using MechEngineer.Features.MoveMultiplierStat;
using MechEngineer.Features.OverrideTonnage;
using UnityEngine;

namespace MechEngineer.Features.OverrideStatTooltips.Helper
{
    internal class MechDefMovementStatistics
    {
        internal Engine Engine { get; }

        internal float WalkMovementPoint { get; }
        internal float WalkSpeed { get; }
        internal float RunSpeed { get; }
        internal float JumpDistance { get; }
        internal int JumpJetCount { get; }

        internal MechDefMovementStatistics(MechDef mechDef)
        {
            this.mechDef = mechDef;
            
            Engine = mechDef.GetEngine();
            movement = Engine?.CoreDef.GetMovement(mechDef.Chassis.Tonnage);
            if (movement == null)
            {
                return;
            }
            
            WalkMovementPoint = movement.WalkMovementPoint;
            MoveMultiplier = GetMoveMultiplier();
            BaseWalkSpeed = GetWalkSpeed();
            BaseRunSpeed = GetRunSpeed();
            WalkSpeed = BaseWalkSpeed * MoveMultiplier;
            RunSpeed = BaseRunSpeed * MoveMultiplier;

            JumpCapacity = GetJumpCapacity();
            BaseJumpDistance = EngineMovement.ConvertMPToGameDistance(JumpCapacity);
            JumpDistanceMultiplier = GetJumpDistanceMultiplier();
            JumpDistance = BaseJumpDistance * JumpDistanceMultiplier;

            JumpJetCount = GetJumpJetCount();
        }
        
        private float MoveMultiplier { get; }
        private float BaseWalkSpeed { get; }
        private float BaseRunSpeed { get; }
        private float JumpDistanceMultiplier { get; }
        private float JumpCapacity { get; }
        private float BaseJumpDistance { get; }

        private readonly MechDef mechDef;
        private readonly EngineMovement movement;
        private readonly StatCollection statCollection = new StatCollection();

        internal float GetStatisticRating()
        {
            // only sprint, no walk and no jump
            return GetStatisticRating(RunSpeed);
        }

        internal static float GetStatisticRating(float runSpeed)
        {
            var constants = UnityGameInstance.BattleTechGame.MechStatisticsConstants;
            return (runSpeed - constants.MinSprintFactor) / constants.MaxSprintFactor;
        }

        private float GetMoveMultiplier()
        {
            var stat = statCollection.MoveMultiplier();
            stat.Create();
            return MechDefStatisticModifier.ModifyStatistic(stat, mechDef);
        }
        
        private float GetWalkSpeed()
        {
            var stat = statCollection.WalkSpeed();
            stat.CreateWithDefault(movement.WalkSpeed);
            return MechDefStatisticModifier.ModifyStatistic(stat, mechDef);
        }

        private float GetRunSpeed()
        {
            var stat = statCollection.RunSpeed();
            stat.CreateWithDefault(movement.RunSpeed);
            return MechDefStatisticModifier.ModifyStatistic(stat, mechDef);
        }

        private float GetJumpDistanceMultiplier()
        {
            var stat = statCollection.JumpDistanceMultiplier();
            stat.Create();
            return MechDefStatisticModifier.ModifyStatistic(stat, mechDef);
        }

        private float GetJumpCapacity()
        {
            var stat = statCollection.JumpCapacity();
            stat.Create();
            return MechDefStatisticModifier.ModifyStatistic(stat, mechDef);
        }

        private int GetJumpJetCount()
        {
            var raw = movement.JumpJetCount;
            var multiplier = GetJumpJetCountMultiplier();
            var mutiplied = raw * multiplier;
            var rounded = PrecisionUtils.RoundDownToInt(mutiplied);
            var cropped = Mathf.Min(rounded, mechDef.Chassis.MaxJumpjets);
            //Control.mod.Logger.Log($"raw={raw} multiplier={multiplier} mutiplied={mutiplied} rounded={rounded} cropped={cropped}");
            return cropped;
        }

        private float GetJumpJetCountMultiplier()
        {
            var stat = statCollection.JumpJetCountMultiplier();
            stat.Create();
            return MechDefStatisticModifier.ModifyStatistic(stat, mechDef);
        }
    }
}