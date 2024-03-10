using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace Autodissmark.ExternalServices.WebDriverTaskBuilder;

public class WebDriverTaskBuilder : IWebDriverTaskBuilder
{
    private IWebDriver _driver;
    private Actions _actions;
    private WebDriverWait _defaultWait;

    public async Task SetLink(string link, int defaultWaitInSeconds, string? downloadDirectory = null)
    {
        if (downloadDirectory != null)
        {
            if (!Directory.Exists(downloadDirectory))
            {
                throw new Exception("Download directory is not exist");
            }

            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", downloadDirectory);
            _driver = new ChromeDriver(options);
        }
        else
        {
            _driver = new ChromeDriver();
        }

        _actions = new Actions(_driver);

        await Task.Run(() =>
        {
            _driver.Navigate().GoToUrl(link);
            _defaultWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(defaultWaitInSeconds));
        });
    }

    public async Task Click(string xPath)
    {
        IWebElement element = await GetElement(xPath);
        _actions.MoveToElement(element).Click().Perform();
    }

    public async Task Click(string xPath, int afterClickDelay, int times)
    {
        IWebElement element = await GetElement(xPath);

        for (int i = 0; i < times; i++)
        {
            _defaultWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(xPath)));
            _actions.MoveToElement(element).Click().Perform();
            Thread.Sleep(afterClickDelay);
        }
    }

    public async Task InputText(string xPath, string text, string? markerXPath = null, int? afterMarkerApearDelay = null)
    {
        IWebElement element = await GetElement(xPath);
        element.SendKeys(text);

        _defaultWait.Until((driver) =>
        {
            IWebElement element = driver.FindElement(By.XPath(xPath));
            var elementValueLength = element.GetAttribute("value").Length;
            var absDviation = (double)Math.Abs(text.Length - elementValueLength);
            var deviation = absDviation / text.Length;
            return deviation < 0.1;
        });

        //_defaultWait.Until(ExpectedConditions.TextToBePresentInElementValue(element, text));

        if (markerXPath is not null && afterMarkerApearDelay is not null)
        {
            _defaultWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(markerXPath)));
            Thread.Sleep(afterMarkerApearDelay.Value);
        }
    }

    public async Task ClearInput(string xPath)
    {
        IWebElement element = await GetElement(xPath);
        element.Clear();
    }

    public async Task WaitForDownladingFile(string downloadDirectory, string searchPattern = "*.*")
    {
        await Task.Run(() =>
        {
            _defaultWait.Until(d => Directory.GetFiles(downloadDirectory, searchPattern).Length > 0);
        });
    }

    public async Task<string> OutputText(string xPath, int totalApearDelay, string? markerXPath = null, int? afterMarkerApearDelay = null)
    {
        IWebElement element = await GetElement(xPath);

        if (markerXPath is not null && afterMarkerApearDelay is not null)
        {
            _defaultWait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(markerXPath)));
            Thread.Sleep(afterMarkerApearDelay.Value);
        }
        else
        {
            Thread.Sleep(totalApearDelay);
        }

        var reuslt = element.GetAttribute("value");

        _driver.Close();
        _driver.Quit();
        return reuslt;
    }

    public async Task<byte[]> OutputFirstByPatternDownladedFile(string downloadDirectory, string searchPattern = "*.*")
    {
        var filePath = Directory.GetFiles(downloadDirectory, "*.mp3")[0];

        byte[] res = File.ReadAllBytes(filePath);
        // Directory.Delete(downloadDirectory, true);

        await Task.Run(() =>
        {
            _driver.Close();
            _driver.Quit();
        });

        return res;
    }

    private async Task<IWebElement> GetElement(string xPath)
    {
        return await Task.Run(() => _defaultWait.Until(driver => driver.FindElement(By.XPath(xPath))));
    }
}
