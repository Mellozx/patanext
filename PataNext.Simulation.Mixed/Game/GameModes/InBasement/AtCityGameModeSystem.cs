﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameHost.Core.Ecs;
using GameHost.Simulation.TabEcs.Interfaces;
using GameHost.Simulation.Utility.EntityQuery;
using PataNext.Module.Simulation.Components;
using PataNext.Module.Simulation.Components.GameModes;
using StormiumTeam.GameBase.Roles.Descriptions;
using StormiumTeam.GameBase.SystemBase;

namespace PataNext.Module.Simulation.GameModes.InBasement
{
	public class AtCityGameModeSystem : GameModeSystemBase<AtCityGameModeData>
	{
		public struct PlayerFreeRoamCharacter : IComponentData
		{
			
		}
		
		public AtCityGameModeSystem(WorldCollection collection) : base(collection)
		{
		}

		private EntityQuery playerQuery, playerWithoutInputQuery, playerWithInputQuery;

		protected override void OnDependenciesResolved(IEnumerable<object> dependencies)
		{
			base.OnDependenciesResolved(dependencies);

			playerQuery             = CreateEntityQuery(new[] {typeof(PlayerDescription)});
			playerWithInputQuery    = QueryWith(playerQuery, new[] {typeof(FreeRoamInputComponent)});
			playerWithoutInputQuery = QueryWithout(playerQuery, new[] {typeof(FreeRoamInputComponent)});
		}

		protected override async Task GetStateMachine(CancellationToken token)
		{
			while (!token.IsCancellationRequested)
			{
				// Add missing input component to players
				foreach (var entity in playerWithoutInputQuery.GetEnumerator())
					AddComponent<FreeRoamInputComponent>(entity);

				await Task.Yield();
			}

			// Remove input component from players
			foreach (var entity in playerWithInputQuery.GetEnumerator())
				GameWorld.RemoveComponent(entity, AsComponentType<FreeRoamInputComponent>());
		}
	}
}