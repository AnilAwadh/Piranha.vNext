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
using System.Web;
using Piranha.Client.Models;
using Piranha.Server;

namespace Piranha.Feed
{
	/// <summary>
	/// Request handler for syndication feeds.
	/// </summary>
	public class FeedHandler : IHandler
	{
		/// <summary>
		/// Tries to handle an incoming request.
		/// </summary>
		/// <param name="api">The current api</param>
		/// <param name="request">The incoming route request</param>
		/// <returns>The result</returns>
		public IResponse Handle(Api api, IRequest request) {
			var now = DateTime.Now;

			if (request.Segments.Length == 1 || String.IsNullOrWhiteSpace(request.Segments[1])) {
				// Post feed for the entire site
				var posts = api.Posts.Get(where: p => p.Published <= now, limit: Config.Site.ArchivePageSize);

				var feed = new Syndication.PostRssFeed(posts);
				var response = request.StreamResponse();

				feed.Write(response);

				return response;
			} else if (request.Segments[1] == "comments") {
				// Comment feed for the entire site
				var comments = api.Comments.Get(where: c => c.IsApproved, 
					order: q => q.OrderByDescending(c => c.Created),
					limit: Config.Site.ArchivePageSize);

				var feed = new Syndication.CommentRssFeed(comments);
				var response = request.StreamResponse();

				feed.Write(response);

				return response;
			} else {
				var type = api.PostTypes.GetSingle(request.Segments[1]);

				if (type != null) {
					// Comment feed for an individual post
					if (request.Segments.Length > 2 && !String.IsNullOrWhiteSpace(request.Segments[2])) {
						var post = PostModel.GetBySlug(request.Segments[2], type.Id).WithComments();

						if (post != null) {
							var comments = api.Comments.Get(where: c => c.IsApproved && c.PostId == post.Id,
								order: q => q.OrderByDescending(c => c.Created),
								limit: Config.Site.ArchivePageSize);

							var feed = new Syndication.CommentRssFeed(comments);
							var response = request.StreamResponse();

							feed.Write(response);

							return response;
						}
					} else {
						// Post feed for an individual post type
						var posts = api.Posts.Get(where: p => p.Published <= now && p.TypeId == type.Id, 
							limit: Config.Site.ArchivePageSize);

						var feed = new Syndication.PostRssFeed(posts);
						var response = request.StreamResponse();

						feed.Write(response);

						return response;
					}
				}
			}
			return null;
		}
	}
}
