

namespace ClipBoardHistory
{
    public class ClipBoardData
    {
        public int Id { get; set; }
        public string? CreateDate { get; set; }
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
                        temp =  tempArray[1] + Environment.NewLine + tempArray[2] + Environment.NewLine 
                            + tempArray[3] + Environment.NewLine + tempArray[4] + Environment.NewLine
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

        private string? ClearDoubleNewLines(string s)
        {
            if (s.Length - s.Replace(Environment.NewLine + Environment.NewLine,"").Length>1)
            {
                ClearDoubleNewLines(s.Replace(Environment.NewLine + Environment.NewLine, ""));
            }
            return s;
        }


    }
}
