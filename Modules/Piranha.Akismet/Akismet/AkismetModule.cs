﻿/*
 * Copyright (c) 2014 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha.vnext
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Piranha.Extend;
using Piranha.Client.Models;

namespace Piranha.Akismet
{
	/// <summary>
	/// Main entry point for the Akismet module.
	/// </summary>
	public class AkismetModule : IModule
	{
		#region Members
		/// <summary>
		/// Private akismet api.
		/// </summary>
		private readonly AkismetApi akismet = new AkismetApi();
		#endregion

		/// <summary>
		/// Initializes the module. This method should be used for
		/// ensuring runtime resources and registering hooks.
		/// </summary>
		public void Init() {
			using (var api = new Api()) {
				// Ensure configuration params
				var param = api.Params.GetSingle(where: p => p.Name == "akismet_apikey");
				if (param == null) {
					param = new Models.Param() { 
						Name = "akismet_apikey"
					};
					api.Params.Add(param);
				}
				param = api.Params.GetSingle(where: p => p.Name == "akismet_siteurl");
				if (param == null) {
					param = new Models.Param() { 
						Name = "akismet_siteurl",
					};
					api.Params.Add(param);
				}

				// Save changes
				api.SaveChanges();
			}

			// Add configuration to the manager
			Manager.Config.Blocks.Add(new Manager.Config.ConfigBlock("Blogging", "Akismet", new List<Manager.Config.ConfigRow>() {
				new Manager.Config.ConfigRow(new List<Manager.Config.ConfigColumn>() {
					new Manager.Config.ConfigColumn(new List<Manager.Config.ConfigItem>() {
						new Manager.Config.ConfigString() {
							Name = "API-key", Param = "akismet_apikey", Value = Config.Akismet.ApiKey
						}
					}),
					new Manager.Config.ConfigColumn(new List<Manager.Config.ConfigItem>() {
						new Manager.Config.ConfigString() {
							Name = "Site URL", Param = "akismet_siteurl", Value = Config.Akismet.SiteUrl
						}
					})
				})
			}));

			// Add model hooks
			Hooks.Models.Comment.OnSave += (c) => {
				if (!String.IsNullOrWhiteSpace(Config.Akismet.ApiKey)) {
					//
					// TODO: Maybe check if the comment exists in the database
					//
					if (akismet.VerifyKey()) {
						if (akismet.CommentCheck(c)) {
							App.Logger.Log(Log.LogLevel.WARNING, "Akismet: Comment was marked as spam by Akismet");
							c.IsSpam = true;
						}
					} else {
						App.Logger.Log(Log.LogLevel.ERROR, "Akismet: ApiKey verification failed.");
					}
				} else {
					App.Logger.Log(Log.LogLevel.INFO, "Akismet: No api key configured. Skipping comment validation.");
				}
			};
		}
	}
}