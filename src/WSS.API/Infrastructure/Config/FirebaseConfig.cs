using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

namespace WSS.API.Infrastructure.Config;

/// <summary>
///     FirebaseConfig class.
/// </summary>
public static class FirebaseConfig
{
    /// <summary>
    ///     Add Firebase to the project.
    /// </summary>
    public static void AddFireBaseAsync(this IServiceCollection serviceCollection)
    {
        // Get Current Path
        var currentDirectory = Directory.GetCurrentDirectory();
        // Path of firebase.json
        var jsonFirebasePath = Path.Combine(currentDirectory, "firebase.json");
        Console.WriteLine(jsonFirebasePath);
        // Initialize the default app
        var defaultApp = FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile(jsonFirebasePath)
        });

        var fbAuth = FirebaseAuth.GetAuth(defaultApp);
        // logger service
        var logger = serviceCollection.BuildServiceProvider().GetService<ILogger<FirebaseApp>>();
        logger!.Log(LogLevel.Information, "Firebase Initialized");

        // Add Firebase to the project
        serviceCollection.AddSingleton(defaultApp);
        serviceCollection.AddSingleton(fbAuth);
    }
}