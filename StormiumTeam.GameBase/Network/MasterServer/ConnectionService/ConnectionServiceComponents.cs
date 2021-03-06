﻿using System;
using System.Threading.Tasks;
using DefaultEcs;
using GameHost.Core.Ecs;
using STMasterServer.Shared.Services;

namespace StormiumTeam.GameBase.Network.MasterServer.UserService
{
	public struct DisconnectUserRequest
	{
		public string Token;

		public DisconnectUserRequest(string token)
		{
			Token = token;
		}

		public class Process : MasterServerRequestService<IConnectionService, DisconnectUserRequest>
		{
			public Process(WorldCollection collection) : base(collection)
			{
			}

			protected override async Task OnUnprocessedRequest(Entity entity, RequestCallerStatus callerStatus)
			{
				await Service.Disconnect(entity.Get<DisconnectUserRequest>().Token);
			}
		}
	}
}