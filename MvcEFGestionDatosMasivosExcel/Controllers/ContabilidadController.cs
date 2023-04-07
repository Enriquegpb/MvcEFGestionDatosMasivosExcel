using Microsoft.AspNetCore.Mvc;
using MvcEFGestionDatosMasivosExcel.Models;
using MvcEFGestionDatosMasivosExcel.Repositories;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using DataTableExtensions = MvcEFGestionDatosMasivosExcel.Extensions.DataTableExtensions;

namespace MvcEFGestionDatosMasivosExcel.Controllers
{
    public class ContabilidadController : Controller
    {
        private IRepoContabilidad repo;
        public ContabilidadController(IRepoContabilidad repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Contabilidad> contabilidades = await this.repo.GetContabilidades();
            return View(contabilidades);
        }














        public async Task<string> GenerateAndDownLoadExcel()
        {
            List<Contabilidad> Contabilidads = await this.repo.GetContabilidades();
            var dataTable = DataTableExtensions.GetDataTable(Contabilidads);
            //dataTable.Columns.Remove("IDContabilidad");

            byte[] fileContents = null;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets.Add("Contabilidades");
                ws.Cells["A1"].Value = "Contabilidad";
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.Font.Size = 18;
                ws.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A2"].Value = "Listado de la Contabilidad";
                ws.Cells["A2"].Style.Font.Bold = true;
                ws.Cells["A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A3"].LoadFromDataTable(dataTable, true);
                ws.Cells["A3:L3"].Style.Font.Bold = true;
                ws.Cells["A3:L3"].Style.Font.Size = 14;
                ws.Cells["A3:L3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells["A3:L3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightCoral);
                ws.Cells["A3:L3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A3:L3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                excelPackage.Save();
                fileContents = excelPackage.GetAsByteArray();
            }
            return Convert.ToBase64String(fileContents);

        }

        public async Task<List<Contabilidad>> ImportContabilidades(IFormFile informe)
        {
            List<Contabilidad> Contabilidads = new List<Contabilidad>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(informe.OpenReadStream()))
            {
                ExcelWorkbook wk = excelPackage.Workbook;
                ExcelWorksheet ws = wk.Worksheets["Contabilidades"];
                int numberrows = ws.Dimension.Rows;
                for (int row = 4; row <= numberrows; row++)
                {
                    Contabilidad cuenta = new Contabilidad
                    {
                        Id = int.Parse(ws.Cells[row, 1].Value.ToString().Trim()),
                        Apellidos = ws.Cells[row, 2].Value.ToString().Trim(),
                        Nombre = ws.Cells[row, 3].Value.ToString().Trim(),
                        Direccion = ws.Cells[row, 4].Value.ToString().Trim(),
                        Email = ws.Cells[row, 5].Value.ToString().Trim(),
                        Saldo = int.Parse(ws.Cells[row, 6].Value.ToString().Trim()),
                    };
                    Contabilidads.Add(cuenta);
                }
            }
            return Contabilidads;
        }















        [HttpPost]
        public async Task<IActionResult> ImportContabilidad(IFormFile file)
        {

            List<Contabilidad> contabilidades = await this.ImportContabilidades(file);
            await this.repo.DeleteMasiveData(contabilidades);
            await this.repo.ImportMasiveData(contabilidades);
            return RedirectToAction("Index", "Contabilidad");
        }  
        [HttpPost]
        public async Task<IActionResult> SubirSaldo(int incremento)
        {

            List<Contabilidad> contabilidades = await this.repo.GetContabilidades();
            await this.repo.UpdateMasiveData(contabilidades, incremento);
            return RedirectToAction("Index", "Contabilidad");
        }

    }
}
