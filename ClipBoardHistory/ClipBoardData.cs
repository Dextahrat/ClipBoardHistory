

namespace ClipBoardHistory
{
    public class ClipBoardData
    {
        public int Id { get; set; }
        public string? CreateDateWithDay
        {
            get
            {
                return CreateDate.ToString(SQLiteDbContext.DateFormat) + Environment.NewLine + Environment.NewLine + CreateDate.ToString("dddd");
            }
        }
        public string? Brief
        {
            get
            {
                if (IsImage)
                    return "[Image]" + Environment.NewLine + Environment.NewLine + " -- For More Double Click -- ";

                string? temp = CBText;
                if (!string.IsNullOrEmpty(temp))
                {
                    //4 satırdan fazla olan textler için brief kısmında satır sayısı kıs
                    var newLineCount = temp.Length - temp.Replace(Environment.NewLine,"").Length;
                    if (newLineCount > 4)
                    {
                        var tempArray = temp.Split(Environment.NewLine);
                        temp =  (tempArray[0]??"") + Environment.NewLine + (tempArray[1]??"") + Environment.NewLine
                            + (tempArray[2]??"") + Environment.NewLine + (tempArray[3]??"") + Environment.NewLine
                            + Environment.NewLine + " -- For More Double Click -- ";
                    }

                    //500 Harften fazla olan textler için brief kımsında harf sayısı kıs
                    if (temp.Length > 500)
                        temp = temp.Substring(0, 450) + Environment.NewLine + Environment.NewLine + " -- For More Double Click -- ";
                }

                return temp;
            }
        }
        public string? Note { get; set; }

        public string? CBText { get; set; }

        /// <summary>Kopyalanan metnin RTF biçimi (varsa). Liste yüklenirken okunmaz, gerektiğinde ayrıca çekilir.</summary>
        public string? Rtf { get; set; }

        /// <summary>Kopyalanan metnin HTML biçimi (varsa, CF_HTML formatında). Liste yüklenirken okunmaz.</summary>
        public string? Html { get; set; }

        /// <summary>Resim kayıtlarında AppConstants.ImageDirectory içindeki PNG dosyasının adı; metin kayıtlarında null.</summary>
        public string? ImagePath { get; set; }

        public bool IsImage => !string.IsNullOrEmpty(ImagePath);

        public string? ImageFullPath => IsImage ? Path.Combine(AppConstants.ImageDirectory, ImagePath!) : null;

        /// <summary>
        /// Veritabanında "yyyy-MM-dd HH:mm:ss" biçiminde metin olarak saklanır
        /// (bkz. SQLiteDbContext.OnModelCreating), kodda DateTime olarak kullanılır.
        /// </summary>
        public DateTime CreateDate { get; set; }

        public Color DayColor
        {
            get
            {
                switch (CreateDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        return Color.Green;
                    case DayOfWeek.Tuesday:
                        return Color.Red;
                    case DayOfWeek.Wednesday:
                        return Color.Blue;
                    case DayOfWeek.Thursday:
                        return Color.SaddleBrown;
                    case DayOfWeek.Friday:
                        return Color.Yellow;
                    case DayOfWeek.Saturday:
                        return Color.Orange;
                    case DayOfWeek.Sunday:
                        return Color.Orchid;
                    default:
                        return Color.White;
                }

            }
        }

    }
}
