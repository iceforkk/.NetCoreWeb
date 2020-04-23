using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FsCms.Web.Models;
using FsCms.Service.DAL;
using FsCms.Entity;
using Microsoft.AspNetCore.Http;
namespace FsCms.Web.Controllers
{
    public class ConstructionController : Controller
    {
        public ArticleContentDAL ArticleContentDAL { get; set; }

        public IActionResult Index()
        {
            var contentlist = ArticleContentDAL.Query(d => d.TypeID == 3, new List<SortInfo<ArticleContent, object>>
            {
                new SortInfo<ArticleContent, object>{ Orderby=s=>s.SortNum, SortMethods= Entity.Enum.SortEnum.Asc}
            }).list.OrderBy(s => s.SortNum).ToList();
            var contentFlist = ArticleContentDAL.Query(d => d.TypeID == 4, new List<SortInfo<ArticleContent, object>>
            {
                new SortInfo<ArticleContent, object>{ Orderby=s=>s.SortNum, SortMethods= Entity.Enum.SortEnum.Asc}
            }).list.OrderBy(s => s.SortNum).ToList();
            ViewBag.DocumentList = contentlist;
            ViewBag.DocumentFList = contentFlist;
            return View();
        }
        public IActionResult Details(int id = 1)
        {
            var content = ArticleContentDAL.GetByOne(m => m.Id == id);
            if (string.IsNullOrEmpty(content.WatchCount.ToString()))
            {
                content.WatchCount = 0;
            }
            else
            {
                content.WatchCount += 1;
            }
            ArticleContentDAL.Update(content);
            ViewBag.Document = content;
            return View();
        }
    }
}
