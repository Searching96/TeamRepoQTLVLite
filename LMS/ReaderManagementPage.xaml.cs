using LoginApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LoginApp
{
    public partial class ReaderManagementPage : Window
    {
        private readonly AppDbContext _context;
        public ObservableCollection<Reader> Readers { get; set; }

        public ReaderManagementPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            Readers = new ObservableCollection<Reader>();
            ReadersDataGrid.ItemsSource = Readers;
            LoadReaders();
        }

        private void LoadReaders()
        {
            Readers.Clear();
            var readers = _context.Readers.ToList();
            foreach (var reader in readers)
            {
                Readers.Add(reader);
            }
        }

        private void AddReader_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reader newReader = GetReaderFromInputs();
                if (newReader != null)
                {
                    _context.Readers.Add(newReader);
                    _context.SaveChanges();
                    Readers.Add(newReader);
                    ClearInputs();
                }
            }
            catch (DbUpdateException ex) 
            {
                MessageBox.Show($"Lỗi khi thêm độc giả: {ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Lỗi khi thêm độc giả: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateReader_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersDataGrid.SelectedItem is Reader selectedReader)
            {
                try
                {
                    // Tạo một bản sao của đối tượng Reader để cập nhật
                    var updatedReader = new Reader
                    {
                        MaDocGia = selectedReader.MaDocGia,
                        HoTen = HoTen.Text,
                        NgaySinh = NgaySinh.SelectedDate ?? selectedReader.NgaySinh,
                        DiaChi = DiaChi.Text,
                        Email = Email.Text,
                        SoDienThoai = SoDienThoai.Text,
                        LoaiDocGia = LoaiDocGiaComboBox.Text,
                        NgayLapThe = NgayLapTheDatePicker.SelectedDate ?? selectedReader.NgayLapThe,
                        NgayHetHan = NgayHetHanDatePicker.SelectedDate,
                        TongNo = decimal.TryParse(TongNoTextBox.Text, out decimal tongNo) ? tongNo : selectedReader.TongNo
                    };

                    // Thực hiện kiểm tra hợp lệ
                    if (updatedReader.NgaySinh >= updatedReader.NgayLapThe)
                    {
                        MessageBox.Show("Ngày sinh phải nhỏ hơn ngày lập thẻ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (updatedReader.NgayLapThe >= updatedReader.NgayHetHan)
                    {
                        MessageBox.Show("Ngày lập thẻ phải nhỏ hơn ngày hết hạn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Cập nhật cơ sở dữ liệu
                    var readerToUpdate = _context.Readers.Find(updatedReader.MaDocGia);
                    if (readerToUpdate != null)
                    {
                        _context.Entry(readerToUpdate).CurrentValues.SetValues(updatedReader);
                        _context.SaveChanges();

                        // Cập nhật UI
                        var index = Readers.IndexOf(selectedReader);
                        Readers[index] = updatedReader;
                        ReadersDataGrid.Items.Refresh();

                        MessageBox.Show("Cập nhật thông tin độc giả thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy độc giả để cập nhật.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (DbUpdateException ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật độc giả trong cơ sở dữ liệu: {ex.InnerException?.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một độc giả để cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeleteReader_Click(object sender, RoutedEventArgs e)
        {
            if (ReadersDataGrid.SelectedItem is Reader selectedReader)
            {
                Readers.Remove(selectedReader);
                ClearInputs();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một độc giả để xóa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void HoTen_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[a-zA-Z\s]+$");
        }

        private void SoDienThoai_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        private void TongNoTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
        }

        private void TongNoTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(TongNoTextBox.Text, out decimal result))
            {
                TongNoTextBox.Text = result.ToString("N2");
            }
            else
            {
                TongNoTextBox.Text = "0.00";
            }
        }
        private Reader GetReaderFromInputs()
        {
            try
            {
                string loaiDocGia = LoaiDocGiaComboBox.Text;
                if (!new[] { "Học sinh/Sinh viên", "Giáo viên", "Khách" }.Contains(loaiDocGia))
                {
                    MessageBox.Show("Loại độc giả không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                DateTime ngaySinh = NgaySinh.SelectedDate.GetValueOrDefault();
                DateTime ngayLapThe = NgayLapTheDatePicker.SelectedDate.GetValueOrDefault();
                DateTime? ngayHetHan = NgayHetHanDatePicker.SelectedDate;

                if (ngaySinh >= ngayLapThe)
                {
                    MessageBox.Show("Ngày sinh phải nhỏ hơn ngày lập thẻ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                if (ngayLapThe >= ngayHetHan)
                {
                    MessageBox.Show("Ngày lập thẻ phải nhỏ hơn ngày hết hạn.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                if (!Regex.IsMatch(HoTen.Text, @"^[a-zA-Z\s]+$"))
                {
                    MessageBox.Show("Họ tên chỉ được chứa chữ cái và khoảng trắng.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                if (!Regex.IsMatch(SoDienThoai.Text, @"^\d{10}$"))
                {
                    MessageBox.Show("Số điện thoại phải chứa đúng 10 số.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

                if (!decimal.TryParse(TongNoTextBox.Text, out decimal tongNo))
                {
                    MessageBox.Show("Tổng nợ không hợp lệ.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
                return new Reader
                {
                    MaDocGia = MaDocGia.Text,
                    HoTen = HoTen.Text,
                    NgaySinh = NgaySinh.SelectedDate ?? DateTime.Now,
                    DiaChi = DiaChi.Text,
                    Email = Email.Text,
                    SoDienThoai = SoDienThoai.Text,
                    LoaiDocGia = LoaiDocGiaComboBox.Text,
                    NgayLapThe = NgayLapTheDatePicker.SelectedDate ?? DateTime.Now,
                    NgayHetHan = NgayHetHanDatePicker.SelectedDate,
                    TongNo = decimal.Parse(TongNoTextBox.Text)
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = SearchTextBox.Text.Trim().ToLower();
            string searchCriteria = (SearchCriteriaComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            var query = _context.Readers.AsQueryable();

            switch (searchCriteria)
            {
                case "Mã Độc Giả":
                    query = query.Where(r => r.MaDocGia.ToLower().Contains(searchTerm));
                    break;
                case "Họ Tên":
                    query = query.Where(r => r.HoTen.ToLower().Contains(searchTerm));
                    break;
                case "Email":
                    query = query.Where(r => r.Email.ToLower().Contains(searchTerm));
                    break;
                case "Số Điện Thoại":
                    query = query.Where(r => r.SoDienThoai.Contains(searchTerm));
                    break;
                case "Loại Độc Giả":
                    query = query.Where(r => r.LoaiDocGia.ToLower().Contains(searchTerm));
                    break;
                default:
                    // Nếu không có tiêu chí nào được chọn, tìm kiếm trên tất cả các trường
                    query = query.Where(r =>
                        r.MaDocGia.ToLower().Contains(searchTerm) ||
                        r.HoTen.ToLower().Contains(searchTerm) ||
                        r.Email.ToLower().Contains(searchTerm) ||
                        r.SoDienThoai.Contains(searchTerm) ||
                        r.LoaiDocGia.ToLower().Contains(searchTerm)
                    );
                    break;
            }

            var filteredReaders = query.ToList();

            Readers.Clear();
            foreach (var reader in filteredReaders)
            {
                Readers.Add(reader);
            }
        }
        private void ClearInputs()
        {
            MaDocGia.Text = string.Empty;
            HoTen.Text = string.Empty;
            NgaySinh.SelectedDate = null;
            DiaChi.Text = string.Empty;
            Email.Text = string.Empty;
            SoDienThoai.Text = string.Empty;
            LoaiDocGiaComboBox.SelectedIndex = -1; 
            NgayLapTheDatePicker.SelectedDate = null;
            NgayHetHanDatePicker.SelectedDate = null;
            TongNoTextBox.Text = string.Empty;
        }

        private void ReadersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReadersDataGrid.SelectedItem is Reader selectedReader)
            {
                MaDocGia.Text = selectedReader.MaDocGia;
                HoTen.Text = selectedReader.HoTen;
                NgaySinh.SelectedDate = selectedReader.NgaySinh;
                DiaChi.Text = selectedReader.DiaChi;
                Email.Text = selectedReader.Email;
                SoDienThoai.Text = selectedReader.SoDienThoai;
                LoaiDocGiaComboBox.Text = selectedReader.LoaiDocGia;
                NgayLapTheDatePicker.SelectedDate = selectedReader.NgayLapThe;
                NgayHetHanDatePicker.SelectedDate = selectedReader.NgayHetHan;
                TongNoTextBox.Text = selectedReader.TongNo.ToString();
            }
        }
    }

    public class Reader
    {
        public string MaDocGia { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string LoaiDocGia { get; set; }
        public DateTime NgayLapThe { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public decimal TongNo { get; set; }
    }
}