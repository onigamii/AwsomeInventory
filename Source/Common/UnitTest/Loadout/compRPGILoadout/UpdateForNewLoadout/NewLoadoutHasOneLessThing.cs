﻿#if UnitTest
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

#if RPG_Inventory_Remake
using RPG_Inventory_Remake.Loadout;
using RPG_Inventory_Remake;
#endif

namespace RPG_Inventory_Remake_Common.UnitTest
{
    public class NewLoadoutHasOneLessThing : Test_UpdateForNewLoadout
    {
        private compRPGILoadout _compRPGI;

        public override void Setup()
        {
            _pawn.SetLoadout(_loadouts.Last());
        }

        public override void Run(out bool result)
        {
            _compRPGI = _pawn.GetComp<compRPGILoadout>();
            result = true;

            for (int i = _loadouts.Count - 2; i > -1; i--)
            {
                RPGILoadout loadout = _loadouts[i];
                if (loadout != null)
                {
                    _pawn.SetLoadout(loadout);
                    // Check if they are the same instance
                    result &= AssertUtility.AreEqual(loadout, _compRPGI.Loadout, nameof(loadout), nameof(_compRPGI.Loadout));
                    // Check if InventoryTracker and Loadout has the same number of items
                    result &= AssertUtility.AreEqual(
                        _compRPGI.InventoryTracker.Count
                        , loadout.Count
                        , string.Format(StringResource.ObjectCount, nameof(_compRPGI.InventoryTracker))
                        , string.Format(StringResource.ObjectCount, nameof(loadout)));
                    foreach (Thing thing in loadout)
                    {
                        // Check if InventoryTracker has same items as Loadout
                        result &= AssertUtility.Contains(
                            _compRPGI.InventoryTracker.Keys
                            , thing
                            , nameof(_compRPGI.InventoryTracker)
                            , thing.LabelCapNoCount);
                        // Check if stack count is matched
                        result &= AssertUtility.Expect(
                            _compRPGI.InventoryTracker[thing]
                            , thing.stackCount * -1
                            , string.Format(StringResource.ObjectCount, nameof(_compRPGI.InventoryTracker)));
                    }
                }
            }
            return;
        }

        public override void Cleanup()
        {
            _pawn.outfits.CurrentOutfit = null;
            _compRPGI.Loadout = null;
            _compRPGI.InventoryTracker = null;
        }
    }
}
#endif