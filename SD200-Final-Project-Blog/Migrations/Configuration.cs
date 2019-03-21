namespace SD200_Final_Project_Blog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SD200_Final_Project_Blog.Controllers;
    using SD200_Final_Project_Blog.Models;
    using SD200_Final_Project_Blog.Models.Domain;
    using SD200_Final_Project_Blog.MyHelpers;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SD200_Final_Project_Blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "SD200_Final_Project_Blog.Models.ApplicationDbContext";
        }

        protected override void Seed(SD200_Final_Project_Blog.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            // Seeding Users and Roles
            List<ApplicationUser> initialUsers = PopulateUsersAndRolesAndSave(context);

            // Seeding one post per user
            PopulateDefaultPostsAndSave(context, initialUsers);


            List<Post> MyPosts = new List<Post>();
            #region Hard Coded Posts
            Post tekken = new Post()
            {
                Title = "Why you should play Tekken",
                Slug = "why-you-should-play-tekken",
                Published = true,
                User = initialUsers.First(user => user.UserName == "everettG@blog.com"),
                UserId = initialUsers.First(user => user.UserName == "everettG@blog.com").Id,
                HeroImageUrl = @"/UserUploads/PostHeroImages/tekken-splash.jpg",
                Body = "<h4>Let's start with tekken's origins</h4><p>Tekken is a fighting video game franchise created, developed, and published by <i>Namco</i>.</p><blockquote> Beginning with the original Tekken released in December 1994, the series has received several sequels as well as updates and spin-off titles.</blockquote> <p>Tekken was one of the first fighting games at the time to use 3D animation.</p>",
                Comments = new List<Comment>()
                {
                   new Comment()
                   {
                       Body = "Please try this game out!",
                       User = initialUsers.First(user => user.UserName == "everettG@blog.com"),
                       UserId = initialUsers.First(user => user.UserName == "everettG@blog.com").Id,
                   },
                   new Comment()
                   {
                       Body = "Sort of difficult but still fun",
                       User = initialUsers.First(user => user.UserName == "moderator@blog.com"),
                       UserId = initialUsers.First(user => user.UserName == "moderator@blog.com").Id,
                   },
                   new Comment()
                   {
                       Body = "Don't buy this game it will devour your time",
                       User = initialUsers.First(user => user.UserName == "admin@blog.com"),
                       UserId = initialUsers.First(user => user.UserName == "admin@blog.com").Id,
                   },
                },
            };
            Post mvc = new Post()
            {
                Title = "What is MVC?",
                Slug = "what-is-mvc",
                Published = true,
                User = initialUsers.First(user => user.UserName == "admin@blog.com"),
                UserId = initialUsers.First(user => user.UserName == "admin@blog.com").Id,
                HeroImageUrl = @"/UserUploads/PostHeroImages/mvc-diagram.png",
                Body = "<p>The Model-View-Controller (<strong>MVC</strong>) is an architectural pattern that separates an application into three main logical components: the <i>model</i>, the <i>view</i>, and the <i>controller</i>. Each of these components are built to handle specific development aspects of an application.</p>",
                Comments = new List<Comment>()
                {
                   new Comment()
                   {
                       Body = "This site was made using MVC and .NET Framework",
                       User = initialUsers.First(user => user.UserName == "admin@blog.com"),
                       UserId = initialUsers.First(user => user.UserName == "admin@blog.com").Id,
                   },
                   new Comment()
                   {
                       Body = "Thanks for the post, I'm still learning MVC at the moment",
                       User = initialUsers.First(user => user.UserName == "everettG@blog.com"),
                       UserId = initialUsers.First(user => user.UserName == "everettG@blog.com").Id,
                   },
                   new Comment()
                   {
                       Body = "Haven't tried MVC, but I think I would prefer Angular and MongoDB",
                       User = initialUsers.First(user => user.UserName == "moderator@blog.com"),
                       UserId = initialUsers.First(user => user.UserName == "moderator@blog.com").Id,
                   },
                },
            };
            #endregion

            context.Posts.AddOrUpdate(post => post.Title, tekken);
            context.Posts.AddOrUpdate(post => post.Title, mvc);

            context.SaveChanges();
        }

        private List<ApplicationUser> PopulateUsersAndRolesAndSave(SD200_Final_Project_Blog.Models.ApplicationDbContext context)
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            List<ApplicationUser> initialUsers = new List<ApplicationUser>();

            #region Creating Initial Users
            ApplicationUser admin = CreateUser(context, roleManager, userManager, "admin@blog.com", "admin@blog.com", userRole: nameof(UserRolesEnum.Admin));

            ApplicationUser moderator = CreateUser(context, roleManager, userManager, "moderator@blog.com", "moderator@blog.com", userRole: nameof(UserRolesEnum.Moderator));

            ApplicationUser everettGrassler = CreateUser(context, roleManager, userManager, "everettG@blog.com", "everettG@blog.com", userPassword: "123Everett", userRole: nameof(UserRolesEnum.Admin));
            #endregion

            initialUsers.Add(admin);
            initialUsers.Add(moderator);
            initialUsers.Add(everettGrassler);

            // Save changes made above to the database
            context.SaveChanges();

            return initialUsers;
        }

        private ApplicationUser CreateUser(
            SD200_Final_Project_Blog.Models.ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            string userName,
            string userEmail,
            string userPassword = "Password-1",
            string userRole = null
            )
        {
            /// <summary>
            ///    Adding new role if it doesn't exist. 
            /// </summary>
            if (!string.IsNullOrEmpty(userRole) && !string.IsNullOrWhiteSpace(userRole) && !context.Roles.Any(role => role.Name == userRole))
            {
                IdentityRole newRole = new IdentityRole(userRole);
                roleManager.Create(newRole);
            }

            // Creating the newUser
            ApplicationUser newUser;


            /// <summary>
            ///     new User will be made if userName doesn't exist on the DB
            ///     and if no role was passed in the user won't be added to a role
            /// </summary>
            if (!context.Users.Any(user => user.UserName == userName))
            {
                newUser = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userEmail
                };

                userManager.Create(newUser, userPassword);
            }
            else
            {
                /// <summary>
                ///     I'm using ".First()" and not ".FirstOrDefault()" because
                ///     the if statement above this will generate the user if
                ///     the user doesn't already exist in the database
                ///     (I'm 100% expecting this user to be in the database)
                /// </summary>
                newUser = context.Users.First(user => user.UserName == userName);
            }

            // Make sure the user is on the passed in role
            if (!string.IsNullOrEmpty(userRole) && !string.IsNullOrWhiteSpace(userRole) && !userManager.IsInRole(newUser.Id, userRole))
            {
                userManager.AddToRole(newUser.Id, userRole);
            }

            return newUser;
        }

        private void PopulateDefaultPostsAndSave(SD200_Final_Project_Blog.Models.ApplicationDbContext context, List<ApplicationUser> initialUsers)
        {
            // make one default post for all initial users
            for (int i = 0; i < initialUsers.Count; i++)
            {
                if (initialUsers[i].UserName != "moderator@blog.com")
                {
                    string title = $"{initialUsers[i].UserName.Replace("@blog", "").Replace(".com", "")}'s Post";
                    #region Body Placeholder Text
                    string body = (
                        "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque vitae lectus a ante finibus mollis at sit amet turpis. Integer consectetur, orci eu feugiat hendrerit, lectus mauris placerat nulla, venenatis convallis nibh purus nec tellus. <b>Aenean fringilla accumsan rutrum.</b> Donec fermentum, purus non feugiat auctor, risus purus tempus odio, ac volutpat risus risus non sapien. Aliquam erat volutpat. Aliquam congue, dui et semper sodales, mi enim faucibus mi, in molestie urna mi vitae turpis. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. <i>Maecenas condimentum massa et purus vehicula rhoncus.</i> Fusce lacinia, ante eget lacinia lacinia, mi mi luctus magna, at tempor nisi ante et nulla. Ut at diam pharetra, posuere velit id, scelerisque arcu. In mi elit, blandit vel tellus vel, finibus consequat dolor.</p> <br />  <p>Curabitur fermentum massa non mi imperdiet, nec mattis quam porta.Sed justo turpis, mollis in tristique et, condimentum id elit. <blockquote>Vestibulum sed erat mollis risus mattis feugiat aliquet sed augue</blockquote>. Vestibulum nec nibh eu dolor consequat pretium.Nunc pellentesque eros ut lectus lobortis vestibulum.Cras tellus turpis, lobortis convallis congue non, suscipit ac lectus. Mauris aliquam arcu ut orci ultricies, non commodo sem fermentum.Curabitur iaculis lacus nibh, at gravida urna facilisis in. Suspendisse massa est, consequat sit amet libero eget, porta lobortis quam.</p>"
                    );
                    #endregion
                    Post newPost = new Post()
                    {
                        Title = title,
                        Slug = title.GenerateSlug(HomeController.MyUnwantedSymbols),
                        User = initialUsers[i],
                        UserId = initialUsers[i].Id,
                        Published = true,
                        HeroImageUrl = $@"\UserUploads\PostHeroImages\blog-post-{1 + i}.jpeg",
                        Body = $@"<h1>Test Post {i} Body</h1>" + body,
                    };

                    // one comment per post (post creator owns this comment)
                    newPost.Comments.Add(new Comment()
                    {
                        User = initialUsers[i],
                        UserId = initialUsers[i].Id,
                        Body = $"{newPost.User.UserName}'s comment on \"{newPost.Title}\" post",
                    });

                    // Add new post to database if the name of the post doesn't match any in the database
                    context.Posts.AddOrUpdate(post => post.Title, newPost);
                }
            }


            // Save changes made above to the database
            context.SaveChanges();
        }
    }
}
