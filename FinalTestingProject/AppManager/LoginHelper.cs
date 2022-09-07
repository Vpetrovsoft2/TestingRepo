using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FinalTestingProject
{
    public class LoginHelper : HelperBase
    {
        public LoginHelper(ApplicationManager manager) : base(manager)
        {

        }
        public void LoginFundist(AccountData accountFundist)
        {
            if (isLoggedIn())
            {
                if (isLoggedIn(accountFundist))
                {
                    return;
                }
                LogoutFundist();
            }
            Type(By.Id("Login"), accountFundist.Username);
            Type(By.Id("Password"), accountFundist.Password);
            driver.FindElement(By.Id("LoginBtn")).Click();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                .Until(d => d.FindElements(By.CssSelector("div.slimScrollDiv")).Count > 0);
        }

        public void LogoutWLC(string wlc)
        {
            if (wlc == "theme")
            {
                driver.FindElement(By.Id("wlc-btn-logout")).Click();
            }
            else
            {
                driver.FindElement(By.CssSelector("[wlcelement='icon_user']")).
                    FindElement(By.CssSelector("data-wlc-element='link - logout'")).Click();
            }
        }

        /// <summary>
        /// Введи "theme" or "engine" 
        /// </summary>
        /// <param name="accountWLC"></param>
        /// <param name="projectCheck"></param>
        public void LoginWLC(AccountData accountWLC, string projectCheck)
        {
            if (projectCheck == "theme")
            {
                if (isLoggedIn())
                {
                    if (isLoggedIn(accountWLC))
                    {
                        return;
                    }
                    LogoutWLC(projectCheck);
                }
                driver.FindElement(By.Id("wlc-btn-login")).Click();
                Type(By.Id("profile-email"), accountWLC.Username);
                Type(By.Id("profile-password"), accountWLC.Password);
                driver.FindElement(By.CssSelector("[type='submit']")).Click();
                Thread.Sleep(1000);
            }
            else
            {
                //тут engine
                if (isLoggedIn())
                {
                    if (isLoggedIn(accountWLC))
                    {
                        return;
                    }
                    LogoutWLC(projectCheck);
                }
                driver.FindElement(By.XPath("//div[3]/button/span")).Click();
                Type(By.Id("email"), accountWLC.Username);
                Type(By.Id("password"), accountWLC.Password);
                driver.FindElement(By.XPath("//form/button/span")).Click();
            }
        }

        public bool isLoggedIn()
        {
            return IsElementPresent(By.Id("LogoutButton"));
        }
        public bool isLoggedIn(AccountData account)
        {
            return isLoggedIn()
                && GetLoggetUserName() == account.Username;
        }

        public string GetLoggetUserName()
        {
            string text = driver.FindElement(By.Id("CurrentLogin")).Text;
            return text.Substring(1, text.Length - 2);
        }

        public void LogoutFundist()
        {
            // Если кнопки логаут нету, то ничего не делать
            if (isLoggedIn())
            {
                driver.FindElement(By.Id("LogoutButton")).Click();
            }
        }
    }
}
