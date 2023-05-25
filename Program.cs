using System;
using System.IO;
using HtmlAgilityPack;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;


//Exemplo de Crawler em C# com a utilização do Selenium para consulta e extração de dados em sites
//funcionalidades navega dentro do site, busca info, preenche campos, realiza de/para com csv externo, extrai informações

namespace CrawlerConsultaExtracao
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            //Console.OutputEncoding = System.Text.Encoding.ASCII;

            //ABRE IE e CRIA MÉTODO WAIT
            IWebDriver ie = new InternetExplorerDriver();
            WebDriverWait wait_ie = new WebDriverWait(ie, TimeSpan.FromSeconds(10));
            WebDriverWait waitpage = new WebDriverWait(ie, TimeSpan.FromSeconds(10));

        //STOP
            ie.Navigate().GoToUrl(@"**************");//url sensível
            ie.Manage().Window.Maximize();

        //LOGIN B3
            wait_ie.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.Id("iframeLoginMar2"))));
            ie.SwitchTo().Frame(ie.FindElement(By.TagName("iframe")));
            IWebElement campoFuncional = ie.FindElement(By.Name("username"));
            campoFuncional.SendKeys("*********"); // dado sensivel login
            IWebElement campoSenha = ie.FindElement(By.Name("password"));
            campoSenha.SendKeys("******" + Keys.Enter); // dado sensível pw
        //HOME B3
            wait_ie.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.Id("frmTotal"))));
            ie.SwitchTo().Frame(ie.FindElement(By.Id("frmTotal")));
            ie.FindElement(By.LinkText("Demais feriados")).Click();
        //CONSULTA DE FERIADOS
            wait_ie.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.Id("Pagina"))));   //ESPERA CARREGAR O id Pagina
            IWebElement dataInicial = ie.FindElement(By.Name("data"));                              //SELECIONA O CAMPO PARA DIGITAR A DATA
            string dataAtual = DateTime.Now.ToString("ddMMyyyy");                                  //CRIA STRING COM A DATA ATUAL FORMATO ddMMyyy
            string dataFutura = DateTime.Today.AddDays(10).ToString("ddMMyyyy");

        //IWebElement esperarTrocaDeCampo = waitpage.Until(e => e.FindElement(By.Name("Agencia"))); WAIT DE TROCA DE CAMPO CASO NECESSÁRIO
            IWebElement dataFinal = ie.FindElement(By.Name("DataFinal"));
            dataFinal.SendKeys((Keys.Control +"a"+ Keys.Backspace));            // força apagar campo dataFinal
            dataFinal.SendKeys(dataFutura + Keys.Enter);                       //escreve data no campo
        //CRIAR STRING DE BUSCA
            string DES_CIDADE = "CARAPICUIBA";
            string DES_UF = "SP";

            string nomeEstado = GetEstado(DES_UF);
            Console.WriteLine(nomeEstado);

            string cidadeEstado = (DES_CIDADE + " - " + nomeEstado);
            Console.WriteLine(cidadeEstado);

            wait_ie.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((By.Id("tblAgencias"))));
            IWebElement table = ie.FindElement(By.Id("tblAgencias"));
            IList<IWebElement> tableRow = table.FindElements(By.TagName("tr"));
            IList<IWebElement> rowTD;
            int linhas = 0;
            foreach (IWebElement row in tableRow)
            {
                rowTD = row.FindElements(By.TagName("td"));

                if (row.Text.Contains(cidadeEstado))
                {
                    Console.WriteLine("REGISTRO ENCONTRADO");
                    Console.WriteLine("");
                    Console.WriteLine("FERIADO EM "+DES_CIDADE+", " +nomeEstado);

                    ie.Quit();
                    break;
                }
                else
                {
                    linhas++;
                    Console.WriteLine(linhas);
                }
            }



            /*
            IWebElement municipio = ie.FindElement(By.Name("CodMunicipio"));
            municipio.SendKeys(cidadeEstado);
            Console.WriteLine(dataFutura);
 */
                //string dataFutura = DateTime.Today.AddDays(10).ToString("ddMMyyyy");
                //Console.WriteLine(dataAtual);
                //Console.WriteLine(dataFutura);
                //string datas = dataAtual + dataFutura;
                //dataInicial.SendKeys(datas);




                //ie.SwitchTo().Frame(ie.FindElement(By.TagName("frmTotal")));
                //IWebElement dataFinal = ie.FindElement(By.Id("dataFinal"));
                //dataFinal.SendKeys(DateTime.Today.AddDays(10).ToString("ddMMyyyy"));







            //IWebElement demaisFeriados = ie.FindElement(By.LinkText("Demais feriados"));
            //demaisFeriados.Click();




            //driver.FindElement(By.Id("formLoginNormal_username" idToFind)).SendKeys("987323449");


            //driver.Quit(); -> fechar o Browser
            Console.ReadLine();
        }
        //FUNÇÕES FORA DA MAIN



        public static string GetEstado(string searchUF)
        {
            var strLines = File.ReadLines("DEPARA_UF_ESTADOUTF.csv");
            foreach (var line in strLines)
            {
                if (line.Split(';')[0].Equals(searchUF))
                    return line.Split(';')[1];
            }

            return "";
        }


    }
}
