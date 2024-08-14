﻿using System.Data.SqlClient;

public class Program
{
    public static void Main(string[] args)
    {
        bool inLoop = true;
        while (inLoop)
        {
            switch (MainMenu())
            {
                case "1":
                    CreateNewRecord();
                    break;
                case "2":
                    ViewTable();
                    break;
                case "3":
                    Update();
                    break;
                case "4":
                    Console.WriteLine("Delete a record");
                    break;
                case "5":
                    inLoop = false;
                    break;
                default:
                    Console.WriteLine("Please enter valid input");
                    break;
            }
        }
        Console.WriteLine("You have exited the application");

    }

    public static string MainMenu()
    {
        Console.Clear();
        Console.WriteLine("*****************************************");
        Console.WriteLine("Welcome to the Actors and Movies Database");
        Console.WriteLine("1. Create a new record");
        Console.WriteLine("2. View records");
        Console.WriteLine("3. Update a record");
        Console.WriteLine("4. Delete a record");
        Console.WriteLine("5. Exit application");
        Console.WriteLine("*****************************************");

        string input = Console.ReadLine();
        return input;

    }


    public static string InternalMenu()
    {
        
        Console.WriteLine("*****************************************");
        Console.WriteLine("Choose the table to view/edit");
        Console.WriteLine("1. Movies table");
        Console.WriteLine("2. Actors table");
        Console.WriteLine("3. Exit");
        Console.WriteLine("*****************************************");
        string? input = Console.ReadLine();

        return input;
    }

    public static void ViewTable()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "localhost,1433";
        builder.InitialCatalog = "MoviesAndActorsDB";
        builder.TrustServerCertificate = true;
        builder.UserID = "SA";
        builder.Password = "Qwerty123!";

        string connectionString = builder.ConnectionString;

        SqlConnection conn = new SqlConnection(connectionString);

        

        bool checker = true;

        while (checker)
        {
            switch (InternalMenu())
            {
                case "1":
                    string sql1 = "Select * from movietable";
                    conn.Open();

                    var cmd = new SqlCommand(sql1, conn);

                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader["movie_title"]} || {reader["movie_genre"]} || {reader["movie_release_year"]} || {reader["movie_rating"]}");

                    }

                    reader.Close();
                    conn.Close();

                    break;
                case "2":
                    string sql2 = "Select * from actortable";
                    conn.Open();


                    var cmd2 = new SqlCommand(sql2, conn);

                    var reader2 = cmd2.ExecuteReader();

                    while (reader2.Read())
                    {
                        Console.WriteLine($"{reader2["actor_name"]} || {reader2["actor_gender"]}");

                    }

                    reader2.Close();
                    conn.Close();
                    break;

