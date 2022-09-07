using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace FinalTestingProject
{
    public class FundistHelper : HelperBase
    {
        public FundistHelper(ApplicationManager manager) : base(manager)
        {

        }

        /// <summary>
        /// Возвращает Список Игр
        /// </summary>
        /// <returns></returns>
        public List<string> GetGameListFromFundist()
        {
            List<string> allGamesList = new List<string>();
            SelectGameCategory("Slots");
            allGamesList = GetGamesListFromAllMerchants(GetMerchantsListFromFundist(), 1);
            return allGamesList;
        }
        /// <summary>
        /// Возвращает список всех мерчей подключенных на проекте
        /// </summary>
        /// <returns></returns>
        public List<string> GetMerchantsListFromFundist()
        {
            List<string> merchantList = new List<string>();
            MerchantsSelectorClick();
            var merchantsAmount = driver.FindElements(By.CssSelector("ul.select2-results__options .select2-results__options--nested li"));
            int count = 1;
            for (int i = 0; i < merchantsAmount.Count; i++)
            {
                string text = driver.FindElement(By.XPath($"//*[@id='select2-filter_type-results']/li[2]/ul/li[{count}]/span")).Text;
                merchantList.Add(text);
                count++;
            }
            return merchantList;
        }

        /// <summary>
        /// Возвращает список всех мерчей у которых есть игры
        /// </summary>
        /// <returns></returns>
        public List<string> GetMerchantsListFromFundistWithGames(string category)
        {
            List<string> merchantList = new List<string>();
            SelectGameCategory(category);
            MerchantsSelectorClick();    
            var merchantsAmount = driver.FindElements(By.CssSelector("ul.select2-results__options .select2-results__options--nested li"));
            int count = 0;
            int countForMerchant = 1;
            for (int i = 0; i <= merchantsAmount.Count - 105; i++)
            {
                try
                {
                    MerchantsSelectorClick();
                    driver.FindElements(By.CssSelector("ul.select2-results__options .select2-results__options--nested li"))[count].Click();
                }
                catch (Exception)
                {
                    MerchantsSelectorClick();
                    driver.FindElements(By.CssSelector("ul.select2-results__options .select2-results__options--nested li"))[count].Click();
                }
                Thread.Sleep(1000);
                driver.FindElement(By.Id("ButtonFilter")).Click();
                Thread.Sleep(10000);
                count++;
                try
                {
                    string errorCheck = driver.FindElement(By.Name("col-Error")).Text;
                    if (errorCheck == "Игр не найдено")
                    {
                        countForMerchant++;
                        continue;
                    }
                }
                catch (Exception)
                {
                    MerchantsSelectorDoubleClick();
                    string merchantName = driver.FindElement(By.XPath($"//*[@id='select2-filter_type-results']/li[2]/ul/li[{countForMerchant}]/span")).Text;
                    merchantList.Add(merchantName);
                    countForMerchant++;
                }
            }
            List<string> cleanMerchantList = CleanMerchantListFromFundist(merchantList);
            return cleanMerchantList;
        }


        /// <summary>
        /// Получаем список игр по всем мерчам
        /// </summary>
        /// <param name="allMerchants"></param>
        /// <param name="amountGamesInOneMerchant"></param>
        /// <returns></returns>
        public List<string> GetGamesListFromAllMerchants(List<string> allMerchants, int amountGamesInOneMerchant)
        {
            List<string> allGamesList = new List<string>();
            int count = 0;
            GamesStatusOnCLick();

            for (int i = 0; i < allMerchants.Count - 105; i++)
            { 
                try
                {
                    MerchantsSelectorClick();
                    driver.FindElements(By.CssSelector("ul.select2-results__options .select2-results__options--nested li"))[count].Click();
                }
                catch (Exception)
                {
                    MerchantsSelectorClick();
                    driver.FindElements(By.CssSelector("ul.select2-results__options .select2-results__options--nested li"))[count].Click();
                }
                Thread.Sleep(1000);
                driver.FindElement(By.Id("ButtonFilter")).Click();
                Thread.Sleep(10000);
                try
                {
                    string errorCheck = driver.FindElement(By.Name("col-Error")).Text;
                    if (errorCheck == "Игр не найдено")
                    {
                        count++;
                        continue;
                    }
                }
                catch (Exception)
                {
                    allGamesList.AddRange(GetGameListOnTheSide(amountGamesInOneMerchant));
                    count++;
                }

            }

            int howManyPages = 15 - amountGamesInOneMerchant;
            int timesCount = 0;
            for (int i = 4; howManyPages < 0; i--)
            {
                if (i == 1)
                {
                    break;
                }
                int result = howManyPages - 15;
                if (result > 0)
                {
                    break;
                }

                timesCount++;
            }
            if (timesCount > 0 && timesCount < 2)
            {
                try
                {
                    driver.FindElement(By.CssSelector("ul.pagination :nth-child(1) a")).Click();
                    new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                    allGamesList.AddRange(GetGameListOnTheSide(amountGamesInOneMerchant));
                }
                catch (Exception)
                {
                    return allGamesList;
                }

            }
            else
            {
                try
                {
                    int index = 1;
                    for (int i = 0; i < howManyPages; i++)
                    {
                        driver.FindElement(By.CssSelector($"ul.pagination :nth-child({index}) a")).Click();
                        new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                        allGamesList.AddRange(GetGameListOnTheSide(amountGamesInOneMerchant));
                        index++;
                    }
                }
                catch (Exception)
                {

                    return allGamesList;
                } 
            }
            return allGamesList;
        }

        /// <summary>
        /// Возвращает список лист со страницы
        /// </summary>
        /// <param name="amountGamesInOneMerchant"></param>
        /// <returns></returns>
        public List<string> GetGameListOnTheSide(int amountGamesInOneMerchant)
        {
            List<string> gameList = new List<string>();
            int index = 2;
            for (int i = 1; i <= amountGamesInOneMerchant; i++)
            {             
                try
                {
                    string gamesName = driver.FindElement(By.CssSelector($"table#gamesList :nth-child({index}) label")).Text;
                    gameList.Add(gamesName);
                    index++;
                }
                catch (Exception)
                {
                    break;
                }
            }     
            return gameList;
        }

        /// <summary>
        /// Кликает на заданную категорию
        /// </summary>
        /// <returns></returns>
        public void SelectGameCategory(string category)
        {
            List<string> allCategoriesList = new List<string>();
            int count = 2;
            driver.FindElement(By.Id("select2-IDCategory-container")).Click();
            var allCategories = driver.FindElements(By.CssSelector("ul.select2-results__options li"));
            for (int i = 0; i < allCategories.Count-2; i++)
            {
                string nameCategory = driver.FindElement(By.CssSelector($"span.select2-results  :nth-child({count})")).Text;
                allCategoriesList.Add(nameCategory);
                count++;
            }
            bool checkCategory = allCategoriesList.Contains(category);
            if (checkCategory)
            {
                int index = allCategoriesList.IndexOf(category);
                driver.FindElement(By.CssSelector($"span.select2-results  :nth-child({index + 2})")).Click();
            }
            else
            {
                throw new Exception("Category not found!");
            }        
        }
        /// <summary>
        /// Раскрывает селектор выбора мерча
        /// </summary>
        public void MerchantsSelectorClick()
        {
            driver.FindElement(By.Id("select2-filter_type-container")).Click();
        }

        /// <summary>
        /// Раскрывает селектор выбора мерча
        /// </summary>
        public void MerchantsSelectorDoubleClick()
        {
            driver.FindElement(By.Id("select2-filter_type-container")).Click();
            driver.FindElement(By.Id("select2-filter_type-container")).Click();
        }

        /// <summary>
        /// Включает отображение включенных игр
        /// </summary>
        public void GamesStatusOnCLick()
        {
            driver.FindElement(By.Id("select2-game_status-container")).Click();
            //В фандисте Id не статичен
            driver.FindElement(By.CssSelector("ul.select2-results__options :nth-child(4)")).Click();
        }

        /// <summary>
        /// Убирает лишние символы
        /// </summary>
        /// <param name="merchantListFromFundist"></param>
        /// <returns></returns>
        public List<string> CleanMerchantListFromFundist(List<string> merchantListFromFundist)
        {
            List<string> result = new List<string>();
            int index = 0;
            for (int i = 0; i <= merchantListFromFundist.Count - 1; i++)
            {
                string text = merchantListFromFundist[index];
                string newListTrue = text.Remove(text.LastIndexOf('(')).Trim();
                result.Add(newListTrue);
                index++;
            }
            return result;
        }
    }
}
