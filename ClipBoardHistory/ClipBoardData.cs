

namespace ClipBoardHistory
{
    public class ClipBoardData
    {
        public int Id { get; set; }
        public string? CreateDateWithDay 
        { 
            get 
            {
                var date = DateTime.ParseExact(CreateDate ?? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss") , "yyyy-MM-dd HH:mm:ss",null);
                return date.ToString("yyyy-MM-dd HH:mm:ss")+Environment.NewLine+Environment.NewLine+date.ToString("dddd"); 
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
        public string? CreateDate { get; set; }
        public Color DayColor 
        {
            get 
            {
                var date = DateTime.ParseExact(CreateDate ?? DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
                switch ((int)(date.DayOfWeek))
                {
                    case 1:
                        return Color.Green;
                    case 2:
                        return Color.Red;
                    case 3:
                        return Color.Blue;
                    case 4:
                        return Color.SaddleBrown;
                    case 5:
                        return Color.Yellow;
                    case 6:
                        return Color.Orange;
                    case 0:
                        return Color.Orchid;
                    default:
                        return Color.White;
                }

            }
        }

    }
}
