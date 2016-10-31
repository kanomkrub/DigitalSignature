using Microsoft.VisualStudio.TestTools.UnitTesting;
using DigitalSignatureService.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalSignatureService.Tests;
using System.Net.Http;

namespace DigitalSignatureService.Core.Tests
{
    [TestClass()]
    public class ContentManagerTests
    {
        [TestMethod()]
        public void CreateTemplateTest()
        {
            var contentManager = ContentManager.GetInstance();
            var templateId1 = contentManager.CreateTemplate("testJaa", "descriptionJaa", null);
            var ids = contentManager.GetTemplateIds();
            var templates = contentManager.GetTemplates();
            if (!ids.Contains(templateId1)) Assert.Fail("create or get template fail");
            if (!templates.Any(t=>t.id == templateId1)) Assert.Fail("create or get template fail");
            contentManager.DeleteTemplate(templateId1);
            ids = contentManager.GetTemplateIds();
            if (ids.Contains(templateId1)) Assert.Fail("delete template fail");
        }
        
        //[TestMethod()]
        //public void CreateTemplateTest2()
        //{
        //    var contentManager = ContentManager.GetInstance();
        //    var templateId1 = contentManager.CreateTemplate("Template0002", "descriptionJaa", PDFUtilityTests.GetTestList());
        //}
    }
}