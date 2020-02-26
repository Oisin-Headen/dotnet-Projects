using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABCCC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ABCCC.Data
{
    public class DbInitializer
    {
        private const string UserName = "admin@ABCCC.com";
        private const string Password = "Im_th3_Admin";

        public static async void Initialize(ABCCCDataContext context, IServiceProvider serviceProvider)
        {
            context.Database.EnsureCreated();

            if (context.AllMovies.Any())
            {
                return;
            }

            // This block creates the admin user, and adds the "Admin" role to it.
            var userManager = (UserManager<ApplicationUser>)serviceProvider.GetService(typeof(UserManager<ApplicationUser>));
            var roleManager = (RoleManager<IdentityRole>)serviceProvider.GetService(typeof(RoleManager<IdentityRole>));
            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new ApplicationUser { UserName = UserName };
                var userresult = await userManager.CreateAsync(user, Password);
            }
            var role = await roleManager.FindByNameAsync("Admin");
            if (role == null)
            {
                role = new IdentityRole { Name= "Admin" };
                var roleresult = await roleManager.CreateAsync(role);
            }
            await userManager.AddToRoleAsync(user, "Admin");




            var Cineplexes = new Cineplex[]
            {
                new Cineplex {Location="St Kilda", ShortDescription ="Come down to St Kilda, for a day at the movies!",
                    LongDescription = "Our St Kilda branch has a wide selection of movies and snacks. Come and visit us!",
                    ImageUrl ="~/Images/StKilda.png"},
                new Cineplex {Location="Fitzroy", ShortDescription="Join us in Fitzroy, and try our excellenct snack bar.",
                    LongDescription ="The snack bar at Fitzroy Cineplex is the stuff of legend. Be sure to grab a sample on the way to your movie.",
                    ImageUrl ="~/Images/Fitzroy.png"},
                new Cineplex {Location="Melbourne CBD", ShortDescription="Find us in the centre of Melbourne, with a great taste in movies.",
                    LongDescription ="The most luxurious Cineplex is based in the city centre. Come and enjoy!",
                    ImageUrl ="~/Images/MelbourneCBD.png"},
                new Cineplex {Location="Sunshine", ShortDescription="Come west to Sunshine, for our renowned cinema.",
                    LongDescription ="With an amazing screen and great snacks, Sunshine Cineplex is the place to be.",
                    ImageUrl ="~/Images/Sunshine.png"},
                new Cineplex {Location="Lilydale", ShortDescription="Travel to Lilydale for our epic Movies.",
                    LongDescription ="At Lilydale Cineplex, we show amazing movies, including some late night marathons.",
                    ImageUrl ="~/Images/Lilydale.png"}
            };
            foreach (var item in Cineplexes)
            {
                context.Cineplex.Add(item);
            }
            context.SaveChanges();

            var Movies = new AbstractMovie[]
            {
                new Movie {Title="The Matrix", ShortDescription="A computer hacker learns from mysterious rebels about the true nature "+
                "of his reality and his role in the war against its controllers.",
                    LongDescription ="Thomas A. Anderson is a man living two lives. By day he is an average computer programmer and by night a "+
                    "hacker known as Neo. Neo has always questioned his reality, but the truth is far beyond his imagination. Neo finds himself"+
                    " targeted by the police when he is contacted by Morpheus, a legendary computer hacker branded a terrorist by the government."+
                    " Morpheus awakens Neo to the real world, a ravaged wasteland where most of humanity have been captured by a race of machines "+
                    "that live off of the humans' body heat and electrochemical energy and who imprison their minds within an artificial reality known"+
                    " as the Matrix. As a rebel against the machines, Neo must return to the Matrix and confront the agents: super-powerful computer programs"+
                    " devoted to snuffing out Neo and the entire human rebellion.",
                    ImageUrl ="~/images/TheMatrix.jpg"},

                new Movie {Title="The Matrix Reloaded", ShortDescription="Neo and the rebel leaders estimate that they have 72 hours until 250,000 probes discover"+
                " Zion and destroy it and its inhabitants. During this, Neo must decide how he can save Trinity from a dark fate in his dreams.",
                    LongDescription ="Six months after the events depicted in The Matrix, Neo has proved to be a good omen for the free humans, as more and"+
                    " more humans are being freed from the matrix and brought to Zion, the one and only stronghold of the Resistance. Neo himself has discovered "+
                    "his superpowers including super speed, ability to see the codes of the things inside the matrix, and a certain degree of precognition. But a"+
                    " nasty piece of news hits the human resistance: 250,000 machine sentinels are digging to Zion and would reach them in 72 hours. As Zion"+
                    " prepares for the ultimate war, Neo, Morpheus and Trinity are advised by the Oracle to find the Keymaker who would help them reach the"+
                    " Source. Meanwhile Neo's recurrent dreams depicting Trinity's death have got him worried and as if it was not enough, Agent Smith has"+
                    " somehow escaped deletion, has become more powerful than before and has chosen Neo as his next target.",
                    ImageUrl ="~/images/TheMatrixReloaded.jpg"},

                new Movie {Title="The Matrix Revolution", ShortDescription="The human city of Zion defends itself against the massive invasion of the"+
                " machines as Neo fights to end the war at another front while also opposing the rogue Agent Smith.",
                    LongDescription ="Neo discovers that somehow he is able to use his powers in the real world too and that his mind can be freed from his body,"+
                    " as a result of which he finds himself trapped on a train station between the Matrix and the Real World. Meanwhile, Zion is preparing for the"+
                    " oncoming war with the machines with very little chances of survival. Neo's associates set out to free him from The Merovingian since it's"+
                    " believed that he is the One who will end the war between humans and the machines. What they do not know is that there is a threat from a"+
                    " third party, someone who has plans to destroy both the worlds.",
                    ImageUrl ="~/images/TheMatrixRevolution.jpg"},

                new Movie {Title="Spiderman: Homecoming", ShortDescription="Peter Parker, with the help of his mentor Tony Stark, tries to balance his life as "+
                "an ordinary high school student in New York City while fighting crime as his superhero alter ego Spider-Man when a new threat emerges.",
                    LongDescription ="Thrilled by his experience with the Avengers, Peter returns home, where he lives with his Aunt May, under the watchful eye "+
                    "of his new mentor Tony Stark, Peter tries to fall back into his normal daily routine - distracted by thoughts of proving himself to be more than"+
                    " just your friendly neighborhood Spider-Man - but when the Vulture emerges as a new villain, everything that Peter holds most important will"+
                    " be threatened.",
                    ImageUrl ="~/images/Spiderman.jpg"},
                 new Movie {Title="Transformers: The Last Knight", ShortDescription="Autobots and Decepticons are at war, with humans on the sidelines. "+
                 "Optimus Prime is gone. The key to saving our future lies buried in the secrets of the past, in the hidden history of Transformers on Earth.",
                     LongDescription ="Optimus Prime finds his dead home planet, Cybertron, in which he comes to find he was responsible for its destruction. "+
                     "He finds a way to bring Cybertron back to life, but in order to do so, Optimus needs to find an artifact that is on Earth.",
                    ImageUrl ="~/images/Transformers.jpg"},

                new MovieComingSoon {Title="Black Panther", ShortDescription="T'Challa, after the death of his father, the King of Wakanda,"+
                " returns home to the isolated, technologically advanced African nation to succeed to the throne and take his rightful place as king.",
                    LongDescription ="After the events of Captain America: Civil War, King T'Challa returns home to the reclusive, technologically "+
                    "advanced African nation of Wakanda to serve as his country's new leader. However, T'Challa soon finds that he is challenged for"+
                    " the throne from factions within his own country. When two foes conspire to destroy Wakanda, the hero known as Black Panther "+
                    "must team up with C.I.A. agent Everett K. Ross and members of the Dora Milaje, Wakanadan special forces, to prevent Wakanda from"+
                    " being dragged into a world war.",
                    ImageUrl ="~/images/BlackPanther.jpg", ReleaseDate="16/02/2018"},
                new MovieComingSoon {Title="Thor: Ragnarok", ShortDescription="Imprisoned, the mighty Thor finds himself in a lethal gladiatorial contest "+
                "against the Hulk, his former ally. Thor must fight for survival and race against time to prevent the all-powerful Hela from destroying his "+
                "home and the Asgardian civilization.",
                    LongDescription ="Thor is imprisoned on the other side of the universe without his mighty hammer and finds himself in a race against time"+
                    " to get back to Asgard to stop Ragnarok, the destruction of his homeworld and the end of Asgardian civilization, at the hands of an "+
                    "all-powerful new threat, the ruthless Hela. But first he must survive a deadly gladiatorial contest that pits him against his former ally "+
                    "and fellow Avenger the Incredible Hulk!",
                    ImageUrl ="~/images/Thor.jpg", ReleaseDate="3/09/2017"},
                new MovieComingSoon {Title="Justice League", ShortDescription="Fueled by his restored faith in humanity and inspired by Superman's "+
                "selfless act, Bruce Wayne enlists the help of his newfound ally, Diana Prince, to face an even greater enemy.",
                    LongDescription ="Fueled by his restored faith in humanity and inspired by Superman's selfless act, Bruce Wayne enlists the help of"+
                    " his newfound ally, Diana Prince, to face an even greater enemy. Together, Batman and Wonder Woman work quickly to find and recruit "+
                    "a team of metahumans to stand against this newly awakened threat. But despite the formation of this unprecedented league of "+
                    "heroes-Batman, Wonder Woman, Aquaman, Cyborg and The Flash-it may already be too late to save the planet from an assault of "+
                    "catastrophic proportions.",
                    ImageUrl ="~/images/Justice.jpg", ReleaseDate="17/11/2017"},

                new MovieComingSoon {Title="Star Wars: The Last Jedi", ShortDescription="Having taken her first steps into a larger world in Star Wars:"+
                " The Force Awakens (2015), Rey continues her epic journey with Finn, Poe and Luke Skywalker in the next chapter of the saga.",
                    LongDescription ="Having taken her first steps into a larger world in Star Wars: The Force Awakens (2015), Rey continues her epic "+
                    "journey with Finn, Poe and Luke Skywalker in the next chapter of the saga.",
                    ImageUrl ="~/images/StarWars.jpg", ReleaseDate="15/12/2017"},

                new MovieComingSoon {Title="The LEGO Ninjago Movie", ShortDescription="Six young ninjas Lloyd, Jay, Kai, Cole, Zane and Nya are tasked"+
                " with defending their island home, called Ninjago.",
                    LongDescription ="Six young ninjas Lloyd, Jay, Kai, Cole, Zane and Nya are tasked with defending their island home, called Ninjago. "+
                    "By night, they're gifted warriors, using their skills and awesome fleet of vehicles to fight villains and monsters. By day, they're"+
                    " ordinary teens struggling against their greatest enemy: high school. ",
                    ImageUrl ="~/images/Lego.jpg", ReleaseDate="21/09/2017"}
            };
            //Movie descriptions and images from :http://www.imdb.com/?ref_=nv_home
            foreach (var item in Movies)
            {
                context.AllMovies.Add(item);
            }
            context.SaveChanges();

            var CineplexMovies = new CineplexMovie[]
            {
                // Movies for St Kilda
                new CineplexMovie {CineplexId=1, MovieId=1, Day=CineplexMovie.DayOfWeek.Monday, Hour=8, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=1, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=8, Period=CineplexMovie.TimePeriod.am  },
                new CineplexMovie {CineplexId=1, MovieId=1, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=8, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=1, Day=CineplexMovie.DayOfWeek.Thursday, Hour=8, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=1, Day=CineplexMovie.DayOfWeek.Friday, Hour=8, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=1, Day=CineplexMovie.DayOfWeek.Saturday, Hour=8, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=1, Day=CineplexMovie.DayOfWeek.Sunday, Hour=8, Period=CineplexMovie.TimePeriod.am },

                new CineplexMovie {CineplexId=1, MovieId=2, Day=CineplexMovie.DayOfWeek.Monday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=2, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=2, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=2, Day=CineplexMovie.DayOfWeek.Thursday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=2, Day=CineplexMovie.DayOfWeek.Friday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=2, Day=CineplexMovie.DayOfWeek.Saturday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=1, MovieId=2, Day=CineplexMovie.DayOfWeek.Sunday, Hour=11, Period=CineplexMovie.TimePeriod.am },

                new CineplexMovie {CineplexId=1, MovieId=3, Day=CineplexMovie.DayOfWeek.Monday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=3, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=3, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=3, Day=CineplexMovie.DayOfWeek.Thursday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=3, Day=CineplexMovie.DayOfWeek.Friday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=3, Day=CineplexMovie.DayOfWeek.Saturday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=3, Day=CineplexMovie.DayOfWeek.Sunday, Hour=2, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=1, MovieId=4, Day=CineplexMovie.DayOfWeek.Monday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=4, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=4, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=4, Day=CineplexMovie.DayOfWeek.Thursday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=4, Day=CineplexMovie.DayOfWeek.Friday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=4, Day=CineplexMovie.DayOfWeek.Saturday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=4, Day=CineplexMovie.DayOfWeek.Sunday, Hour=5, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=1, MovieId=5, Day=CineplexMovie.DayOfWeek.Monday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=5, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=5, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=5, Day=CineplexMovie.DayOfWeek.Thursday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=5, Day=CineplexMovie.DayOfWeek.Friday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=5, Day=CineplexMovie.DayOfWeek.Saturday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=1, MovieId=5, Day=CineplexMovie.DayOfWeek.Sunday, Hour=8, Period=CineplexMovie.TimePeriod.pm },

                // Movies for Fitzroy
                new CineplexMovie {CineplexId=2, MovieId=1, Day=CineplexMovie.DayOfWeek.Monday, Hour=7, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=1, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=7, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=1, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=7, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=1, Day=CineplexMovie.DayOfWeek.Thursday, Hour=7, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=1, Day=CineplexMovie.DayOfWeek.Friday, Hour=7, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=1, Day=CineplexMovie.DayOfWeek.Saturday, Hour=7, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=1, Day=CineplexMovie.DayOfWeek.Sunday, Hour=7, Period=CineplexMovie.TimePeriod.am },

                new CineplexMovie {CineplexId=2, MovieId=2, Day=CineplexMovie.DayOfWeek.Monday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=2, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=2, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=2, Day=CineplexMovie.DayOfWeek.Thursday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=2, Day=CineplexMovie.DayOfWeek.Friday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=2, Day=CineplexMovie.DayOfWeek.Saturday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=2, MovieId=2, Day=CineplexMovie.DayOfWeek.Sunday, Hour=10, Period=CineplexMovie.TimePeriod.am },

                new CineplexMovie {CineplexId=2, MovieId=3, Day=CineplexMovie.DayOfWeek.Monday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=3, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=3, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=3, Day=CineplexMovie.DayOfWeek.Thursday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=3, Day=CineplexMovie.DayOfWeek.Friday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=3, Day=CineplexMovie.DayOfWeek.Saturday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=3, Day=CineplexMovie.DayOfWeek.Sunday, Hour=1, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=2, MovieId=4, Day=CineplexMovie.DayOfWeek.Monday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=4, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=4, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=4, Day=CineplexMovie.DayOfWeek.Thursday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=4, Day=CineplexMovie.DayOfWeek.Friday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=4, Day=CineplexMovie.DayOfWeek.Saturday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=4, Day=CineplexMovie.DayOfWeek.Sunday, Hour=4, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=2, MovieId=5, Day=CineplexMovie.DayOfWeek.Monday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=5, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=5, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=5, Day=CineplexMovie.DayOfWeek.Thursday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=5, Day=CineplexMovie.DayOfWeek.Friday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=5, Day=CineplexMovie.DayOfWeek.Saturday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=2, MovieId=5, Day=CineplexMovie.DayOfWeek.Sunday, Hour=7, Period=CineplexMovie.TimePeriod.pm },

                // Movies for Melbourne CBD
                new CineplexMovie {CineplexId=3, MovieId=1, Day=CineplexMovie.DayOfWeek.Monday, Hour=9, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=3, MovieId=1, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=9, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=3, MovieId=1, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=9, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=3, MovieId=1, Day=CineplexMovie.DayOfWeek.Thursday, Hour=9, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=3, MovieId=1, Day=CineplexMovie.DayOfWeek.Friday, Hour=9, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=3, MovieId=1, Day=CineplexMovie.DayOfWeek.Saturday, Hour=9, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=3, MovieId=1, Day=CineplexMovie.DayOfWeek.Sunday, Hour=9, Period=CineplexMovie.TimePeriod.am },

                new CineplexMovie {CineplexId=3, MovieId=2, Day=CineplexMovie.DayOfWeek.Monday, Hour=12, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=2, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=12, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=2, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=12, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=2, Day=CineplexMovie.DayOfWeek.Thursday, Hour=12, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=2, Day=CineplexMovie.DayOfWeek.Friday, Hour=12, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=2, Day=CineplexMovie.DayOfWeek.Saturday, Hour=12, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=2, Day=CineplexMovie.DayOfWeek.Sunday, Hour=12, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=3, MovieId=3, Day=CineplexMovie.DayOfWeek.Monday, Hour=3, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=3, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=3, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=3, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=3, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=3, Day=CineplexMovie.DayOfWeek.Thursday, Hour=3, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=3, Day=CineplexMovie.DayOfWeek.Friday, Hour=3, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=3, Day=CineplexMovie.DayOfWeek.Saturday, Hour=3, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=3, Day=CineplexMovie.DayOfWeek.Sunday, Hour=3, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=3, MovieId=4, Day=CineplexMovie.DayOfWeek.Monday, Hour=6, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=4, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=6, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=4, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=6, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=4, Day=CineplexMovie.DayOfWeek.Thursday, Hour=6, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=4, Day=CineplexMovie.DayOfWeek.Friday, Hour=6, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=4, Day=CineplexMovie.DayOfWeek.Saturday, Hour=6, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=4, Day=CineplexMovie.DayOfWeek.Sunday, Hour=6, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=3, MovieId=5, Day=CineplexMovie.DayOfWeek.Monday, Hour=9, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=5, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=9, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=5, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=9, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=5, Day=CineplexMovie.DayOfWeek.Thursday, Hour=9, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=5, Day=CineplexMovie.DayOfWeek.Friday, Hour=9, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=5, Day=CineplexMovie.DayOfWeek.Saturday, Hour=9, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=3, MovieId=5, Day=CineplexMovie.DayOfWeek.Sunday, Hour=9, Period=CineplexMovie.TimePeriod.pm },

                // Movies for Sunshine
                new CineplexMovie {CineplexId=4, MovieId=1, Day=CineplexMovie.DayOfWeek.Monday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=4, MovieId=1, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=4, MovieId=1, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=4, MovieId=1, Day=CineplexMovie.DayOfWeek.Thursday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=4, MovieId=1, Day=CineplexMovie.DayOfWeek.Friday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=4, MovieId=1, Day=CineplexMovie.DayOfWeek.Saturday, Hour=10, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=4, MovieId=1, Day=CineplexMovie.DayOfWeek.Sunday, Hour=10, Period=CineplexMovie.TimePeriod.am },

                new CineplexMovie {CineplexId=4, MovieId=2, Day=CineplexMovie.DayOfWeek.Monday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=2, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=2, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=2, Day=CineplexMovie.DayOfWeek.Thursday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=2, Day=CineplexMovie.DayOfWeek.Friday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=2, Day=CineplexMovie.DayOfWeek.Saturday, Hour=1, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=2, Day=CineplexMovie.DayOfWeek.Sunday, Hour=1, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=4, MovieId=3, Day=CineplexMovie.DayOfWeek.Monday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=3, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=3, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=3, Day=CineplexMovie.DayOfWeek.Thursday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=3, Day=CineplexMovie.DayOfWeek.Friday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=3, Day=CineplexMovie.DayOfWeek.Saturday, Hour=4, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=3, Day=CineplexMovie.DayOfWeek.Sunday, Hour=4, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=4, MovieId=4, Day=CineplexMovie.DayOfWeek.Monday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=4, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=4, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=4, Day=CineplexMovie.DayOfWeek.Thursday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=4, Day=CineplexMovie.DayOfWeek.Friday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=4, Day=CineplexMovie.DayOfWeek.Saturday, Hour=7, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=4, Day=CineplexMovie.DayOfWeek.Sunday, Hour=7, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=4, MovieId=5, Day=CineplexMovie.DayOfWeek.Monday, Hour=10, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=5, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=10, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=5, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=10, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=5, Day=CineplexMovie.DayOfWeek.Thursday, Hour=10, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=5, Day=CineplexMovie.DayOfWeek.Friday, Hour=10, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=5, Day=CineplexMovie.DayOfWeek.Saturday, Hour=10, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=4, MovieId=5, Day=CineplexMovie.DayOfWeek.Sunday, Hour=10, Period=CineplexMovie.TimePeriod.pm },

                // Movies for Lilydale
                new CineplexMovie {CineplexId=5, MovieId=1, Day=CineplexMovie.DayOfWeek.Monday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=1, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=1, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=1, Day=CineplexMovie.DayOfWeek.Thursday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=1, Day=CineplexMovie.DayOfWeek.Friday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=1, Day=CineplexMovie.DayOfWeek.Saturday, Hour=11, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=1, Day=CineplexMovie.DayOfWeek.Sunday, Hour=11, Period=CineplexMovie.TimePeriod.am },

                new CineplexMovie {CineplexId=5, MovieId=2, Day=CineplexMovie.DayOfWeek.Monday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=2, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=2, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=2, Day=CineplexMovie.DayOfWeek.Thursday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=2, Day=CineplexMovie.DayOfWeek.Friday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=2, Day=CineplexMovie.DayOfWeek.Saturday, Hour=2, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=2, Day=CineplexMovie.DayOfWeek.Sunday, Hour=2, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=5, MovieId=3, Day=CineplexMovie.DayOfWeek.Monday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=3, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=3, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=3, Day=CineplexMovie.DayOfWeek.Thursday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=3, Day=CineplexMovie.DayOfWeek.Friday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=3, Day=CineplexMovie.DayOfWeek.Saturday, Hour=5, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=3, Day=CineplexMovie.DayOfWeek.Sunday, Hour=5, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=5, MovieId=4, Day=CineplexMovie.DayOfWeek.Monday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=4, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=4, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=4, Day=CineplexMovie.DayOfWeek.Thursday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=4, Day=CineplexMovie.DayOfWeek.Friday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=4, Day=CineplexMovie.DayOfWeek.Saturday, Hour=8, Period=CineplexMovie.TimePeriod.pm },
                new CineplexMovie {CineplexId=5, MovieId=4, Day=CineplexMovie.DayOfWeek.Sunday, Hour=8, Period=CineplexMovie.TimePeriod.pm },

                new CineplexMovie {CineplexId=5, MovieId=5, Day=CineplexMovie.DayOfWeek.Monday, Hour=6, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=5, Day=CineplexMovie.DayOfWeek.Tuesday, Hour=6, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=5, Day=CineplexMovie.DayOfWeek.Wednesday, Hour=6, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=5, Day=CineplexMovie.DayOfWeek.Thursday, Hour=6, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=5, Day=CineplexMovie.DayOfWeek.Friday, Hour=6, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=5, Day=CineplexMovie.DayOfWeek.Saturday, Hour=6, Period=CineplexMovie.TimePeriod.am },
                new CineplexMovie {CineplexId=5, MovieId=5, Day=CineplexMovie.DayOfWeek.Sunday, Hour=6, Period=CineplexMovie.TimePeriod.am }
            };
            foreach (var item in CineplexMovies)
            {
                context.CineplexMovie.Add(item);
            }
            context.SaveChanges();
        }
    }
}
