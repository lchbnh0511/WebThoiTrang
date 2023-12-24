Create proc sp_ThongKe
as
BEGIN
	declare @SoTruyCapGanNhat int
	declare @Count int
	select @Count = count(*)
	from ThongKes
	if @Count is null set @Count = 0
	if @Count = 0
		insert into ThongKes(ThoiGian, SoTruyCap)
		values(GETDATE(), 1)
	else
		begin
			declare @ThoiGianGanNhat datetime
			select @SoTruyCapGanNhat = tk.SoTruyCap, @ThoiGianGanNhat = tk.ThoiGian
			from ThongKes tk
			where tk.ID = (select max(tk1.ID) from ThongKes tk1)
			if @SoTruyCapGanNhat is null  set @SoTruyCapGanNhat = 0
			if @ThoiGianGanNhat is null set @ThoiGianGanNhat = GETDATE()
			--Nếu chuyển sang ngày mới chưa (sau 12 giờ đêm)
			--Nếu chưa chuyển sang ngày mới thì update
			if day(@ThoiGianGanNhat) = day(getdate())
				begin
					update ThongKes
					set SoTruyCap = @SoTruyCapGanNhat + 1, ThoiGian = GETDATE()
					where ID = (select max(tk1.ID) from ThongKes tk1)
				end
				--Nếu đã sang ngày mới thì insert thêm 1 bản ghi mới
				else
					insert into ThongKes(ThoiGian, SoTruyCap)
					values(GETDATE(), 1)
		end
		--Số truy cập ngày hôm nay
		declare @HomNay int
		set @HomNay = DATEPART(DW,GETDATE())
		select @SoTruyCapGanNhat = SoTruyCap, @ThoiGianGanNhat = ThoiGian
		from ThongKes
		where ID=(select max(ID) from ThongKes)
		--Số truy cập ngày hôm qua
		declare @TruyCapHomQua bigint	
		select @TruyCapHomQua = ISNULL(SoTruyCap, 0)
		from ThongKes
		where CONVERT(nvarchar(20), ThoiGian, 103) = CONVERT(nvarchar(20), GETDATE()-1, 103)
		if @TruyCapHomQua is null set @TruyCapHomQua = 0

		--Truy cập đầu tuần này
		DECLARE @DauTuanNay datetime
		SET @DauTuanNay= DATEADD(wk, DATEDIFF(wk, 6, GetDate()), 6)
		-- Tính Ngày đầu của tuần trước Tính từ thời điểm 00:00:))
		DECLARE @NgayDauTuanTruoc datetime
		SET @NgayDauTuanTruoc = Cast(CONVERT(nvarchar(30),DATEADD(dd, -(@HomNay+6), GetDate()),101) AS datetime)
		-- Tính ngày cuối tuần trước tính đến 24h ngày cuối tuần 
		DECLARE @NgayCuoiTuanTruoc datetime
		SET @NgayCuoiTuanTruoc = Cast(CONVERT(nvarchar(30),DATEADD(dd, -@HomNay, GetDate()),101) +' 23:59:59' AS datetime)

		--Số truy cập tuần này
		declare @TruyCapTuanNay bigint
		select @TruyCapTuanNay = ISNULL(sum(SoTruyCap), 0)
		from ThongKes
		where ThoiGian between @DauTuanNay and GETDATE()	
		--Số truy cập tuần trước
		declare @SoLanTruyCapTuanTruoc bigint
		select @SoLanTruyCapTuanTruoc = ISNULL(sum(SoTruyCap), 0)
		from ThongKes ttk
		where ttk.ThoiGian between @NgayDauTuanTruoc and @NgayCuoiTuanTruoc
		--Số truy cập tháng này
		declare @SoTruyCapThangNay bigint 
		select @SoTruyCapThangNay=isnull(sum(SoTruyCap),0)
		from ThongKes ttk 
		where MONTH(ttk.ThoiGian)=MONTH(GETDATE())
		--Số truy cập tháng trước
		declare @SoTruyCapThangTruoc bigint 
		select @SoTruyCapThangTruoc=isnull(sum(SoTruyCap),0)
		from ThongKes ttk 
		where MONTH(ttk.ThoiGian)=MONTH(GETDATE())-1

		-- Tính tổng số
		DECLARE @TongSo bigint
		SELECT  @TongSo=isnull(Sum(SoTruyCap),0) FROM ThongKes ttk
		
		SELECT @SoTruyCapGanNhat AS HomNay, 
		@TruyCapHomQua AS HomQua,
		@TruyCapTuanNay AS TuanNay, 
		@SoLanTruyCapTuanTruoc AS TuanTruoc, 
		@SoTruyCapThangNay AS ThangNay, 
		@SoTruyCapThangTruoc AS ThangTruoc,
		@TongSo AS TatCa
END