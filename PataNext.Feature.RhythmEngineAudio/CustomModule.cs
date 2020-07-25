﻿using System;
using DefaultEcs;
using GameHost.Core.Modules;
using GameHost.Injection;
using GameHost.Simulation.Application;
using GameHost.Threading;
using GameHost.Worlds;
using PataNext.Simulation.Client.Systems;

namespace PataNext.Feature.RhythmEngineAudio
{
	public class CustomModule : GameHostModule
	{
		public CustomModule(Entity source, Context ctxParent, GameHostModuleDescription original) : base(source, ctxParent, original)
		{
			var global = new ContextBindingStrategy(ctxParent, true).Resolve<GlobalWorld>();
			foreach (var listener in global.World.Get<IListener>())
			{
				if (listener is SimulationApplication simulationApplication)
				{
					simulationApplication.Data.Collection.GetOrCreate(typeof(ShoutDrumSystem));
				}
			}
		}
	}
}