﻿using System.IO;

namespace MetacriticScraper.UnitTests.TestData
{
    public static class TestDataPaths
    {
        public static string FirstPageHtml
        {
            get
            {
                var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                return string.Format(@"{0}\{1}", directory, @"TestData\FirstPage.html");
            }
        }
    }
}