                    case "3":
                        checker = false;
                    break;
                default:
                    Console.WriteLine("Please enter valid input");
                    break;


            }
        }
    }

    public static async void CreateNewRecord()
    {
        bool checker = true;

        while (checker)
        {
            switch (InternalMenu())
            {
                case "1":
                    Console.WriteLine("You picked the movies table");
                    CreateNewMovie();
                    break;
                case "2": 
                    Console.WriteLine("You picked the actors table");
                    CreateNewActor();
                    break;
                case "3":
                    checker = false;
                    Console.WriteLine("You have exited");
                    break;
                default: 
                    Console.WriteLine("Please enter valid input");
                    break;
                    

            }
        }
    }

    public static void CreateNewActor()
    {
        int actorID;
        string actorName;
        string actorGender;
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter actor name");
        Console.WriteLine("*****************************************");
        actorName = Console.ReadLine();
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter actor gender");
        Console.WriteLine("*****************************************");
        actorGender = Console.ReadLine();

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "localhost,1433";
        builder.InitialCatalog = "MoviesAndActorsDB";
        builder.TrustServerCertificate = true;
        builder.UserID = "SA";
        builder.Password = "Qwerty123!";

        string connectionString = builder.ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string sqlGetMaxId = "SELECT ISNULL(MAX(actor_id), 0) FROM actortable";
            using (SqlCommand cmdGetMaxId = new SqlCommand(sqlGetMaxId, conn))
            {
                object result = cmdGetMaxId.ExecuteScalar();
                actorID = Convert.ToInt32(result) + 1;
            }
            string sqlInsert = "INSERT INTO actortable(actor_id,actor_name,actor_gender)" +
           "VALUES (@ActorID, @ActorName, @ActorGender)";


            using (SqlCommand insert = new SqlCommand(sqlInsert, conn))
            {
                insert.Parameters.AddWithValue("@ActorID", actorID);
                insert.Parameters.AddWithValue("@ActorName", actorName);
                insert.Parameters.AddWithValue("@ActorGender", actorGender);

                int rowsAffected = insert.ExecuteNonQuery();
                Console.WriteLine("*****************************************");
                Console.WriteLine($"{rowsAffected} record(s) inserted.");
                Console.WriteLine("*****************************************");
                conn.Close();
            }
        }
        Console.WriteLine("*****************************************");
        Console.WriteLine($"Added {actorName} to the actortable!");
        Console.WriteLine("*****************************************");
    }

    public static void CreateNewMovie()
    {
        
        int movieID;
        string movieTitle;
        string movieGenre;
        int movieReleaseYear;
        double movieRating;
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter movie name:");
        Console.WriteLine("*****************************************");
        movieTitle = Console.ReadLine();
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter movie genre:");
        Console.WriteLine("*****************************************");
        movieGenre = Console.ReadLine();
        Console.WriteLine("*****************************************");
        Console.WriteLine($"Enter {movieTitle}'s release year");
        Console.WriteLine("*****************************************");
        if (!int.TryParse(Console.ReadLine(), out movieReleaseYear))
        {
            Console.WriteLine("Please enter a valid number");
            return;
        }
       
        Console.WriteLine($"Enter {movieTitle}'s movie rating");
        if (!double.TryParse(Console.ReadLine(), out movieRating))
        {
            Console.WriteLine("Please enter a valid number");
            return;
        }

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "localhost,1433";
        builder.InitialCatalog = "MoviesAndActorsDB";
        builder.TrustServerCertificate = true;
        builder.UserID = "SA";
        builder.Password = "Qwerty123!";

        string connectionString = builder.ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string sqlGetMaxId = "SELECT ISNULL(MAX(movie_id), 0) FROM movietable";
            using (SqlCommand cmdGetMaxId = new SqlCommand(sqlGetMaxId, conn))
            {
                object result = cmdGetMaxId.ExecuteScalar();
                movieID = Convert.ToInt32(result) + 1;
            }
            string sqlInsert = "INSERT INTO movietable(movie_id,movie_title,movie_genre,movie_release_year,movie_rating)" +
           "VALUES (@MovieID, @Title, @Genre, @ReleaseYear, @Rating)";


            using (SqlCommand insert = new SqlCommand(sqlInsert, conn))
            {
                insert.Parameters.AddWithValue("@MovieID", movieID);
                insert.Parameters.AddWithValue("@Title", movieTitle);
                insert.Parameters.AddWithValue("@Genre", movieGenre);
                insert.Parameters.AddWithValue("@ReleaseYear", movieReleaseYear);
                insert.Parameters.AddWithValue("@Rating", movieRating);

                int rowsAffected = insert.ExecuteNonQuery();
                Console.WriteLine($"{rowsAffected} record(s) inserted.");
                conn.Close();
            }

        }
        Console.WriteLine("*****************************************");
        Console.WriteLine($"Added {movieTitle} to movietable!");
        Console.WriteLine("*****************************************");


    }

    public static void Update()
    {
        bool checker = true;

        while(checker){
            switch (InternalMenu())
            {
                case "1":
                    //Console.WriteLine("Movies");
                    UpdateMovie();
                    break;
                case "2":
                   //Console.WriteLine("Actors");
                    UpdateActor();
                    break;
                case "3":
                    Console.WriteLine("Exit");
                    checker = false;
                    break;
                default:
                    Console.WriteLine("Enter valid input");
                    break;
            }
        }
        
    }

    public static void UpdateMovie()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = "localhost,1433";
        builder.InitialCatalog = "MoviesAndActorsDB";
        builder.TrustServerCertificate = true;
        builder.UserID = "SA";
        builder.Password = "Qwerty123!";

        string connectionString = builder.ConnectionString;

        SqlConnection conn = new SqlConnection(connectionString);

        string sql1 = "Select * from movietable";
        conn.Open();

        var cmd = new SqlCommand(sql1, conn);

        var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            Console.WriteLine($"{reader["movie_id"]} || {reader["movie_title"]} || {reader["movie_genre"]} || {reader["movie_release_year"]} || {reader["movie_rating"]}");

        }

        reader.Close();
        conn.Close();

        int movieID;
        string movieTitle;
        string movieGenre;
        int movieReleaseYear;
        double movieRating;
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter the ID of the movie you want to update:");
        Console.WriteLine("*****************************************");
        if (!int.TryParse(Console.ReadLine(), out movieID))
        {
            Console.WriteLine("Please enter a valid number for the movie ID.");
            return; 
        }
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter new movie title (leave blank to keep current title):");
        Console.WriteLine("*****************************************");
        movieTitle = Console.ReadLine();
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter new movie genre (leave blank to keep current genre):");
        Console.WriteLine("*****************************************");
        movieGenre = Console.ReadLine();
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter new release year (leave blank to keep current release year):");
        Console.WriteLine("*****************************************");
        string releaseYearInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(releaseYearInput) && !int.TryParse(releaseYearInput, out movieReleaseYear))
        {
            Console.WriteLine("Please enter a valid number for the release year.");
            return; 
        }
        Console.WriteLine("*****************************************");
        Console.WriteLine("Enter new movie rating (leave blank to keep current rating):");
        Console.WriteLine("*****************************************");
        string ratingInput = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(ratingInput) && !double.TryParse(ratingInput, out movieRating))
        {
            Console.WriteLine("Please enter a valid number for the movie rating.");
            return; 
        }

      

        using (SqlConnection conn2 = new SqlConnection(connectionString))
        {
            conn2.Open();

            
            List<string> updates = new List<string>();

            if (!string.IsNullOrWhiteSpace(movieTitle))
            {
                updates.Add("movie_title = @Title");
            }
            if (!string.IsNullOrWhiteSpace(movieGenre))
            {
                updates.Add("movie_genre = @Genre");
            }
            if (!string.IsNullOrWhiteSpace(releaseYearInput))
            {
                updates.Add("movie_release_year = @ReleaseYear");
            }
            if (!string.IsNullOrWhiteSpace(ratingInput))
            {
                updates.Add("movie_rating = @Rating");
            }

            if (updates.Count == 0)
            {
                Console.WriteLine("No fields to update.");
                return;
            }

            string sqlUpdate = $"UPDATE movietable SET {string.Join(", ", updates)} WHERE movie_id = @MovieID";

            using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn2))
            {
                
                cmdUpdate.Parameters.AddWithValue("@MovieID", movieID);

                if (!string.IsNullOrWhiteSpace(movieTitle))
                {
                    cmdUpdate.Parameters.AddWithValue("@Title", movieTitle);
                }
                if (!string.IsNullOrWhiteSpace(movieGenre))
                {
                    cmdUpdate.Parameters.AddWithValue("@Genre", movieGenre);
                }
                if (!string.IsNullOrWhiteSpace(releaseYearInput))
                {
                    cmdUpdate.Parameters.AddWithValue("@ReleaseYear", releaseYearInput);
                }
                if (!string.IsNullOrWhiteSpace(ratingInput))
                {
                    cmdUpdate.Parameters.AddWithValue("@Rating", ratingInput);
                }

                
                int rowsAffected = cmdUpdate.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Movie updated successfully.");
                }
                else
                {
                    Console.WriteLine("No movie found with the given ID.");
                }


            }
            conn2.Close();
        }
        
    }

    public static void UpdateActor()
    {

    }
}
