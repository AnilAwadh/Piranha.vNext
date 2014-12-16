﻿/*
 * Piranha CMS
 * Copyright (c) 2014, Håkan Edling, All rights reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library.
 */

using System;

namespace Piranha.Config
{
	/// <summary>
	/// Commenting configuration.
	/// </summary>
	public static class Comments
	{
		/// <summary>
		/// Gets/sets if comments from authorized users should be moderated.
		/// </summary>
		public static bool ModerateAuthorized {
			get { return Utils.GetParam<bool>("comment_moderate_authorized", s => Convert.ToBoolean(s)); }
			set { Utils.SetParam("comment_moderate_authorized", value); }
		}

		/// <summary>
		/// Gets/sets if comments from anonymous users should be moderated.
		/// </summary>
		public static bool ModerateAnonymous {
			get { return Utils.GetParam<bool>("comment_moderate_anonymous", s => Convert.ToBoolean(s)); }
			set { Utils.SetParam("comment_moderate_anonymous", value); }
		}

		/// <summary>
		/// Gets/sets if the author should be notified by email when a comment is posted.
		/// </summary>
		public static bool NotifyAuthor {
			get { return Utils.GetParam<bool>("comment_notify_author", s => Convert.ToBoolean(s)); }
			set { Utils.SetParam("comment_notify_author", value); }
		}

		/// <summary>
		/// Gets/sets if moderators should be notified by email when a comment is posted.
		/// </summary>
		public static bool NotifyModerators {
			get { return Utils.GetParam<bool>("comment_notify_moderators", s => Convert.ToBoolean(s)); }
			set { Utils.SetParam("comment_notify_moderators", value); }
		}

		/// <summary>
		/// Gets/sets a comma separated list of email adresses.
		/// </summary>
		public static string Moderators {
			get { return Utils.GetParam<string>("comment_moderators", s => s); }
			set { Utils.SetParam("comment_moderators", value); }
		}
	}
}
