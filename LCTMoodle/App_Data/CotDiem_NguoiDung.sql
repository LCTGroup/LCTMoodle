use rtcmfraf_Moodle;

GO
--Cột điểm _ Người dùng
CREATE TABLE dbo.CotDiem_NguoiDung (
	MaCotDiem INT NOT NULL,
	MaNguoiDung INT NOT NULL,
	Diem FLOAT(1) NOT NULL,
	MaNguoiTao INT NOT NULL,
	ThoiDiemTao DATETIME NOT NULL DEFAULT GETDATE(),
	PRIMARY KEY (MaCotDiem, MaNguoiDung)
)

GO
--Lấy chuỗi người dùng - điểm theo mã khóa học
ALTER PROC dbo.layCotDiem_NguoiDungTheoMaKhoaHoc_ChuoiNguoiDung_Diem (
	@0 INT --MaKhoaHoc
)
AS
BEGIN
	DECLARE @chuoiMa VARCHAR(MAX) = ''

	--Lấy chuỗi người dùng
	SELECT @chuoiMa += CAST(MaNguoiDung AS VARCHAR) + ','
		FROM KhoaHoc_NguoiDung
		WHERE 
			MaKhoaHoc = @0 AND
			LaHocVien = 1

	--Nếu không tồn tại học viên thì dừng
	IF (@chuoiMa = '')
	BEGIN
		RETURN
	END

	--Tạo ngăn cách giữa người dùng & điểm = dấu '|'
	SET @chuoiMa = LEFT(@chuoiMa, LEN(@chuoiMa) - 1) + '|'

	--Lấy chuỗi điểm
	SELECT @chuoiMa += CASE
		WHEN CD_ND.Diem IS NULL THEN
			','
		ELSE 
			CAST(CD_ND.Diem AS VARCHAR(MAX)) + ','
		END
	FROM 
		NguoiDung ND
			--Lấy người dùng là học viên của khóa học
			INNER JOIN KhoaHoc_NguoiDung KH_ND ON
				KH_ND.MaKhoaHoc = @0 AND
				KH_ND.MaNguoiDung = ND.Ma
			--Lấy cột điểm của khóa học
			RIGHT JOIN CotDiem CD ON
				CD.MaKhoaHoc = @0
			--Lấy điểm của cột điểm
			LEFT JOIN CotDiem_NguoiDung CD_ND ON
				CD.Ma = CD_ND.MaCotDiem AND
				ND.Ma = CD_ND.MaNguoiDung
	ORDER BY ND.Ma, CD.ThuTu

	SELECT LEFT(@chuoiMa, LEN(@chuoiMa) - 1)
END

GO
--Cập nhật điểm
ALTER PROC dbo.capNhatCotDiem_NguoiDung_Nhieu (
	@0 dbo.BangCotDiem_NguoiDung READONLY --Bảng cột điểm - người dùng
)
AS
BEGIN
	--Xóa điểm cũ nếu có
	DELETE CD_ND 
		FROM 
			dbo.CotDiem_NguoiDung CD_ND
				INNER JOIN @0 B ON
					B.MaNguoiDung = CD_ND.MaNguoiDung AND
					B.MaCotDiem = CD_ND.MaCotDiem
					
	--Thêm điểm mới vào
	INSERT INTO dbo.CotDiem_NguoiDung (MaCotDiem, MaNguoiDung, Diem, MaNguoiTao)
		SELECT MaCotDiem, MaNguoiDung, Diem, MaNguoiTao
			FROM @0
END

GO
--Cập nhật điểm
CREATE PROC dbo.capNhatCotDiem_NguoiDung_Mot (
	@0 INT, --MaCotDiem
	@1 INT, --ManguoiDung
	@2 INT, --Diem
	@3 INT --MaNguoiTao
)
AS
BEGIN
	--Cập nhật điểm cũ nếu có
	UPDATE dbo.CotDiem_NguoiDung
		SET 
			Diem = @2,
			MaNguoiTao = @3
		WHERE 
			MaCotDiem = @0 AND
			MaNguoiDung = @1

	IF (@@ROWCOUNT = 0)
	BEGIN
		INSERT INTO dbo.CotDiem_NguoiDung (MaCotDiem, MaNguoiDung, Diem, MaNguoiTao) VALUES
			(@0, @1, @2, @3)
	END
END

