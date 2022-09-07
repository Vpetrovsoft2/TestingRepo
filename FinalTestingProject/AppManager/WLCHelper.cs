using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace FinalTestingProject
{
    public class WLCHelper : HelperBase
    {
        public WLCHelper(ApplicationManager manager) : base(manager)
        {

        }

        /// <summary>
        /// Ищет все игры через поиск 
        /// </summary>
        /// <param name="gamesList"></param>
        public void CheckGameInSearch(List<string> gamesList)
        {
            int index = 0;
            for (int i = 1; i <= gamesList.Count; i++)
            {
                string text = gamesList[index];
                driver.FindElement(By.CssSelector("[data-wlc-element='input_search']")).Clear();
                driver.FindElement(By.CssSelector("[data-wlc-element='input_search']")).SendKeys(text);
                try
                {
                    driver.FindElement(By.CssSelector("div.wlc-game-catalog__games :nth-child(1)"));

                }
                catch (Exception)
                {
                    throw new Exception($"Game {gamesList[index]} not found");
                }
                index++;
            }
            driver.FindElement(By.CssSelector("[data-wlc-element='input_search']")).Clear();
        }
        
        /// <summary>
        /// Возвращает список мерчей из селектора на WLC
        /// </summary>
        /// <param name="allMerchantsFromFundist"></param>
        public List<string> GetMerchantsInSelector()
        {
            var allElements = driver.FindElements(By.CssSelector(".wlc-filter-list__item"));
            List<string> merchantsInWLC = new List<string>();
            int index = 2;
            Thread.Sleep(5000);
            for (int i = 0; i < allElements.Count - 1; i++)
            {
                driver.FindElement(By.CssSelector("div#wlc-filter-merchants")).Click();
                string merchant = driver.FindElement(By.XPath($"//*[@id='wlc-filter-merchants-list']/li[{index}]/div")).Text;
                merchantsInWLC.Add(merchant);
                index++;
            }
            return merchantsInWLC;
        }

        /// <summary>
        /// Выбирает мерча и ищет соответcтвующую игру (работает только если 1 игра - 1 мерч)
        /// </summary>
        /// <param name="merchantsList"></param>
        /// <param name="gamesList"></param>
        public void TakeMerchantsAndSeeGames(List<string> merchantsList, List<string> gamesList)
        {
            //тут нужно изменить логику, чтобы любое количество игр проверялось
            int index = 0;
            int gameCount = 1;
            for (int i = 0; i < merchantsList.Count; i++)
            {
                driver.FindElement(By.CssSelector("div#wlc-filter-merchants")).Click();
                string gameName = gamesList[index];
                driver.FindElement(By.CssSelector($"ul.wlc-filter-list :nth-child({index + 2})")).Click();
                for (int k = 0; k < merchantsList.Count; k++)
                {
                    try
                    {
                        //тут нужно переделать, убрать try/catch и получить сначала список игр на wlc и уже в нём искать нашу игру
                        string tryFounding = driver.FindElement(By.CssSelector($"#wlc-game-catalog > " +
                            $"div:nth-child({gameCount}) > div > div > div.row.wlc-game-thumb__footer > " +
                            $"div > div.col.wlc-game-thumb-title > div")).Text;
                        if (gameName == tryFounding)
                        {
                            driver.FindElement(By.CssSelector($"ul.wlc-filter-list :nth-child({index + 2})")).Click();
                            break;
                        }
                        gameCount++;
                    }
                    catch (Exception)
                    {
                        driver.FindElement(By.XPath("//*[@id='applicationContainer']/div/div/section[7]/div/div/div[2]/div[2]/div/div/div/div[2]/div[2]/div[2]/button")).Click();
                        continue;
                    }
                }
                index++;
                gameCount++;
            }

        }

        internal void StartGame()
        {
            //запуск игр по списку
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновляет страницу пока не появятся мерчи
        /// </summary>
        public void CheckMerchantsOnDisplay()
        {
            var check = driver.FindElements(By.CssSelector("li.wlc-filter-list__item"));

            while (check.Count == 1)
            {
                driver.Navigate().Refresh();
                new WebDriverWait(driver, TimeSpan.FromSeconds(20))
                    .Until(d => d.FindElements(By.CssSelector("div.col")).Count > 0);
                check = driver.FindElements(By.CssSelector("li.wlc-filter-list__item"));
            }
            //Тут еще будет ограничение по количеству обновлений
        }
    }
}
