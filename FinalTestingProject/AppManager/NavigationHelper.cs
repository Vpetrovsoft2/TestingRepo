using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalTestingProject
{
    public class NavigationHelper : HelperBase
    {
        protected string baseURL;
        public NavigationHelper(ApplicationManager manager, string baseURL) : base(manager)
        {
            this.baseURL = baseURL;
        }
        public void GoToGamesMenu()
        {
            driver.FindElement(By.Id("SideMenu-Merchants")).Click();
            driver.FindElement(By.CssSelector("#SideMenu-Merchants-Games > span.nav-label")).Click();
        }

        public void GoToHomePageFundist()
        {
             if (IsElementPresent(By.Id("Login")))
             {
                 return;
             }
             driver.Navigate().GoToUrl(baseURL = "https://www.fundist.org");
        }

        public void GoToUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => d.FindElements(By.CssSelector("div.col")).Count > 0);
        }

    }
}
