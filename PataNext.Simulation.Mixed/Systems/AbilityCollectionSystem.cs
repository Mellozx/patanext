﻿using System;
using System.Collections.Generic;
using GameHost.Core.Ecs;
using GameHost.Simulation.TabEcs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PataNext.Module.Simulation.BaseSystems;
using PataNext.Module.Simulation.Components;
using ZLogger;

namespace PataNext.Module.Simulation.Systems
{
	public class AbilityCollectionSystem : AppSystem
	{
		private ILogger                                       logger;
		private Dictionary<string, BaseRhythmAbilityProvider> providerMap;

		public AbilityCollectionSystem(WorldCollection collection) : base(collection)
		{
			providerMap = new Dictionary<string, BaseRhythmAbilityProvider>();
			
			DependencyResolver.Add(() => ref logger);
			// this is a dependency we don't use, but since the providers use them, it help other to indicate that we may need to wait a bit more
			DependencyResolver.Add<LocalRhythmCommandResourceManager>();
		}

		public GameEntity SpawnFor<TJsonData>(string           abilityId,                               GameEntity owner,
		                           AbilitySelection selection = AbilitySelection.Horizontal, 
		                           TJsonData jsonData = default)
		{
			return SpawnFor(abilityId, owner, selection, JsonConvert.SerializeObject(jsonData));
		}

		public GameEntity SpawnFor(string           abilityId, GameEntity owner,
		                           AbilitySelection selection = AbilitySelection.Horizontal,
		                           string           jsonData  = null)
		{
			if (!providerMap.TryGetValue(abilityId, out var provider))
			{
				logger.ZLogWarning("No provider for ability '{0}'", abilityId);
				return default;
			}

			provider.ProvidedJson = jsonData;
			return provider.SpawnEntityWithArguments(new CreateAbility
			{
				Owner     = owner,
				Selection = selection
			});
		}

		public void Register(BaseRhythmAbilityProvider abilityProvider)
		{
			providerMap[abilityProvider.MasterServerId] = abilityProvider;
		}
	}
}