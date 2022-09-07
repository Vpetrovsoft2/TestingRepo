using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace FinalTestingProject
{
    public class ApplicationManager
    {
        public IWebDriver driver;
        protected string baseURL;

        protected NavigationHelper navigator;
        protected FundistHelper fundist;
        protected WLCHelper wlc;
        protected LoginHelper auth;
        
        private static ThreadLocal<ApplicationManager> appManager = new ThreadLocal<ApplicationManager>();

        public ApplicationManager()
        {
            driver = new ChromeDriver();
            baseURL = "https://www.fundist.org";


            navigator = new NavigationHelper(this, baseURL);
            fundist = new FundistHelper(this);
            wlc = new WLCHelper(this);
            auth = new LoginHelper(this);
        }

        public static ApplicationManager GetInstance()
        {
            if (!appManager.IsValueCreated)
            {
                ApplicationManager newInstance = new ApplicationManager();
                newInstance.Navigation.GoToHomePageFundist();
                appManager.Value = newInstance;
            }
            return appManager.Value;
        }

        public void Stop()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        public LoginHelper Auth
        {
            get
            {
                return auth;
            }
        }
        public IWebDriver Driver
        {
            get
            {
                return driver;
            }
        }

        public NavigationHelper Navigation
        {
            get
            {
                return navigator;
            }
        }

        public FundistHelper Fundist
        {
            get
            {
                return fundist;
            }
        }
        public WLCHelper Wlc
        {
            get
            {
                return wlc;
            }
        }

    }
}
