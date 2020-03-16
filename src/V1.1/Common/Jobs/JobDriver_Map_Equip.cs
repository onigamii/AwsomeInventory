﻿// <copyright file="JobDriver_Map_Equip.cs" company="Zizhen Li">
// Copyright (c) Zizhen Li. All rights reserved.
// Licensed under the GPL-3.0-only license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using RimWorld;
using Verse;
using Verse.AI;

namespace AwesomeInventory.Common
{
    /// <summary>
    /// Instruction on how to equip items on the map.
    /// </summary>
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Follow naming convention")]
    public class JobDriver_Map_Equip : JobDriver
    {
        /// <summary>
        /// Make reservation before action.
        /// </summary>
        /// <param name="errorOnFailed"> If true, throw errors on failed. </param>
        /// <returns> Returns true if a reservation is made. </returns>
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        /// <summary>
        /// Make detailed instruction on how to do the job.
        /// </summary>
        /// <returns> Instructions on what to do. </returns>
        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnDestroyedOrNull(TargetIndex.A);
            this.FailOnBurningImmobile(TargetIndex.A);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
            yield return new Toil()
            {
                initAction = () =>
                {
                    ThingWithComps thingWithComps = (ThingWithComps)job.targetA.Thing;
                    ThingWithComps thingWithComps2 = null;
                    if (thingWithComps.def.stackLimit > 1 && thingWithComps.stackCount > 1)
                    {
                        thingWithComps2 = (ThingWithComps)thingWithComps.SplitOff(1);
                    }
                    else
                    {
                        thingWithComps2 = thingWithComps;
                        thingWithComps2.DeSpawn();
                    }

                    // put away equiped weapon first
                    if (pawn.equipment.Primary != null)
                    {
                        if (!pawn.equipment.TryTransferEquipmentToContainer(pawn.equipment.Primary, pawn.inventory.innerContainer))
                        {
                            // if failed, drop the weapon
                            pawn.equipment.MakeRoomFor(thingWithComps2);
                        }
                    }

                    if (pawn.equipment.Primary == null)
                    {
                        // unregister new weapon in the inventory list and register it in equipment list.
                        pawn.equipment.GetDirectlyHeldThings().TryAddOrTransfer(thingWithComps2);
                    }
                    else
                    {
                        Messages.Message("CannotEquip".Translate(thingWithComps2.LabelShort), MessageTypeDefOf.RejectInput, false);
                    }
                },
            };
        }
    }
}