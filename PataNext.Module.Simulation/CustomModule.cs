﻿using System;
using System.Collections.Generic;
using DefaultEcs;
using GameHost.Applications;
using GameHost.Core.Applications;
using GameHost.Core.Ecs;
using GameHost.Core.Modding;
using GameHost.Entities;
using GameHost.Injection;
using PataNext.Module.Simulation;
using PataNext.Module.RhythmEngine;
using PataNext.Module.RhythmEngine.Data;
using PataponGameHost.RhythmEngine.Components;

[assembly: ModuleDescription("PataNext Simulation", "guerro", typeof(CustomModule))]

namespace PataNext.Module.Simulation
{
	public class CustomModule : CModule
	{
		public CustomModule(Entity source, Context ctxParent, SModuleInfo original) : base(source, ctxParent, original)
		{
			Console.WriteLine("My custom module has been loaded!");

			var simulationClient = new GameSimulationThreadingClient();
			simulationClient.Connect();

			simulationClient.InjectAssembly(GetType().Assembly);

			var inputClient = new GameInputThreadingClient();
			inputClient.Connect();

			inputClient.InjectAssembly(GetType().Assembly);
		}
	}

	[RestrictToApplication(typeof(GameSimulationThreadingHost))]
	public class TestModSystem : AppSystem
	{
		private IManagedWorldTime worldTime;

		private CModule module;

		public TestModSystem(WorldCollection collection) : base(collection)
		{
			DependencyResolver.Add(() => ref worldTime);
			DependencyResolver.Add(() => ref module);
		}

		protected override void OnDependenciesResolved(IEnumerable<object> dependencies)
		{
			for (var i = 0; i != 1; i++)
			{
				var ent = World.Mgr.CreateEntity();
				ent.Set(new RhythmEngineController {State      = EngineControllerState.Playing, StartTime = worldTime.Total.Add(TimeSpan.FromSeconds(2))});
				ent.Set(new RhythmEngineSettings {BeatInterval = TimeSpan.FromSeconds(0.5), MaxBeat   = 4});
				ent.Set(new RhythmEngineLocalState());
				ent.Set(new RhythmEngineExecutingCommand());
				ent.Set(new GameComboState());
				ent.Set(new GameCommandState());
				ent.Set(new RhythmEngineLocalCommandBuffer());
				ent.Set(new RhythmEnginePredictedCommandBuffer());
			}
			
			World.Mgr.CreateEntity().Set(new RhythmCommandDefinition("march", stackalloc[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Pata),
				RhythmCommandAction.With(1, RhythmKeys.Pata),
				RhythmCommandAction.With(2, RhythmKeys.Pata),
				RhythmCommandAction.With(3, RhythmKeys.Pon),
			}));
			World.Mgr.CreateEntity().Set(new RhythmCommandDefinition("defend", stackalloc[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Chaka),
				RhythmCommandAction.With(1, RhythmKeys.Chaka),
				RhythmCommandAction.With(2, RhythmKeys.Pata),
				RhythmCommandAction.With(3, RhythmKeys.Pon),
			}));
			World.Mgr.CreateEntity().Set(new RhythmCommandDefinition("attack", stackalloc[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Pon),
				RhythmCommandAction.With(1, RhythmKeys.Pon),
				RhythmCommandAction.With(2, RhythmKeys.Pata),
				RhythmCommandAction.With(3, RhythmKeys.Pon),
			}));
			World.Mgr.CreateEntity().Set(new RhythmCommandDefinition("fast_defend", stackalloc[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Chaka),
				RhythmCommandAction.WithSlider(1, 1, RhythmKeys.Pon),
			}));
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();
		}
	}
}