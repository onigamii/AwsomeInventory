﻿// <copyright file="ApparelOptionUtility.cs" company="Zizhen Li">
// Copyright (c) 2019 - 2020 Zizhen Li. All rights reserved.
// Licensed under the LGPL-3.0-only license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace AwesomeInventory
{
    /// <summary>
    /// Provides support for apparel options displayed in menu.
    /// </summary>
    public static class ApparelOptionUtility
    {
        /// <summary>
        /// Check if a <paramref name="pawn"/> can wear <paramref name="apparel"/>.
        /// </summary>
        /// <param name="pawn"> Pawn that wants to wear <paramref name="apparel"/>. </param>
        /// <param name="apparel"> Apparel to wear. </param>
        /// <returns> Returns true if <paramref name="pawn"/> can wear <paramref name="apparel"/>. </returns>
        public static bool CanWear(Pawn pawn, Apparel apparel)
        {
            ValidateArg.NotNull(pawn, nameof(pawn));
            ValidateArg.NotNull(apparel, nameof(apparel));

            return !pawn.apparel.WouldReplaceLockedApparel(apparel)
                && (apparel.def.apparel.gender == Gender.None || apparel.def.apparel.gender == pawn.gender)
                && (!apparel.def.apparel.tags.Contains("Royal") || pawn.royalty.AllTitlesInEffectForReading.Count != 0)
                && ApparelUtility.HasPartsToWear(pawn, apparel.def);
        }
    }
}
