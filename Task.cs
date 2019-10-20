using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Task
{
    [TestFixture]
    public class TaskTestClass : BaseTestClass
    {
        [SetUp]
        public void Start()
        {
            LaunchBrowser();
            RegistrationPage();
        }

        [TestCase]
        //valid data
        public void CardNumber1()
        {
            InputCardNumber("4276 3100 3748 5911");
            IsPageSmsCode();
        }

        [TestCase]
        //11 numbers - less than required
        public void CardNumber2()
        {
            InputCardNumber("4276 3100 3748 591");
            IsNotPageSmsCode();
        }

        [TestCase]
        //15 numbers - more than required
        public void CardNumber3()
        {
            InputCardNumber("4276 3100 3748 591118 0");
            NumderChecking("4276 3100 3748 591118 0");
            IsPageSmsCode();
        }

        [TestCase]
        //card of another bank
        public void CardNumber4()
        {
            InputCardNumber("5211 7828 4798 0050");
            IsNotPageSmsCode();
        }

        [TestCase]
        //empty input
        public void CardNumber5()
        {
            InputCardNumber("");
            IsNotPageSmsCode();
        }

        [TearDown]
        public void End()
        {
            Close();
        }

    }
    //_____________________________________________________________________________________________________//
    public class BaseTestClass
    {
        public IWebDriver driver;
        IWebElement element;

        public void LaunchBrowser()
        {
            driver = new ChromeDriver();
            driver.Url = "https://online.sberbank.ru";
        }

        public void RegistrationPage()
        {
            element = driver.FindElement(By.TagName("a"));
            element.Click();
        }

        public void InputCardNumber(string cardNumber)
        {
            element = driver.FindElement(By.Id("cardNumber"));
            element.SendKeys(cardNumber);
            element = driver.FindElement(By.XPath("//*[@title='Продолжить']"));
            element.Click();
        }

        public void NumderChecking (string cardNumber)
        {
            Assert.AreNotEqual(element.GetAttribute("value"), cardNumber);
        }

        public void IsPageSmsCode()
        {
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(driver.PageSource.Contains("confirmPassword"));
        }

        public void IsNotPageSmsCode()
        {
            System.Threading.Thread.Sleep(1000);
            Assert.IsFalse(driver.PageSource.Contains("confirmPassword"));
        }

        public void Close()
        {
            driver.Close();
        }
    }
}
