namespace SD200_Final_Project_Blog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SD200_Final_Project_Blog.Models;
    using SD200_Final_Project_Blog.Models.Domain;
    using System;
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
            // make one default movie for all initial users
            for (int i = 0; i < initialUsers.Count; i++)
            {
                Post newPost = new Post()
                {
                    Id = Guid.NewGuid(),
                    Title = $"{initialUsers[i].UserName}'s Post",
                    User = initialUsers[i],
                    UserId = initialUsers[i].Id,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Published = true,
                    HeroImageUrl = $@"\UserUploads\PostHeroImages\blog-post-{1 + i}.jpeg",
                    Body = $@"<h1>Test Post {i} Body</h1>\n",
                };

                // Add new movie to database if the name of the movie doesn't match any in the database
                context.Posts.AddOrUpdate(post => post.Title, newPost);
            }


            // Save changes made above to the database
            context.SaveChanges();
        }
    }
}
