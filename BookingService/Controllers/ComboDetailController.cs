//using BookingService.Models;
//using BookingService.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace BookingService.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class ComboDetailController : ControllerBase
//{
//    private readonly IComboDetailService _comboDetailService;

//    public ComboDetailController(IComboDetailService comboDetailService)
//    {
//        _comboDetailService = comboDetailService;
//    }

//    // ✅ TẠO MỚI - POST: /api/combodetail
//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] ComboDetail comboDetail)
//    {
//        await _comboDetailService.CreateComboDetailAsync(comboDetail);
//        return Ok(new { message = "Tạo chi tiết combo thành công." });
//    }

//    // ✅ LẤY TẤT CẢ - GET: /api/combodetail
//    [HttpGet]
//    public async Task<IActionResult> Readall()
//    {
//        var danhSach = await _comboDetailService.GetAllAsync();
//        return Ok(danhSach);
//    }

//    // ✅ LẤY THEO ID - GET: /api/combodetail/{id}
//    [HttpGet("{id}")]
//    public async Task<IActionResult> Realbyid(int id)
//    {
//        var detail = await _comboDetailService.GetByIdAsync(id);
//        if (detail == null)
//            return NotFound(new { message = "Không tìm thấy chi tiết combo." });
//        return Ok(detail);
//    }

//    // ✅ CẬP NHẬT - PUT: /api/combodetail
//    [HttpPut]
//    public async Task<IActionResult> Update([FromBody] ComboDetail comboDetail)
//    {
//        var tonTai = await _comboDetailService.GetByIdAsync(comboDetail.Id);
//        if (tonTai == null)
//            return NotFound(new { message = "Không tìm thấy chi tiết combo để cập nhật." });

//        await _comboDetailService.UpdateAsync(comboDetail);
//        return Ok(new { message = "Cập nhật chi tiết combo thành công." });
//    }

//    // ✅ XÓA - DELETE: /api/combodetail/{id}
//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Inactive(int id)
//    {
//        var detail = await _comboDetailService.GetByIdAsync(id);
//        if (detail == null)
//            return NotFound(new { message = "Chi tiết combo không tồn tại!" });

//        await _comboDetailService.DeleteAsync(id);
//        return Ok(new { message = "Xoá chi tiết combo thành công!" });
//    }
//}
