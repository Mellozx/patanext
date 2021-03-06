﻿using GameHost.Core.Ecs;
using GameHost.Core.Threading;
using GameHost.Simulation.TabEcs;
using GameHost.Simulation.TabEcs.HLAPI;
using GameHost.Simulation.TabEcs.Interfaces;
using GameHost.Simulation.Utility.EntityQuery;
using PataNext.Module.Simulation.Components.GamePlay.Abilities;
using PataNext.Module.Simulation.Passes;
using PataNext.Simulation.mixed.Components.GamePlay.Abilities;
using StormiumTeam.GameBase.SystemBase;

namespace PataNext.Module.Simulation.Game.GamePlay.Abilities
{
	public class ExecuteActiveAbilitySystem : GameAppSystem, IAbilitySimulationPass
	{
		public readonly IScheduler Post;

		public ExecuteActiveAbilitySystem(WorldCollection collection) : base(collection)
		{
			Post = new Scheduler();
		}

		private EntityQuery abilityMaskQuery;
		private EntityQuery setupQuery;
		private EntityQuery executorQuery;

		public void OnAbilitySimulationPass()
		{
			(abilityMaskQuery ??= CreateEntityQuery(new[]
			{
				typeof(AbilityState),
				typeof(ExecutableAbility)
			})).CheckForNewArchetypes();

			var setupAccessor = GetAccessor<SetupExecutableAbility>();
			foreach (var entity in setupQuery ??= CreateEntityQuery(new[]
			{
				typeof(SetupExecutableAbility)
			}))
			{
				setupAccessor[entity].Function(entity);
			}

			var ownerActiveAbilityAccessor = GetAccessor<OwnerActiveAbility>();
			var executableAccessor         = GetAccessor<ExecutableAbility>();
			foreach (var entity in executorQuery ??= CreateEntityQuery(new[]
			{
				typeof(OwnerActiveAbility)
			}))
			{
				ref readonly var ownerActiveAbility = ref ownerActiveAbilityAccessor[entity];
				TryInvoke(entity, ownerActiveAbility.PreviousActive, in executableAccessor);
				TryInvoke(entity, ownerActiveAbility.Active, in executableAccessor);
				TryInvoke(entity, ownerActiveAbility.Incoming, in executableAccessor);
			}

			Post.Run();
		}

		private void TryInvoke(GameEntity owner, GameEntity ability, in ComponentDataAccessor<ExecutableAbility> accessor)
		{
			if (abilityMaskQuery.MatchAgainst(ability))
				accessor[ability].Function?.Invoke(owner, ability, GetComponentData<AbilityState>(ability));
		}
	}
}