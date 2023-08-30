﻿using FastFoodChain16.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FastFoodChain16.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        QuanLyBanFastFood16Context da = new QuanLyBanFastFood16Context();
        //Hien thi DS Chi tiet don hang 
        [HttpPost("Get All OrderDetail")]
        public IActionResult GetOrderDetails()
        {
            var ds = da.ChiTietDonHangs.ToList();
            return Ok(ds);
        }

        //Hien thi DS Hoa Don Dat Hang theo id
        [HttpGet("Get OrderDetail by id ")]
        public IActionResult GetOrderDetailById(int id)
        {
            var ds = da.ChiTietDonHangs.FirstOrDefault(s => s.MaDh == id);
            return Ok(ds);
        }

        //Thong ke doanh thu theo thang 
        [HttpPost(" Cal total by month ")]
        public IActionResult CalTotalByMonth( int thang)
        {
            var ds = da.DonHangs.Where(s => s.NgayDat.Value.Month == thang)
                .Join(da.ChiTietDonHangs, d => d.MaDh, o => o.MaDh, (d, o) => new { d.MaDh, trigia = o.SoLuong * o.DonGia, thang = d.NgayDat.Value.Month })
                .GroupBy(s => s.thang).Select(s => new { s.Key, tongDT = s.Sum(s => s.trigia) }).ToList();
            return Ok(ds);
        }
        
        //Them Hoa don dat hang
        [HttpPost(" Add new OrderDetail")]
        public void AddOrderDetail([FromBody] OrderDetail orderDetail)
        {
            using (var tran = da.Database.BeginTransaction())
            {
                try
                {
                    ChiTietDonHang p = new ChiTietDonHang();
                    p.MaSp = orderDetail.MaSp;
                    p.SoLuong = orderDetail.SoLuong;
                    p.DonGia = orderDetail.DonGia;
                    p.Discount = orderDetail.Discount;
                    p.MaTk = orderDetail.MaTk;


                    da.ChiTietDonHangs.Add(p);
                    da.SaveChanges();
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();

                }

            }
           
        }
        //Sua Hoa don dat hang 
        [HttpPut(" Edit a OrderDetail")]
        public void EditOrderDetail([FromBody] OrderDetail orderDetail)
        {
            ChiTietDonHang p = da.ChiTietDonHangs.First(s => s.MaDh == orderDetail.MaDh);
            p.MaSp = orderDetail.MaSp;
            p.SoLuong = orderDetail.SoLuong;
            p.DonGia = orderDetail.DonGia;
            p.Discount = orderDetail.Discount;
            p.MaTk = orderDetail.MaTk;

            da.ChiTietDonHangs.Update(p);
            da.SaveChanges();
        }

        //Xoa chi tiet hoa don
        [HttpDelete(" Delete a OrderDetail")]
        public void DeleteOrderDetail(int id)
        {
            try
            {
                ChiTietDonHang p = da.ChiTietDonHangs.First(s => s.MaDh == id);
                da.ChiTietDonHangs.Remove(p);
                da.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
        
    



    }
}
