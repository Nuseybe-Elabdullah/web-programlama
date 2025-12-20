namespace GymManagementSystem.Services
{
    /// <summary>
    /// Interface for OpenAI service
    /// واجهة خدمة OpenAI
    /// </summary>
    public interface IOpenAIService
    {
        Task<string> GenerateWorkoutPlan(int height, decimal weight, string fitnessGoal);
        Task<string> GenerateBodyTransformationImage(int age, string gender, int height, decimal currentWeight, decimal targetWeight, string fitnessGoal);
    }
}
