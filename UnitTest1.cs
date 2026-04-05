using Microsoft.Playwright;
using NUnit.Framework;
using System.Threading.Tasks;

namespace SauceDemoTests
{
    public class BuyTshirtTests
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;

        [SetUp]
        public async Task Setup()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            _page = await _browser.NewPageAsync();
        }

        [Test]
        public async Task BuyTshirt()
        {
            // Credentials from environment variables
            var username = Environment.GetEnvironmentVariable("USERNAME")
                ?? throw new ArgumentNullException("USERNAME is not set");

            var password = Environment.GetEnvironmentVariable("PASSWORD")
                ?? throw new ArgumentNullException("PASSWORD is not set");

            await _page.GotoAsync("https://www.saucedemo.com/");

            // Login
            await _page.FillAsync("#user-name", username);
            await _page.FillAsync("#password", password);
            await _page.ClickAsync("#login-button");

            // Add T-shirt (Sauce Labs Bolt T-Shirt)
            await _page.ClickAsync("text=Sauce Labs Bolt T-Shirt");
            await _page.ClickAsync("button:has-text('Add to cart')");

            // Go to cart
            await _page.ClickAsync(".shopping_cart_link");

            // Checkout
            await _page.ClickAsync("button:has-text('Checkout')");

            await _page.FillAsync("#first-name", "Evita");
            await _page.FillAsync("#last-name", "Paipale");
            await _page.FillAsync("#postal-code", "LV5422");

            await _page.ClickAsync("input:has-text('Continue')");
            await _page.ClickAsync("button:has-text('Finish')");

            // Assertion
            var confirmation = await _page.InnerTextAsync(".complete-header");
            Assert.That(confirmation, Does.Contain("Thank you"));
        }

        [TearDown]
        public async Task TearDown()
        {
            await _browser.CloseAsync();
        }
    }
}
