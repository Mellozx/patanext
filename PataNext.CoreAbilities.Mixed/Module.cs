﻿using System;
using DefaultEcs;
using GameHost.Core.Ecs;
using GameHost.Core.Modules;
using GameHost.Core.Modules.Feature;
using GameHost.Core.Threading;
using GameHost.Injection;
using GameHost.IO;
using GameHost.Simulation.Application;
using GameHost.Threading;
using GameHost.Worlds;
using PataNext.Game.Abilities;
using Module = PataNext.CoreAbilities.Mixed.Module;

[assembly: RegisterAvailableModule("PataNext Standard Abilities", "guerro", typeof(Module))]

namespace PataNext.CoreAbilities.Mixed
{
	public class Module : GameHostModule, IModuleHasAbilityDescStorage
	{
		private AbilityDescStorage abilityDescStorage;

		public Module(Entity source, Context ctxParent, GameHostModuleDescription description) : base(source, ctxParent, description)
		{
			var global = new ContextBindingStrategy(ctxParent, true).Resolve<GlobalWorld>();
			foreach (ref readonly var listener in global.World.Get<IListener>())
			{
				if (listener is SimulationApplication simulationApplication)
				{
					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultMarchAbilityProvider));
					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultMarchAbilitySystem));

					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultBackwardAbilityProvider));
					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultBackwardAbilitySystem));

					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultRetreatAbilityProvider));
					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultRetreatAbilitySystem));

					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultJumpAbilityProvider));
					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultJumpAbilitySystem));

					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultPartyAbilityProvider));
					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultPartyAbilitySystem));

					simulationApplication.Data.Collection.GetOrCreate(typeof(Defaults.DefaultChargeAbilityProvider));

					simulationApplication.Data.Collection.GetOrCreate(typeof(CTate.TaterazayBasicDefendFrontalAbilityProvider));
					simulationApplication.Data.Collection.GetOrCreate(typeof(CTate.TaterazayBasicDefendFrontalAbilitySystem));

					simulationApplication.Data.Collection.GetOrCreate(typeof(CTate.TaterazayBasicDefendStayAbilityProvider));
					simulationApplication.Data.Collection.GetOrCreate(typeof(CTate.TaterazayBasicDefendStayAbilitySystem));

					simulationApplication.Data.Collection.GetOrCreate(typeof(CTate.TaterazayEnergyFieldAbilityProvider));
					simulationApplication.Data.Collection.GetOrCreate(typeof(CTate.TaterazayEnergyFieldAbilitySystem));

					simulationApplication.Data.Collection.GetOrCreate(typeof(Subset.DefaultSubsetMarchAbilitySystem));
				}
			}

			Storage.Subscribe((_, exteriorStorage) =>
			{
				var storage = exteriorStorage switch
				{
					{} => new StorageCollection {exteriorStorage, DllStorage},
					null => new StorageCollection {DllStorage}
				};

				abilityDescStorage = new AbilityDescStorage(storage.GetOrCreateDirectoryAsync("Abilities").Result);
			}, true);

			global.Scheduler.Schedule(tryLoadModule, SchedulingParameters.AsOnce);
		}

		public AbilityDescStorage Value => abilityDescStorage;

		private void tryLoadModule()
		{
			var global = new ContextBindingStrategy(Ctx.Parent, true).Resolve<GlobalWorld>();
			foreach (var ent in global.World)
			{
				if (ent.TryGet(out RegisteredModule registeredModule)
				    && registeredModule.State == ModuleState.None
				    && registeredModule.Description.NameId == "PataNext.CoreAbilities.Server")
				{
					Console.WriteLine("Load Server Module!");
					/*global.World.CreateEntity()
					      .Set(new RequestLoadModule {Module = ent});*/
					return;
				}
			}
			
			global.Scheduler.Schedule(tryLoadModule, SchedulingParameters.AsOnce);
		}

		protected override void OnDispose()
		{
		}
	}
}