Create Proc spDSKHchuaDH
As
Begin
       select *
	   from KhachHang
	   Where MaKH not in ( select distinct MaKH from DonHang)
End
exec spDSKHchuaDH
