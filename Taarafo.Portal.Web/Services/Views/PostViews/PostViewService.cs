﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE TO CONNECT THE WORLD
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taarafo.Portal.Web.Brokers.Loggings;
using Taarafo.Portal.Web.Models.Posts;
using Taarafo.Portal.Web.Models.PostViews;
using Taarafo.Portal.Web.Services.Foundations.Posts;

namespace Taarafo.Portal.Web.Services.Views.PostViews
{
    public partial class PostViewService : IPostViewService
    {
        private readonly IPostService postService;
        private readonly ILoggingBroker loggingBroker;

        public PostViewService(
            IPostService postService,
            ILoggingBroker loggingBroker)
        {
            this.postService = postService;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<List<PostView>> RetrieveAllPostViewsAsync() =>
        TryCatch(async () =>
        {
            List<Post> posts =
                await this.postService.RetrieveAllPostsAsync();

            return posts.Select(AsPostView).ToList();
        });

        public ValueTask<PostView> RemovePostViewByIdAsync(Guid postViewId) =>
        TryCatch(async () =>
        {
            ValidatePostViewId(postViewId);

            Post deletedPost =
               await this.postService.RemovePostByIdAsync(postViewId);

            return MapToPostView(deletedPost);
        });

        private static Func<Post, PostView> AsPostView =>
            post => MapToPostView(post);

        private static PostView MapToPostView(Post post)
        {
            return new PostView
            {
                Id = post.Id,
                Content = post.Content,
                CreatedDate = post.CreatedDate,
                UpdatedDate = post.UpdatedDate,
                Author = post.Author
            };
        }
    }
}
