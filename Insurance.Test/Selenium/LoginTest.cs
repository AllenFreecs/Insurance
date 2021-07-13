using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Xunit;

namespace Insurance.Test.Selenium
{
   public class LoginTest
    {
        [Fact]
        [Trait("Category", "Smoke")]
        public void TestLogin()
        {

            using (IWebDriver driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))
            {
                driver.Navigate().GoToUrl("http://localhost:4200/login");
                Thread.Sleep(2000);
                IWebElement username = driver.FindElement(By.Id("username"));
                IWebElement password = driver.FindElement(By.Id("password"));
                IWebElement login = driver.FindElement(By.Id("login"));
                

                username.SendKeys("test");
                Thread.Sleep(2000);
                password.SendKeys("pass");
                Thread.Sleep(2000);
                login.Click();
                Thread.Sleep(2000);

                bool feedback = driver.FindElement(By.Id("inv")).Displayed;

                Assert.True(feedback);
                username.Clear();
                username.SendKeys("SA");
                Thread.Sleep(2000);
                password.Clear();
                password.SendKeys(Keys.Delete);
                Thread.Sleep(2000);
               
                password.SendKeys("Allen@123");
                login.Click();
                Thread.Sleep(2000);
                var cookie = driver.Manage().Cookies.AllCookies;

                Assert.NotNull(cookie);
            }
        }
    }
}
