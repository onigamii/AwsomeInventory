﻿// <copyright file="Logger.cs" company="Zizhen Li">
// Copyright (c) 2019 - 2020 Zizhen Li. All rights reserved.
// Licensed under the LGPL-3.0-only license. See LICENSE.md file in the project root for full license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace AwesomeInventory
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/>.
    /// </summary>
    public class Logger : ILogger
    {
        /// <inheritdoc/>
        public void Message(string message)
        {
            Log.Message(message, true);
        }
    }
}
