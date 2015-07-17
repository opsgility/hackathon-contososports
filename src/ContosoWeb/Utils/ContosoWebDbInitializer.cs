using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using Contoso.Models;

namespace ContosoWeb.Utils
{
    public class ContosoWebDbInitializer : CreateDatabaseIfNotExists<ContosoWebContext>
    {
        protected override void Seed(ContosoWebContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var players = new List<Player>
            {
                new Player {Name = "J Hance", GoalsScored = 6},
                new Player {Name = "T Perham", GoalsScored = 5},
                new Player {Name = "D Pelton", GoalsScored = 5},
                new Player {Name = "C Herb", GoalsScored = 4}
            };
            context.Players.AddOrUpdate(x => x.Name, players.ToArray());
            context.SaveChanges();

            var teams = new List<Team>
            {
                new Team {Name = "Contoso Cubs", AbbreviatedName = "SEA", ShortName = "Cubs", LogoImageSrc = "contoso_cubs.svg", Players = players.Where(x => x.PlayerId == 1 || x.PlayerId == 2).ToList()},
                new Team {Name = "Farikam Falcons", AbbreviatedName = "LA", ShortName = "Falcons", LogoImageSrc = "fabrikam_falcons.svg", Players = players.Where(x => x.PlayerId == 3 || x.PlayerId == 4).ToList()}
            };
            context.Teams.AddOrUpdate(x => x.Name, teams.ToArray());
            context.SaveChanges();

            var gameStats = new List<GameStat>
            { 
                new GameStat { Team = teams.Single(x => x.TeamId == 1), Goals = 0, Shots = 0, Passes = 0, FoulsLost = 0, FoulsWon = 0, Offside = 0, Corners = 0 },
                new GameStat { Team = teams.Single(x => x.TeamId == 2), Goals = 0, Shots = 0, Passes = 0, FoulsLost = 0, FoulsWon = 0, Offside = 0, Corners = 0 }
            };
            context.GameStats.AddOrUpdate(x => x.Passes, gameStats.ToArray());
            context.SaveChanges();

            var matches = new List<Match>
            {
                new Match { HomeTeam = teams.Single(x => x.TeamId == 1), AwayTeam = teams.Single(x => x.TeamId == 2), MatchDate = DateTime.UtcNow.AddDays(2), Progress = MatchProgress.Pending },
                new Match { HomeTeam = teams.Single(x => x.TeamId == 1), AwayTeam = teams.Single(x => x.TeamId == 2), MatchDate = DateTime.UtcNow.AddMinutes(14), Progress = MatchProgress.InProgress, HomeTeamStats = gameStats.Single(x => x.Team.TeamId == 1), AwayTeamStats = gameStats.Single(x => x.Team.TeamId == 2) },
                new Match { HomeTeam = teams.Single(x => x.TeamId == 1), HomeTeamScore = 0, AwayTeam = teams.Single(x => x.TeamId == 2), AwayTeamScore = 0, MatchDate = new DateTimeOffset(new DateTime(2015, 6, 19), TimeSpan.FromHours(6)), Progress = MatchProgress.Completed },
                new Match { HomeTeam = teams.Single(x => x.TeamId == 1), HomeTeamScore = 1, AwayTeam = teams.Single(x => x.TeamId == 2), AwayTeamScore = 0, MatchDate = new DateTimeOffset(new DateTime(2015, 6, 17), TimeSpan.FromHours(6)), Progress = MatchProgress.Completed },
                new Match { HomeTeam = teams.Single(x => x.TeamId == 2), HomeTeamScore = 0, AwayTeam = teams.Single(x => x.TeamId == 1), AwayTeamScore = 3, MatchDate = new DateTimeOffset(new DateTime(2015, 6, 14), TimeSpan.FromHours(6)), Progress = MatchProgress.Completed },
                new Match { HomeTeam = teams.Single(x => x.TeamId == 1), HomeTeamScore = 1, AwayTeam = teams.Single(x => x.TeamId == 2), AwayTeamScore = 1, MatchDate = new DateTimeOffset(new DateTime(2015, 4, 12), TimeSpan.FromHours(6)), Progress = MatchProgress.Completed },
                new Match { HomeTeam = teams.Single(x => x.TeamId == 1), HomeTeamScore = 3, AwayTeam = teams.Single(x => x.TeamId == 2), AwayTeamScore = 0, MatchDate = new DateTimeOffset(new DateTime(2015, 2, 14), TimeSpan.FromHours(6)), Progress = MatchProgress.Completed }
            };
            context.Matches.AddOrUpdate(x => x.MatchDate, matches.ToArray());
            context.SaveChanges();

            var categories = new List<Category>{
                new Category { Name = "Men", Description = "Men's apparel", ImageUrl = "sla_men.png" },
                new Category { Name = "Women", Description = "Women's apparel", ImageUrl = "sla_women.png" },
                new Category { Name = "Kids", Description = "Kid's apparel", ImageUrl = "sla_kids.png" },
                new Category { Name = "Balls", Description = "Soccer Balls", ImageUrl = "sla_ball.jpg" },
                new Category { Name = "Tickets", Description = "Match tickets", ImageUrl = "tickets_blue.png" }
            };
            context.Categories.AddOrUpdate(x => x.Name, categories.ToArray());
            context.SaveChanges();
            var categoriesMap = categories.ToDictionary(c => c.Name, c => c.CategoryId);

            var products = new List<Product>
            {
                new Product
                {
                    SkuNumber = "TIK-0001",
                    Title = $"Cubs vs. Falcons - {DateTime.UtcNow.AddDays(2).ToString("MM/dd")}",
                    CategoryId = categoriesMap["Tickets"],
                    Price = 59.99M,
                    SalePrice = 59.99M,
                    ProductArtUrl = "Tickets_blue.png",
                    ProductDetails = "{ \"Match\" : \"Cubs vs. Falcons\" } ",
                    Description = "Ticket to the Cubs vs Falcons match.",
                    Inventory = 40000,
                    LeadTime = 0,
                    RecommendationId = 1
                },
                new Product
                {
                    SkuNumber = "TIK-0002",
                    Title = "Cubs vs. Falcons - " + DateTime.UtcNow.AddDays(9).ToString("MM/dd"),
                    CategoryId = categoriesMap["Tickets"],
                    Price = 59.99M,
                    SalePrice = 59.99M,
                    ProductArtUrl = "Tickets_blue.png",
                    ProductDetails = "{ \"Match\" : \"Cubs vs. Falcons\" } ",
                    Description = "Ticket to the Cubs vs Falcons match.",
                    Inventory = 40000,
                    LeadTime = 0,
                    RecommendationId = 2
                },
                new Product
                {
                    SkuNumber = "TIK-0003",
                    Title = "Cubs vs. Falcons - " + DateTime.UtcNow.AddDays(16).ToString("MM/dd"),
                    CategoryId = categoriesMap["Tickets"],
                    Price = 59.99M,
                    SalePrice = 59.99M,
                    ProductArtUrl = "Tickets_blue.png",
                    ProductDetails = "{ \"Match\" : \"Cubs vs. Falcons\" } ",
                    Description = "Ticket to the Cubs vs Falcons match.",
                    Inventory = 40000,
                    LeadTime = 0,
                    RecommendationId = 3
                },
                new Product
                {
                    SkuNumber = "TIK-0004",
                    Title = "Semi Final 1 - " + DateTime.UtcNow.AddDays(23).ToString("MM/dd"),
                    CategoryId = categoriesMap["Tickets"],
                    Price = 79.99M,
                    SalePrice = 79.99M,
                    ProductArtUrl = "Tickets_green.png",
                    ProductDetails = "{ \"Match\" : \"Semi Final 1 - Teams TBC\" } ",
                    Description = "Ticket to the Cubs vs Falcons match.",
                    Inventory = 40000,
                    LeadTime = 0,
                    RecommendationId = 4
                },
                new Product
                {
                    SkuNumber = "TIK-0005",
                    Title = "Semi Final 2 - " + DateTime.UtcNow.AddDays(30).ToString("MM/dd"),
                    CategoryId = categoriesMap["Tickets"],
                    Price = 79.99M,
                    SalePrice = 79.99M,
                    ProductArtUrl = "Tickets_green.png",
                    ProductDetails = "{ \"Match\" : \"Semi Final 2 - Teams TBC\" } ",
                    Description = "Ticket to the Cubs vs Falcons match.",
                    Inventory = 40000,
                    LeadTime = 0,
                    RecommendationId = 5
                },
                new Product
                {
                    SkuNumber = "TIK-0006",
                    Title = "Final - " + DateTime.UtcNow.AddDays(37).ToString("MM/dd"),
                    CategoryId = categoriesMap["Tickets"],
                    Price = 99.99M,
                    SalePrice = 99.99M,
                    ProductArtUrl = "Tickets_red.png",
                    ProductDetails = "{ \"Match\" : \"Final - Teams TBC\" } ",
                    Description = "Ticket to the Cubs vs Falcons match.",
                    Inventory = 40000,
                    LeadTime = 0,
                    RecommendationId = 6
                },
                new Product
                {
                    SkuNumber = "MEN-0001",
                    Title = "Men's Cubs Jersey - Black",
                    CategoryId = categoriesMap["Men"],
                    Price = 99.99M,
                    SalePrice = 79.99M,
                    ProductArtUrl = "cubs_b_m_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Contoso Cubs\",  \"Size\" : \"Large\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Black\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 4,
                    LeadTime = 6,
                    RecommendationId = 7
                },
                new Product
                {
                    SkuNumber = "MEN-0002",
                    Title = "Men's Falcons Jersey - Blue",
                    CategoryId = categoriesMap["Men"],
                    Price = 99.99M,
                    SalePrice = 79.99M,
                    ProductArtUrl = "falcon_b_m_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Fabrikam Falcons\",  \"Size\" : \"Large\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Blue\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 4,
                    LeadTime = 6,
                    RecommendationId = 8
                },
                new Product
                {
                    SkuNumber = "MEN-0003",
                    Title = "Men's Falcons Jersey - Yellow",
                    CategoryId = categoriesMap["Men"],
                    Price = 99.99M,
                    SalePrice = 79.99M,
                    ProductArtUrl = "falcon_y_m_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Fabrikam Falcons\",  \"Size\" : \"Large\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Yellow\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 4,
                    LeadTime = 6,
                    RecommendationId = 9
                },
                new Product
                {
                    SkuNumber = "MEN-0004",
                    Title = "Men's Cubs Jersey - White",
                    CategoryId = categoriesMap["Men"],
                    Price = 99.99M,
                    SalePrice = 79.99M,
                    ProductArtUrl = "cubs_w_m_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\",  \"Size\" : \"Large\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"White\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 7,
                    LeadTime = 0,
                    RecommendationId = 10
                },
                new Product
                {
                    SkuNumber = "MEN-0005",
                    Title = "Men's Cubs Jersey - Orange",
                    CategoryId = categoriesMap["Men"],
                    Price = 99.99M,
                    SalePrice = 79.99M,
                    ProductArtUrl = "cubs_o_m_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\",  \"Size\" : \"Large\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Orange\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 7,
                    LeadTime = 0,
                    RecommendationId = 11
                },
                new Product
                {
                    SkuNumber = "MEN-0006",
                    Title = "Men's Falcons Jersey - White",
                    CategoryId = categoriesMap["Men"],
                    Price = 99.99M,
                    SalePrice = 79.99M,
                    ProductArtUrl = "falcon_w_m_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\",  \"Size\" : \"Large\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"White\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 7,
                    LeadTime = 0,
                    RecommendationId = 12
                },
                new Product
                {
                    SkuNumber = "WMN-0001",
                    Title = "Women's Cubs Jersey - Black",
                    CategoryId = categoriesMap["Women"],
                    Price = 89.99M,
                    SalePrice = 89.99M,
                    ProductArtUrl = "cubs_b_w_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Contoso Cubs\",  \"Size\" : \"8\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Black\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 4,
                    LeadTime = 6,
                    RecommendationId = 13
                },
                new Product
                {
                    SkuNumber = "WMN-0002",
                    Title = "Women's Falcons Jersey - Blue",
                    CategoryId = categoriesMap["Women"],
                    Price = 89.99M,
                    SalePrice = 89.99M,
                    ProductArtUrl = "falcon_b_w_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Fabrikam Falcons\",  \"Size\" : \"8\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Blue\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 4,
                    LeadTime = 6,
                    RecommendationId = 14
                },
                new Product
                {
                    SkuNumber = "WMN-0003",
                    Title = "Women's Falcons Jersey - Yellow",
                    CategoryId = categoriesMap["Women"],
                    Price = 89.99M,
                    SalePrice = 89.99M,
                    ProductArtUrl = "falcon_y_w_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Fabrikam Falcons\",  \"Size\" : \"8\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Yellow\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 4,
                    LeadTime = 6,
                    RecommendationId = 15
                },
                new Product
                {
                    SkuNumber = "WMN-0004",
                    Title = "Women's Cubs Jersey - White",
                    CategoryId = categoriesMap["Women"],
                    Price = 89.99M,
                    SalePrice = 89.99M,
                    ProductArtUrl = "cubs_w_w_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\",  \"Size\" : \"8\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"White\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 7,
                    LeadTime = 0,
                    RecommendationId = 16
                },
                new Product
                {
                    SkuNumber = "WMN-0005",
                    Title = "Women's Cubs Jersey - Orange",
                    CategoryId = categoriesMap["Women"],
                    Price = 89.99M,
                    SalePrice = 89.99M,
                    ProductArtUrl = "falcon_o_w_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\",  \"Size\" : \"8\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Orange\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 7,
                    LeadTime = 0,
                    RecommendationId = 17
                },
                new Product
                {
                    SkuNumber = "WMN-0006",
                    Title = "Women's Falcons Jersey - White",
                    CategoryId = categoriesMap["Women"],
                    Price = 89.99M,
                    SalePrice = 89.99M,
                    ProductArtUrl = "falcon_w_w_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\",  \"Size\" : \"8\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"White\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 7,
                    LeadTime = 0,
                    RecommendationId = 18
                },
                new Product
                {
                    SkuNumber = "KID-0001",
                    Title = "Kid's Cubs Jersey - Black",
                    CategoryId = categoriesMap["Kids"],
                    Price = 39.99M,
                    SalePrice = 39.99M,
                    ProductArtUrl = "cubs_b_k_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Contoso Cubs\",  \"Size\" : \"Small\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Black\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 9,
                    LeadTime = 0,
                    RecommendationId = 19
                },
                new Product
                {
                    SkuNumber = "KID-0002",
                    Title = "Kid's Cubs Jersey - Orange",
                    CategoryId = categoriesMap["Kids"],
                    Price = 39.99M,
                    SalePrice = 39.99M,
                    ProductArtUrl = "cubs_o_k_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Contoso Cubs\",  \"Size\" : \"Small\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Orange\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 9,
                    LeadTime = 0,
                    RecommendationId = 20
                },
                new Product
                {
                    SkuNumber = "KID-0003",
                    Title = "Kid's Cubs Jersey - White",
                    CategoryId = categoriesMap["Kids"],
                    Price = 39.99M,
                    SalePrice = 39.99M,
                    ProductArtUrl = "cubs_w_k_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Contoso Cubs\",  \"Size\" : \"Small\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"White\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 9,
                    LeadTime = 0,
                    RecommendationId = 21
                },
                new Product
                {
                    SkuNumber = "KID-0004",
                    Title = "Kid's Falcons Jersey - Blue",
                    CategoryId = categoriesMap["Kids"],
                    Price = 39.99M,
                    SalePrice = 39.99M,
                    ProductArtUrl = "falcon_b_k_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Fabrikam Falcons\",  \"Size\" : \"Small\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Blue\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 9,
                    LeadTime = 0,
                    RecommendationId = 22
                },
                new Product
                {
                    SkuNumber = "KID-0005",
                    Title = "Kid's Falcons Jersey - White",
                    CategoryId = categoriesMap["Kids"],
                    Price = 39.99M,
                    SalePrice = 39.99M,
                    ProductArtUrl = "falcon_w_k_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Fabrikam Falcons\",  \"Size\" : \"Small\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"White\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 9,
                    LeadTime = 0,
                    RecommendationId = 23
                },
                new Product
                {
                    SkuNumber = "KID-0006",
                    Title = "Kid's Falcons Jersey - Yellow",
                    CategoryId = categoriesMap["Kids"],
                    Price = 39.99M,
                    SalePrice = 39.99M,
                    ProductArtUrl = "falcon_y_k_tee.png",
                    ProductDetails = "{ \"Brand\" : \"Contoso Sports\", \"Team\" : \"Fabrikam Falcons\",  \"Size\" : \"Small\", \"Material\" : \"Cotton/nylon\", \"Color\" : \"Yellow\", \"Fit\" : \"Slim\" }",
                    Description = "This ultra light sports jersey effectively wicks moisture away from the skin to keep you cool and performing at your best.",
                    Inventory = 9,
                    LeadTime = 0,
                    RecommendationId = 24
                },
                new Product
                {
                    SkuNumber = "GEA-0001",
                    Title = "Practice Ball",
                    CategoryId = categoriesMap["Balls"],
                    Price = 29.99M,
                    SalePrice = 29.99M,
                    ProductArtUrl = "practice_ball.jpg",
                    ProductDetails = "{ \"Size\" : \"5\",  \"Pattern\" : \"Swirl\", \"Material\" : \"Synthetic\" } ",
                    Description = "Entry level training ball. Regulation size, all weather/season.",
                    Inventory = 8,
                    LeadTime = 0,
                    RecommendationId = 25
                },
                new Product
                {
                    SkuNumber = "GEA-0002",
                    Title = "Match Ball",
                    CategoryId = categoriesMap["Balls"],
                    Price = 69.99M,
                    SalePrice = 69.99M,
                    ProductArtUrl = "match_ball.jpg",
                    ProductDetails = "{ \"Size\" : \"5\",  \"Pattern\" : \"Swirl\", \"Material\" : \"Leather\" } ",
                    Description = "High quality leather match ball. Regulation size, all weather/season.",
                    Inventory = 8,
                    LeadTime = 0,
                    RecommendationId = 26
                },
                new Product
                {
                    SkuNumber = "GEA-0003",
                    Title = "Contoso Cubs Ball",
                    CategoryId = categoriesMap["Balls"],
                    Price = 49.99M,
                    SalePrice = 49.99M,
                    ProductArtUrl = "cubs_ball.jpg",
                    ProductDetails = "{ \"Size\" : \"5\",  \"Pattern\" : \"Swirl\", \"Material\" : \"Synthetic\" } ",
                    Description = "Cubs supporter's ball. Regulation size, all weather/season.",
                    Inventory = 8,
                    LeadTime = 0,
                    RecommendationId = 27
                },
                new Product
                {
                    SkuNumber = "GEA-0004",
                    Title = "Fabrikam Falcons Ball",
                    CategoryId = categoriesMap["Balls"],
                    Price = 39.99M,
                    SalePrice = 39.99M,
                    ProductArtUrl = "falcon_ball.jpg",
                    ProductDetails = "{ \"Size\" : \"5\",  \"Pattern\" : \"Swirl\", \"Material\" : \"Synthetic\" } ",
                    Description = "Falcons supporter's ball. Regulation size, all weather/season.",
                    Inventory = 8,
                    LeadTime = 0,
                    RecommendationId = 28
                }
            };
            context.Products.AddOrUpdate(x => x.Title, products.ToArray());
            context.SaveChanges();

            var stores = Enumerable.Range(1, 20).Select(id => new Store { StoreId = id, Name = string.Format(CultureInfo.InvariantCulture, "Store{0}", id) }).ToList();
            context.Stores.AddOrUpdate(x => x.Name, stores.ToArray());
            context.SaveChanges();

            var rainchecks = GetRainchecks(stores, products);
            context.RainChecks.AddOrUpdate(x => x.StoreId, rainchecks.ToArray());
            context.SaveChanges();

            var orders = new List<Order>
            {
                new Order
                {
                    Name = "Order1",
                    Username = "Admin",
                    OrderDate = DateTime.Now,
                    OrderStatus = "Processing",
                    Total = 123,
                    Email = "admin@contososla.com",
                    Address = "123 Fake St",
                    City = "Brooklyn",
                    Country = "USA",
                    Phone = "1111111111",
                    PostalCode = "11111",
                    State = "New York",
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail
                        {
                            Product = context.Products.ToList()[0],
                            Quantity = 1,
                            UnitPrice = 10
                        }
                    }
                },
                new Order
                {
                    Name = "Order2",
                    Username = "Admin",
                    OrderDate = DateTime.Now,
                    OrderStatus = "Completed",
                    Total = 234,
                    Email = "admin@contososla.com",
                    Address = "234 Fake St",
                    City = "Brooklyn",
                    Country = "USA",
                    Phone = "2222222222",
                    PostalCode = "22222",
                    State = "New York",
                    OrderDetails = new List<OrderDetail>
                    {
                        new OrderDetail
                        {
                            Product = context.Products.ToList()[1],
                            Quantity = 2,
                            UnitPrice = 20
                        },
                        new OrderDetail
                        {
                            Product = context.Products.ToList()[2],
                            Quantity = 19,
                            UnitPrice = 2
                        },
                        new OrderDetail
                        {
                            Product = context.Products.ToList()[3],
                            Quantity = 3,
                            UnitPrice = 202
                        }
                    }
                }
            };
            context.Orders.AddOrUpdate(x => x.Name, orders.ToArray());
            context.SaveChanges();
        }

        /// <summary>
        /// Generate an enumeration of rainchecks.  The random number generator uses a seed to ensure 
        /// that the sequence is consistent, but provides somewhat random looking data.
        /// </summary>
        public static IEnumerable<Raincheck> GetRainchecks(IList<Store> stores, IList<Product> products)
        {
            var random = new Random(1234);

            foreach (var store in stores)
            {
                for (var i = 0; i < random.Next(1, 5); i++)
                {
                    yield return new Raincheck
                    {
                        StoreId = store.StoreId,
                        Name = string.Format("John Smith{0}", random.Next()),
                        Quantity = random.Next(1, 10),
                        ProductId = products[random.Next(0, products.Count)].ProductId,
                        SalePrice = Math.Round(100 * random.NextDouble(), 2)
                    };
                }
            }
        }
    }
}