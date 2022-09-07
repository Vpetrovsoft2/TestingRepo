using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinalTestingProject
{
    public class TestBase
    {
        protected ApplicationManager appManager;


        [SetUp]
        public void SetupApplicationManagerTest()
        {
            appManager = ApplicationManager.GetInstance();
        }

        [TearDown]
        public void TeardownTest()
        {
            appManager.Stop();
        }

        
    }
}
