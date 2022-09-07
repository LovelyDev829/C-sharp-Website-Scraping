using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fstream = File.OpenWrite("out.csv");
            string base_url = "https://www.khanacademy.org/";
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            IWebDriver driver = new ChromeDriver(service, options, TimeSpan.FromMinutes(3));
            driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));
            driver.Url = base_url;

            var elements = driver.FindElements(By.ClassName("_1b2zmqj"));

            foreach (IWebElement elem in elements)
            {
                string course_title = elem.FindElement(By.ClassName("_17zmj242")).GetAttribute("innerHTML");
                Console.WriteLine(course_title); // Course
                var summaries = elem.FindElements(By.ClassName("_xy39ea8"));
                foreach (IWebElement summ in summaries)
                {
                    string course_summary_title = summ.GetAttribute("innerHTML");
                    Console.WriteLine(course_summary_title); // Course Summary
                    IWebElement pare = summ.FindElement(By.XPath(".."));

                    IWebDriver driver1 = new ChromeDriver(service, options, TimeSpan.FromMinutes(3));
                    driver1.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));
                    driver1.Url = pare.GetAttribute("href");

                    var units = driver1.FindElements(By.ClassName("_xu2jcg"));
                    foreach (IWebElement unit in units)
                    {
                        try
                        {
                            string unit_title = unit.FindElement(By.ClassName("_k50sd54")).GetAttribute("innerHTML");
                            Console.WriteLine(unit_title); // Unit

                            IWebDriver driver2 = new ChromeDriver(service, options, TimeSpan.FromMinutes(3));
                            driver2.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));
                            driver2.Url = unit.FindElement(By.ClassName("_dwmetq")).GetAttribute("href");

                            var lessons = driver2.FindElements(By.ClassName("_2qe0as"));

                            foreach (IWebElement lesson in lessons)
                            {
                                string lesson_title = lesson.FindElement(By.ClassName("_dwmetq")).GetAttribute("innerHTML");
                                Console.WriteLine(lesson_title); // lesson
                                var videos = lesson.FindElements(By.ClassName("_1qwvhpzm"));
                                foreach (IWebElement video in videos)
                                {
                                    string video_title = video.FindElement(By.ClassName("_14hvi6g8")).GetAttribute("innerHTML");
                                    string video_link = video.GetAttribute("href");
                                    Console.WriteLine(video_title); // video title
                                    Console.WriteLine(video_link); // video path
                                    string out_line_string = course_title + ",";
                                    out_line_string += course_summary_title + ",";
                                    out_line_string += unit_title + ",";
                                    out_line_string += lesson_title + ",";
                                    out_line_string += video_title + ",";
                                    out_line_string += video_link + ",";
                                    out_line_string += "Video\n";
                                    fstream.Write(Encoding.UTF8.GetBytes(out_line_string), 0, Encoding.UTF8.GetBytes(out_line_string).Length);
                                }
                                var webs = lesson.FindElements(By.ClassName("_16c6bd9"));
                                foreach (IWebElement web in webs)
                                {
                                    string web_title = web.FindElement(By.ClassName("_pc9bder")).GetAttribute("innerHTML");
                                    string web_link = web.FindElement(By.ClassName("_dwmetq")).GetAttribute("href");
                                    Console.WriteLine(web_title); // web title
                                    Console.WriteLine(web_link); // web url
                                    string out_line_string = course_title + ",";
                                    out_line_string += course_summary_title + ",";
                                    out_line_string += unit_title + ",";
                                    out_line_string += lesson_title + ",";
                                    out_line_string += web_title + ",";
                                    out_line_string += web_link + ",";
                                    out_line_string += "Website\n";
                                    fstream.Write(Encoding.UTF8.GetBytes(out_line_string), 0, Encoding.UTF8.GetBytes(out_line_string).Length);
                                }
                            }
                            driver2.Close();
                        }
                        catch(Exception)
                        {

                        }
                    }
                    driver1.Close();
                }
            }
            driver.Close();
            fstream.Close();
        }
    }
}
