namespace Coffee.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal DonGia { get; set; }
        public int SoLuong { get; set; }
        public byte[]? Hinh { get; set; }
        public decimal ThanhTien
        {
            get { return SoLuong * DonGia; }
        }
        public CartItem()
        {

        }
        public CartItem(Sanpham sp)
        {
            ProductId = sp.ProductId;
            ProductName = sp.ProductName;
            DonGia = sp.Price;
            SoLuong = 1;
            Hinh = sp.Hinhanh;
        }
    }
}
