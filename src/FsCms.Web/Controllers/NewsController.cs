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
    public class NewsController : Controller
    {
        public ArticleContentDAL ArticleContentDAL { get; set; }
        public ArticleTypeDAL ArticleTypeDAL { get; set; }

        public IActionResult Index(int id,int page = 1,int count = 10)
        {
            var content = ArticleContentDAL.Query(d => d.Status == 1 && d.TypeID == id, new List<SortInfo<ArticleContent, object>>
                {
                    new SortInfo<ArticleContent, object>{ Orderby=s=>s.SortNum, SortMethods= Entity.Enum.SortEnum.Asc}
                }, new Entity.Common.PageInfo { PageIndex = page, PageSize = count, IsPaging = true });
            var contentlist = content.list.OrderBy(s => s.SortNum).ToList();
            var contentcount = content.count;
            var type = ArticleTypeDAL.GetByOne(d => d.Id == id);
            ViewBag.DocumentList = contentlist;
            ViewData["ListCount"] = contentcount;
            ViewData["TypeID"] = id;
            ViewData["TypeName"] = type != null ? type.TypeName : string.Empty;
            ViewData["ShowType"] = id == 6 ? "2" : id == 0 ? "3" : "1";
            return View();
        }
        public IActionResult Details(int id)
        {
            var content = ArticleContentDAL.GetByOne(m => m.Id == id);
            var type = content != null ? ArticleTypeDAL.GetByOne(m => m.Id == content.TypeID) : null;
            if(string.IsNullOrEmpty(content.WatchCount.ToString()))
            {
                content.WatchCount = 0;
            }else
            {
                content.WatchCount += 1;
            }
            ArticleContentDAL.Update(content);
            ViewBag.Document = content;
            ViewData["TypeName"] = type != null ? type.TypeName : string.Empty ;
            return View();
        }

        [HttpPost]
        public JsonResult ArticleContentList(int id,int page,int count)
        {
            var contentlist = ArticleContentDAL.Query(d => d.Status == 1 && d.TypeID == id, new List<SortInfo<ArticleContent, object>>
                {
                    new SortInfo<ArticleContent, object>{ Orderby=s=>s.SortNum, SortMethods= Entity.Enum.SortEnum.Asc}
                }, new Entity.Common.PageInfo { PageIndex = page, PageSize = count, IsPaging = true }).list.OrderBy(s => s.SortNum).ToList();
            return Json(new { data = contentlist, total = contentlist.Count });
        }
    }
}
