using System.Text.Json;
using System.Text.Json.Serialization;
using Google.Apis.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WSS.API.Infrastructure.Config;

public static class FirebaseAuthConfig
{
    public static void RegisterFireAuth(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                // Path of firebase.json
                var jsonFirebasePath = Path.Combine(currentDirectory, "firebase.json");
                var content = File.ReadAllText(jsonFirebasePath);

                var firebaseOptions = JsonSerializer.Deserialize<FirebaseOptions>(content);
                if (firebaseOptions is not null)
                {
                    firebaseOptions.Authority = $"https://securetoken.google.com/{firebaseOptions.ProjectId}";
                }

                opt.IncludeErrorDetails = true;
                opt.Authority = firebaseOptions?.Authority.ThrowIfNull(nameof(firebaseOptions.Authority));
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = firebaseOptions?.Authority.ThrowIfNull(nameof(firebaseOptions.Authority)),
                    ValidateAudience = true,
                    ValidAudience = firebaseOptions?.ProjectId.ThrowIfNull(nameof(firebaseOptions.ProjectId)),
                    ValidateLifetime = true
                };
            });
    }
}

public class FirebaseOptions
{
    [JsonPropertyName("project_id")] public string ProjectId { get; set; }

    [JsonPropertyName("auth_uri")] public string Authority { get; set; }
}