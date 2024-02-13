using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace GymLog.Web.Tests.Home;

public class HomePageTests : IDisposable
{
    private readonly IWebDriver _webDriver = new ChromeDriver();

    public void Dispose()
    {
        _webDriver.Quit();
        _webDriver.Dispose();
    }

    private void NavigateToHome()
    {
        _webDriver.Navigate().GoToUrl("http://localhost:5142/");
        Thread.Sleep(3000);
    }

    [Fact]
    public void NavigateToHomePage()
    {
        NavigateToHome();

        ReadOnlyCollection<IWebElement> cards = _webDriver.FindElements(By.ClassName("card"));

        Assert.Equal("GymLog - Home", _webDriver.Title);
        Assert.NotEmpty(cards);
    }

    [Fact]
    public void ChangeDate()
    {
        NavigateToHome();

        IWebElement selectElement = _webDriver.FindElement(By.Id("selectDate"));
        SelectElement select = new(selectElement);
        select.SelectByText("2024-01-26");

        int cardCount = _webDriver.FindElements(By.ClassName("card")).Count;

        Assert.Equal(2, cardCount);
    }

    [Fact]
    public void CreateWorkout()
    {
        NavigateToHome();

        _webDriver.FindElement(By.ClassName("btn-success")).Click();

        Thread.Sleep(1000);

        _webDriver.FindElement(By.Id("duration")).SendKeys("1 hour");
        _webDriver.FindElement(By.Id("sets")).SendKeys("3");
        _webDriver.FindElement(By.Id("reps")).SendKeys("10");

        Thread.Sleep(1000);

        _webDriver.FindElement(By.ClassName("btn-primary")).Click();

        Thread.Sleep(2000);

        bool createSuccessful = _webDriver.FindElement(By.ClassName("blazored-toast-message")).Displayed;

        Assert.True(createSuccessful);
    }

    [Fact]
    public void CreateWorkout_FormValidation()
    {
        NavigateToHome();

        _webDriver.FindElement(By.ClassName("btn-success")).Click();

        Thread.Sleep(1000);

        _webDriver.FindElement(By.ClassName("btn-primary")).Click();

        _webDriver.FindElement(By.Id("sets")).Clear();
        _webDriver.FindElement(By.Id("reps")).Clear();

        Thread.Sleep(2000);

        ReadOnlyCollection<IWebElement> validationMessages = _webDriver.FindElements(By.ClassName("validation-message"));

        bool durationFieldValidationMessage = validationMessages.Any(x => x.Text == "The Duration field is required.");
        bool setsFieldValidationMessage = validationMessages.Any(x => x.Text == "The Sets field must be a number.");
        bool repsFieldValidationMessage = validationMessages.Any(x => x.Text == "The Reps field must be a number.");

        Thread.Sleep(1000);

        _webDriver.FindElement(By.ClassName("btn-primary")).Click();

        Assert.True(durationFieldValidationMessage);
        Assert.True(setsFieldValidationMessage);
        Assert.True(repsFieldValidationMessage);
    }

    [Fact]
    public void OpenWorkoutDetails()
    {
        NavigateToHome();

        _webDriver.FindElement(By.ClassName("btn-primary")).Click();

        Thread.Sleep(1000);

        string title = _webDriver.FindElement(By.ClassName("bm-title")).Text;

        Assert.Equal("Workout details", title);
    }

    [Fact]
    public void UpdateWorkout()
    {
        NavigateToHome();

        _webDriver.FindElement(By.ClassName("btn-secondary")).Click();

        Thread.Sleep(1000);

        IWebElement? durationElement = _webDriver.FindElement(By.Id("duration"));
        durationElement.Clear();
        durationElement.SendKeys("2 hours");

        IWebElement? setsElement = _webDriver.FindElement(By.Id("sets"));
        setsElement.Clear();
        setsElement.SendKeys("5");

        IWebElement? repsElement = _webDriver.FindElement(By.Id("reps"));
        repsElement.Clear();
        repsElement.SendKeys("5");

        Thread.Sleep(1000);

        _webDriver.FindElement(By.ClassName("btn-primary")).Click();

        Thread.Sleep(2000);

        bool updateSuccessful = _webDriver.FindElement(By.ClassName("blazored-toast-message")).Displayed;

        Assert.True(updateSuccessful);
    }

    [Fact]
    public void UpdateWorkout_FormValidation()
    {
        NavigateToHome();

        _webDriver.FindElement(By.ClassName("btn-secondary")).Click();

        Thread.Sleep(1000);

        _webDriver.FindElement(By.Id("duration")).Clear();
        _webDriver.FindElement(By.Id("sets")).Clear();
        _webDriver.FindElement(By.Id("reps")).Clear();

        Thread.Sleep(2000);

        ReadOnlyCollection<IWebElement> validationMessages = _webDriver.FindElements(By.ClassName("validation-message"));

        bool durationFieldValidationMessage = validationMessages.Any(x => x.Text == "The Duration field is required.");
        bool setsFieldValidationMessage = validationMessages.Any(x => x.Text == "The Sets field must be a number.");
        bool repsFieldValidationMessage = validationMessages.Any(x => x.Text == "The Reps field must be a number.");

        Thread.Sleep(1000);

        _webDriver.FindElement(By.ClassName("btn-primary")).Click();

        Assert.True(durationFieldValidationMessage);
        Assert.True(setsFieldValidationMessage);
        Assert.True(repsFieldValidationMessage);
    }
    
    [Fact]
    public void DeleteWorkout()
    {
        NavigateToHome();

        _webDriver.FindElement(By.ClassName("btn-danger")).Click();

        Thread.Sleep(1000);

        _webDriver.FindElement(By.ClassName("btn-danger")).Click();

        Thread.Sleep(2000);

        bool deleteSuccessful = _webDriver.FindElement(By.ClassName("blazored-toast-message")).Displayed;

        Assert.True(deleteSuccessful);
    }

    [Fact]
    public void NavigateToExercises()
    {
        NavigateToHome();

        _webDriver.FindElement(By.Id("linkExercises")).Click();

        Thread.Sleep(2000);

        IWebElement title = _webDriver.FindElement(By.TagName("h1"));

        Assert.Equal("GymLog - Exercises", _webDriver.Title);
        Assert.Equal("Exercises", title.Text);
    }

    [Fact]
    public void NavigateToWorkouts()
    {
        NavigateToHome();

        _webDriver.FindElement(By.Id("linkWorkouts")).Click();

        Thread.Sleep(2000);

        IWebElement title = _webDriver.FindElement(By.TagName("h1"));

        Assert.Equal("GymLog - Workouts", _webDriver.Title);
        Assert.Equal("Workouts", title.Text);
    }
}