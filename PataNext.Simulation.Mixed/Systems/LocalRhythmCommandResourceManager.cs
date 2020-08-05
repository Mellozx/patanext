﻿using System.Collections.Generic;
using GameHost.Core.Ecs;
using GameHost.Simulation.TabEcs;
using PataNext.Module.Simulation.Components.GamePlay.RhythmEngine.Structures;
using PataNext.Module.Simulation.Game.RhythmEngine;
using PataNext.Module.Simulation.Resources;
using PataNext.Simulation.mixed.Components.GamePlay.RhythmEngine.DefaultCommands;
using StormiumTeam.GameBase.SystemBase;

namespace PataNext.Module.Simulation.Systems
{
	public class LocalRhythmCommandResourceManager : GameAppSystem
	{
		public RhythmCommandResourceDb DataBase;
		
		public LocalRhythmCommandResourceManager(WorldCollection collection) : base(collection)
		{
			DependencyResolver.Add(() => ref DataBase);
		}

		protected override void OnDependenciesResolved(IEnumerable<object> dependencies)
		{
			base.OnDependenciesResolved(dependencies);

			AddComponent(DataBase.GetOrCreate(AsComponentType<MarchCommand>(), "march", new[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Pata),
				RhythmCommandAction.With(1, RhythmKeys.Pata),
				RhythmCommandAction.With(2, RhythmKeys.Pata),
				RhythmCommandAction.With(3, RhythmKeys.Pon),
			}).Entity, new MarchCommand());
			/*gameWorld.AddBuffer<RhythmCommandActionBuffer>(localCommandDb.GetOrCreate(("attack", 4)).Entity).AddRangeReinterpret(stackalloc[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Pon),
				RhythmCommandAction.With(1, RhythmKeys.Pon),
				RhythmCommandAction.With(2, RhythmKeys.Pata),
				RhythmCommandAction.With(3, RhythmKeys.Pon),
			});
			gameWorld.AddBuffer<RhythmCommandActionBuffer>(localCommandDb.GetOrCreate(("defend", 4)).Entity).AddRangeReinterpret(stackalloc[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Chaka),
				RhythmCommandAction.With(1, RhythmKeys.Chaka),
				RhythmCommandAction.With(2, RhythmKeys.Pata),
				RhythmCommandAction.With(3, RhythmKeys.Pon),
			});
			gameWorld.AddBuffer<RhythmCommandActionBuffer>(localCommandDb.GetOrCreate(("summon", 4)).Entity).AddRangeReinterpret(stackalloc[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Don),
				RhythmCommandAction.With(1, RhythmKeys.Don),
				RhythmCommandAction.WithOffset(1, 0.5f, RhythmKeys.Don),
				RhythmCommandAction.WithOffset(2, 0.5f, RhythmKeys.Don),
				RhythmCommandAction.WithOffset(3, 0, RhythmKeys.Don),
			});
			gameWorld.AddBuffer<RhythmCommandActionBuffer>(localCommandDb.GetOrCreate(("quick_defend", 4)).Entity).AddRangeReinterpret(stackalloc[]
			{
				RhythmCommandAction.With(0, RhythmKeys.Chaka),
				RhythmCommandAction.WithSlider(1, 1, RhythmKeys.Pon)
			});*/
		}
	}
}