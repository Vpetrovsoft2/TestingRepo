using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace FinalTestingProject
{
    [TestFixture]
    public class UntitledTestCase : TestBase
    {

        [Test]
        public void GameMerchantsTesting()
        {
            AccountData accountFundist = new AccountData("DJPC_WLC", "@uly9A1I");
            appManager.Auth.LoginFundist(accountFundist);
            appManager.Navigation.GoToGamesMenu();
            List<string> merchantsListFromFundist = appManager.Fundist.GetMerchantsListFromFundistWithGames("Slots");
            List<string> allGamesList = appManager.Fundist.GetGameListFromFundist();
            appManager.Auth.LogoutFundist();

            appManager.Navigation.GoToUrl("https://prod-jpclive.egamings.com");
            AccountData accountWLC = new AccountData("Uragantest666+eur@gmail.com", "Test123!");
            appManager.Auth.LoginWLC(accountWLC, "theme");
            appManager.Wlc.CheckMerchantsOnDisplay();
            List<string> merchantsListOnWLC = appManager.Wlc.GetMerchantsInSelector();
            //merchantsListFromFundist.Sort();
            //merchantsListOnWLC.Sort();
            //Assert.That(merchantsListFromFundist, Is.EqualTo(merchantsListOnWLC));
            appManager.Wlc.CheckGameInSearch(allGamesList);            
            //appManager.Wlc.TakeMerchantsAndSeeGames(merchantsListOnWLC, allGamesList);
            //appManager.Wlc.StartGame();
            appManager.Auth.LogoutWLC("theme");
        }

    }
}