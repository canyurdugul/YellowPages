using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YellowPages.Business.Abstracts;
using YellowPages.Business.Contracts.DTOs.Person;
using YellowPages.Business.Contracts.DTOs.Report;
using YellowPages.Database.UnitOfWork.Abstracts;
using YellowPages.Entities.Person;
using YellowPages.Entities.Report;

namespace YellowPages.Client.ApiReport.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {

        private readonly ILogger<ReportController> _logger;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IReportBusiness _reportBusiness;

        public ReportController(ILogger<ReportController> logger, IUnitOfWorkFactory unitOfWorkFactory, IReportBusiness reportBusiness)
        {
            _logger = logger;
            _unitOfWorkFactory = unitOfWorkFactory;
            _reportBusiness = reportBusiness;
        }

        [HttpGet]
        [Route("get/{id:guid}")]
        public async Task<JsonResult> Get(Guid? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _reportBusiness.GetById(unitOfWork, id.Value);
                return new JsonResult(result);
            }
        }
        [HttpGet]
        [Route("download-report/{id:guid}")]
        public async Task<byte[]> DownloadReport(Guid? id)
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _reportBusiness.GetById(unitOfWork, id.Value);
                if (System.IO.File.Exists(result.ReportPath))
                {
                    var fileByteArray = System.IO.File.ReadAllBytes(result.ReportPath);
                    return fileByteArray;
                }
                else
                {
                    return null;
                }
            }
        }
        [HttpGet]
        [Route("get-all")]
        public async Task<JsonResult> GetAll()
        {
            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _reportBusiness.GetReportQuery(unitOfWork);
                return new JsonResult(result.Select(s => new ReportDTO
                {
                    Id = s.Id,
                    CreatedDate = s.CreatedDate,
                    ReportStatus = s.ReportStatus,
                }));
            }
        }
        [HttpPost]
        [Route("create-or-update")]
        public async Task<JsonResult> CreateOrUpdate()
        {
            var report = new Report()
            {
                ReportStatus = Common.Enums.ReportStatusEnum.Pending
            };

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _reportBusiness.Insert(unitOfWork, report);
                return new JsonResult(result);
            }
        }
        [HttpDelete]
        [Route("delete/{id:guid}")]
        public async Task<JsonResult> Delete(Guid? id)
        {
            if (id == Guid.Empty)
                return new JsonResult(BadRequest("Id parameter can't be null"));

            using (var unitOfWork = _unitOfWorkFactory.Create())
            {
                var result = await _reportBusiness.DeleteById(unitOfWork, id.Value);
                return new JsonResult(result);
            }
        }
    }
}
