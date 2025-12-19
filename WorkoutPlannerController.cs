using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GymManagementSystem.Models.ViewModels;
using GymManagementSystem.Services;

namespace GymManagementSystem.Controllers
{
    /// <summary>
    /// Controller for AI-powered workout planner
    /// متحكم مخطط التمارين بالذكاء الاصطناعي
    /// </summary>
    [Authorize]
    public class WorkoutPlannerController : Controller
    {
        private readonly IOpenAIService _openAIService;
        private readonly ILogger<WorkoutPlannerController> _logger;

        public WorkoutPlannerController(IOpenAIService openAIService, ILogger<WorkoutPlannerController> logger)
        {
            _openAIService = openAIService;
            _logger = logger;
        }

        // GET: WorkoutPlanner
        [HttpGet]
        public IActionResult Index()
        {
            return View(new WorkoutPlannerViewModel());
        }

        // POST: WorkoutPlanner/Generate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Generate(WorkoutPlannerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Generating workout plan for user with goal: {Goal}", model.FitnessGoal);

                    // Generate workout plan using OpenAI
                    var plan = await _openAIService.GenerateWorkoutPlan(
                        model.Height,
                        model.Weight,
                        model.FitnessGoal.ToString()
                    );

                    model.GeneratedPlan = plan;

                    TempData["Success"] = "Workout plan generated successfully!";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating workout plan");
                    ModelState.AddModelError("", "An error occurred while generating your workout plan. Please try again.");
                }
            }

            return View("Index", model);
        }
    }
}
