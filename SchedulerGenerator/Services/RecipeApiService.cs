﻿using SchedulerGenerator.Models.ExternalApi;
using SchedulerGenerator.Models.ExternalApi.IGS;
using SchedulerGenerator.Services.Interfaces;

namespace SchedulerGenerator.Services
{

    public class RecipeApiService : IRecipeApiService
    {
        private readonly ILogger<RecipeApiService> logger;
        private readonly RecipeAPISettings settings;

        public const string _RecipeMethodUrl = "recipe";
        public RecipeApiService(ILogger<RecipeApiService> logger, RecipeAPISettings settings)
        {
            this.logger = logger;
            this.settings = settings;
        }

        public async Task<List<Recipe>> GetRecipesAsync()
        {
            try
            {
                var url = new UriBuilder(settings.UseHttps?"https":"http", settings.Host, settings.Port).Uri;
               
                var client = new HttpClient() { BaseAddress = url};
                var jsonOptions = new System.Text.Json.JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                    
                } ;
                var response = await client.GetFromJsonAsync<Recipes>(_RecipeMethodUrl, jsonOptions);
                
                return response.recipes;
            }
            catch (Exception ex)
            {
                logger.LogError("Recipe api request exception", ex);
                throw new Exception($"\nPlease recheck RecipeApi settings: {settings}"  , ex);
            }
        }
    }
}
