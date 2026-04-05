using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using System.Text.RegularExpressions;

namespace PlaywrightTests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ExampleTest : PageTest
{
    [Test]
    public async Task BuyTShirt()
    {
        string username = "standard_user";
        string password = "secret_sauce";
        string firstname = "Evita";
        string lastname = "Paipale";
        string postcode = "LV5422";


        await Page.GotoAsync("https://www.saucedemo.com/");

        await Page.FillAsync("#user-name", username);
        await Page.FillAsync("#password", password);
        await Page.ClickAsync("#login-button");

        // 3. Add T-shirt to cart (Sauce Labs Bolt T-Shirt)
        await Page.ClickAsync("text=Sauce Labs Bolt T-Shirt");
        await Page.ClickAsync("button:has-text('Add to cart')");

        // 4. Go to cart
        await Page.ClickAsync(".shopping_cart_link");

        // Verify item is in cart
        var itemVisible = await Page.IsVisibleAsync("text=Sauce Labs Bolt T-Shirt");
        if (!itemVisible)
        {
            throw new Exception("T-shirt not found in cart!");
        }

        // 5. Proceed to checkout
        await Page.ClickAsync("#checkout");

        // 6. Fill checkout info
        await Page.FillAsync("#first-name", firstname);
        await Page.FillAsync("#last-name", lastname);
        await Page.FillAsync("#postal-code", postcode);

        await Page.ClickAsync("#continue");

        // 7. Finish purchase
        await Page.ClickAsync("#finish");

        // 8. Verify success message
        var successText = await Page.InnerTextAsync(".complete-header");

        if (successText.Contains("Thank you for your order"))
        {
            Console.WriteLine("Test Passed: Order completed successfully.");
        }
        else
        {
            throw new Exception("Test Failed: Order not completed.");
        }

    }
}