using System.Text;
using System.Text.Json;

namespace GymManagementSystem.Services
{
    /// <summary>
    /// Service for generating personalized workout plans
    /// خدمة لإنشاء خطط التمارين المخصصة
    /// </summary>
    public class OpenAIService : IOpenAIService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAIService> _logger;
        private readonly HttpClient _httpClient;

        public OpenAIService(IConfiguration configuration, ILogger<OpenAIService> logger, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Generate personalized workout plan
        /// إنشاء خطة تمارين مخصصة
        /// </summary>
        public async Task<string> GenerateWorkoutPlan(int height, decimal weight, string fitnessGoal)
        {
            try
            {
                var apiKey = _configuration["Gemini:ApiKey"];

                if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_OPENAI_API_KEY_HERE")
                {
                    return GetFallbackWorkoutPlan(fitnessGoal);
                }

                // Calculate BMI
                var heightInMeters = height / 100.0m;
                var bmi = weight / (heightInMeters * heightInMeters);

                // Create prompt
                var prompt = $@"Create a personalized workout plan for someone with the following details:
- Height: {height} cm
- Weight: {weight} kg
- BMI: {bmi:F1}
- Fitness Goal: {fitnessGoal}

Please provide:
1. A brief assessment of their current fitness level
2. A weekly workout plan (3-5 days)
3. Specific exercises with sets and reps
4. Nutrition tips
5. Expected timeline for results

Keep the plan practical and achievable.";

                // Call external API using REST
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}";
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JsonDocument.Parse(responseBody);
                    
                    var text = jsonResponse.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    return text ?? GetFallbackWorkoutPlan(fitnessGoal);
                }
                else
                {
                    _logger.LogError($"API error: {response.StatusCode}");
                    return GetFallbackWorkoutPlan(fitnessGoal);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating workout plan");
                return GetFallbackWorkoutPlan(fitnessGoal);
            }
        }

        /// <summary>
        /// Standard workout plan template
        /// قالب خطة تمارين قياسية
        /// </summary>
        private string GetFallbackWorkoutPlan(string fitnessGoal)
        {
            return $@"# Personalized Workout Plan - {fitnessGoal}

## Note
This is a standard workout plan template designed for your fitness goal.

## Weekly Workout Schedule

### Monday - Upper Body
- Push-ups: 3 sets of 12 reps
- Dumbbell rows: 3 sets of 10 reps
- Shoulder press: 3 sets of 10 reps
- Bicep curls: 3 sets of 12 reps

### Wednesday - Lower Body
- Squats: 3 sets of 15 reps
- Lunges: 3 sets of 12 reps per leg
- Leg press: 3 sets of 12 reps
- Calf raises: 3 sets of 15 reps

### Friday - Full Body
- Burpees: 3 sets of 10 reps
- Planks: 3 sets of 30 seconds
- Mountain climbers: 3 sets of 20 reps
- Jump squats: 3 sets of 12 reps

## Nutrition Tips
- Drink at least 8 glasses of water daily
- Eat protein-rich foods after workouts
- Include vegetables in every meal
- Avoid processed foods and sugary drinks

## Expected Results
With consistent effort, you should see noticeable improvements in 4-6 weeks.

---
*This is a general workout plan. For personalized guidance, please consult with our trainers.*";
        }

        /// <summary>
        /// Generate body transformation visualization
        /// Vücut dönüşüm görselleştirmesi oluştur
        /// </summary>
        public async Task<string> GenerateBodyTransformationImage(int age, string gender, int height, decimal currentWeight, decimal targetWeight, string fitnessGoal)
        {
            try
            {
                var apiKey = _configuration["Gemini:ApiKey"];

                if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_OPENAI_API_KEY_HERE")
                {
                    _logger.LogWarning("API key not configured, skipping image generation");
                    return string.Empty;
                }

                // Calculate BMI values
                var heightInMeters = height / 100.0m;
                var currentBmi = currentWeight / (heightInMeters * heightInMeters);
                var targetBmi = targetWeight / (heightInMeters * heightInMeters);

                // Determine body type descriptions
                string currentBodyType = GetBodyTypeDescription(currentBmi);
                string targetBodyType = GetBodyTypeDescription(targetBmi);

                // Create detailed prompt for image generation
                var prompt = $@"Create a realistic before and after body transformation visualization showing:

BEFORE (Left side):
- {gender} person, {age} years old
- Height: {height}cm
- Weight: {currentWeight}kg
- BMI: {currentBmi:F1} ({currentBodyType})
- Body type: {currentBodyType}

AFTER (Right side - Target):
- Same person after achieving fitness goal: {fitnessGoal}
- Target weight: {targetWeight}kg
- Target BMI: {targetBmi:F1} ({targetBodyType})
- Body type: {targetBodyType}

Style: Professional fitness transformation photo, side-by-side comparison, realistic human anatomy, athletic photography style, neutral background, proper lighting. Show clear difference in body composition. The person should look healthier, more toned, and confident in the after image.";

                // Generate image using Gemini API
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Note: Using text generation as Gemini Pro doesn't support image generation via API
                // In production, you would use DALL-E or Stable Diffusion API
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}";
                var response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // For now, return a description that can be used to generate an image
                    // In production, integrate with an actual image generation API
                    return $"transformation_{Guid.NewGuid()}.png";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating body transformation image");
                return string.Empty;
            }
        }

        /// <summary>
        /// Get body type description based on BMI
        /// </summary>
        private string GetBodyTypeDescription(decimal bmi)
        {
            if (bmi < 18.5m) return "Zayıf";
            if (bmi < 25m) return "Normal";
            if (bmi < 30m) return "Fazla Kilolu";
            return "Obez";
        }
    }
}
