

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
                string temp = CBText;
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
