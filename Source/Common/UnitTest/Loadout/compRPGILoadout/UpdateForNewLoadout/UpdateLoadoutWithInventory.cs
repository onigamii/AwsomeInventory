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
    public class UpdateLoadoutWithInventory : Test_UpdateForNewLoadout
    {
        private RPGILoadout _loadout5;
        private List<RPGILoadout> _list;
        private compRPGILoadout _compRPGI;

        /// <summary>
        ///     Set up a list, with which this test first tests an increase in stack count,
        /// then tests a decrease in stack count
        /// </summary>
        public override void Setup()
        {
            _loadout5 = new RPGILoadout(_loadout4)
            {
                _finemeal,
                _shirtClothNormal,
                _knifePlasteelLegendary
            };

            _list = new List<RPGILoadout>()
            {
                _loadout5,
                _loadout4
            };

            LoadoutManager.AddLoadout(_loadout5);
            _pawn.equipment.AddEquipment((_knifePlasteelLegendary.DeepCopySimple()) as ThingWithComps);
            _pawn.inventory.innerContainer.TryAdd(_finemeal.DeepCopySimple());
        }

        public override void Run(out bool result)
        {
            result = true;
            _compRPGI = _pawn.GetComp<compRPGILoadout>();

            _pawn.SetLoadout(_loadout4);
            foreach (RPGILoadout loadout in _list)
            {
                _pawn.SetLoadout(loadout);

                result &= AssertUtility.AreEqual(loadout, _compRPGI.Loadout, nameof(loadout), nameof(_compRPGI.Loadout));
                result &= AssertUtility.AreEqual(
                    _compRPGI.InventoryTracker.Count
                    , loadout.Count
                    , string.Format(StringResource.ObjectCount, nameof(_compRPGI.InventoryTracker))
                    , string.Format(StringResource.ObjectCount, nameof(loadout)));
                foreach (Thing thing in loadout)
                {
                    result &= AssertUtility.Contains(
                        _compRPGI.InventoryTracker.Keys
                        , thing
                        , nameof(_compRPGI.InventoryTracker)
                        , thing.LabelCapNoCount);

                    result &= AssertUtility.Expect(
                        _compRPGI.InventoryTracker[thing]
                        , LoadoutUtility.CompareThing(thing, _knifePlasteelLegendary)
                            ? (thing.stackCount - _knifePlasteelLegendary.stackCount) * -1
                            : LoadoutUtility.CompareThing(thing, _finemeal)
                                ? (thing.stackCount - _finemeal.stackCount) * -1
                                : thing.stackCount * -1
                        , string.Format(StringResource.ObjectCount, nameof(_compRPGI.InventoryTracker)));

                    // Log.Warning(loadout.label + ": " + thing.LabelCapNoCount + ": " + _compRPGI.InventoryTracker[thing]);
                }
            }

            return;
        }

        public override void Cleanup()
        {
            _pawn.outfits.CurrentOutfit = null;
            _compRPGI.Loadout = null;
            _compRPGI.InventoryTracker = null;
            LoadoutManager.TryRemoveLoadout(_loadout5);
            _pawn.equipment.DestroyAllEquipment();
            _pawn.inventory.DestroyAll();
        }
    }
}
#endif