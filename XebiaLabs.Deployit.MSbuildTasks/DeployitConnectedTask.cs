using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using XebiaLabs.Deployit.Client;
using XebiaLabs.Deployit.Client.Manifest;
using XebiaLabs.Deployit.Client.Package;

namespace XebiaLabs.Deployit.MSbuildTasks
{
	public abstract class DeployitConnectedTask : Task
	{
		[Required]
		public string DeployitURL { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }

		
		public bool CheckConnection { get; set; }

	   
		protected abstract bool ExecuteCore(DeployitServer deployitServer);


		public override bool Execute()
		{
			var server = new DeployitServer();
			try
			{
				server.Connect(new Uri(DeployitURL), new NetworkCredential(Username, Password), CheckConnection);
				return ExecuteCore(server);
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex);
				return false;
			}
			finally
			{
				server.Disconnect();
			}
		}

	}

}