using System.Diagnostics;

namespace AmigaNet.Amos
{
    public class AmosBase
    {
        private readonly String resourcesPath;
        private readonly IGameEngine gameEngine;
        private readonly Random Random = new Random();
        private readonly List<Data> dataItems = new List<Data>();

        public AmosBase(String resourcesPath, IGameEngine gameEngine)
        {
            this.resourcesPath = resourcesPath;
            this.gameEngine = gameEngine;
        }

        /// <summary>
        /// WAIT
        /// instruction: wait before performing the next instruction
        /// Wait number of 50ths of a second
        /// </summary>
        public void Wait(int time)
        {
            var sek = time / 50f;
            Thread.Sleep((int)(sek * 1000));
        }

        /// <summary>
        /// STR$
        /// function: convert a number into a string
        /// s$=Str$(number)
        /// </summary>
        /// <returns></returns>
        public String Str_S(int number)
        {
            var nrStr = number.ToString();
            if (number > 0) return " " + nrStr;
            return nrStr;
        }

        /// <summary>
        /// ADD
        /// instruction: perform fast integer addition
        /// Add variable, expression
        /// Add variable, expression,base To top
        /// </summary>
        public void Add(ref int variable, int expression, int basee, int top)
        {
            variable = variable + expression;
            if (variable < basee) variable = top;
            if (variable > top) variable = basee;
        }

        /// <summary>
        /// UPPER$
        /// function: convert a string of text to upper case
        /// new$=Upper$(old$)
        /// </summary>
        public String Upper_S(String s)
        {
            return s.ToUpper();
        }

        /// <summary>
        /// LEFT$
        /// function: return the leftmost characters of a string
        /// destination$=Left$(source$,number)
        /// Left$(destination$,number)=source$
        /// </summary>
        public String Left_S(String s, int number)
        {
            if (number > s.Length) number = s.Length;
            return number >=0 ? s.Substring(0, number) : s;
        }

        /// <summary>
        /// RIGHT$
        /// function: return the rightmost characters of a string
        /// destination$=Right$(source$,number)
        /// Right$(destination$,number)=source$
        /// </summary>
        public String Right_S(String s, int number)
        {
            return number >= 0 && number <= s.Length ? s.Substring(s.Length - number, number) : s;
        }

        /// <summary>
        /// MID$
        /// function: return a number of characters from the middle of a string
        /// destination$=Mid$(source$,offset,number)
        /// Mid$(destination$,offset,number)=source$
        /// </summary>
        public String Mid_S(String s, int offset, int number)
        {
            var startIdx = offset - 1;
            if (startIdx + number > s.Length)
            {
                number = s.Length - startIdx;
            }
            return s.Substring(startIdx, number);
        }

        /// <summary>
        /// INSTR
        /// function: search for occurrences of one string within another string
        /// x = Instr(host$, guest$)
        /// x=Instr(host$, guest$, start of search position)
        /// </summary>
        public int Instr(String host, String guest)
        {
            return host.IndexOf(guest);
        }

        /// <summary>
        /// VAL
        /// function: convert a string of digits into a number
        /// v=Val(x$)
        /// v#=Val(x$)
        /// </summary>
        public int Val(String s)
        {
            return int.Parse(s);
        }

        /// <summary>
        /// SGN
        /// function: return the sign of a number
        /// sign = Sgn(value)
        /// sign=Sgn(value#)
        /// </summary>
        public int Sgn(int value)
        {
            if (value < 0) return -1;
            if (value > 0) return 1;
            return 0;
        }

        /// <summary>
        /// The TIMER reserved variable is incremented by 1 unit every 50th of a second, in other words, it returns the amount 
        /// of time that has elapsed since your Amiga was last switched on.
        /// </summary>
        public long TIMER
        {
            get
            {
                var s = Stopwatch.GetTimestamp() / 10000 / 20;
                return s;
            }
        }

        /// <summary>
        /// The RND function generates integers at random, between zero and any number specified in brackets. If your 
        /// specified number is greater than zero, random numbers will be generated up to that maximum number. However, if
        /// you specify 0, then RND will return the last random value it generated.
        /// </summary>
        /// <returns></returns>
        public int Rnd(int number)
        {
            // NOTE: AMOS Rnd(x) === C# Random(x+1)
            return Random.Next(number + 1);
        }

        /// <summary>
        /// function: check if specified file exists
        /// value=Exist("filename")
        /// EXIST looks through the current directory of filenames and checks it against the filename in your given string. If the names match, then the file does exist and a value of -1 (true) will be reported, otherwise 0 (false) will be returned.
        /// </summary>
        /// <returns></returns>
        public bool Exist(String fileName)
        {
            return File.Exists(fileName);
        }

                
        /// <summary>
        /// TRACK LOOP ON
        /// TRACK LOOP OFF
        /// instructions: toggle a Tracker loop
        /// </summary>
        public void TrackLoopOn()
        {
            gameEngine.IsTrackLoop = true;
        }

        /// <summary>
        /// TRACK PLAY
        /// instruction: play a Tracker module
        /// Track Play
        /// Track Play bank number, pattern number
        /// </summary>
        public void TrackPlay(int nr=0)
        {
            //NOTE: nr would be ignored as we always load tracks and play them from bank nr 3
            gameEngine.PlayTrack();
        }

        /// <summary>
        /// TRACK STOP
        /// instruction: stop all Tracker music
        /// Track Stop
        /// </summary>
        public void TrackStop()
        {
            gameEngine.StopTrack();
        }


        /// <summary>
        /// structure: read data into a String variable
        /// </summary>
        /// <returns></returns>
        public String ReadS()
        {
            var resName = GetCallerName();
            var data = GetData(resName);
            return data.ReadS();
        }

        /// <summary>
        /// structure: read data into a Int variable
        /// </summary>
        /// <returns></returns>
        public int Read()
        {
            var resName = GetCallerName();
            var data = GetData(resName);
            return data.Read();
        }

        private Data GetData(String resName)
        {
            var data = dataItems.FirstOrDefault(d => String.Equals(d.Name, resName));
            if (data == null)
            {
                data = new Data(resName, File.ReadAllText(Path.Combine(resourcesPath, resName)));
                dataItems.Add(data);
            }
            return data;
        }

        private String GetCallerName()
        {
            return new StackTrace().GetFrame(2).GetMethod().Name;
        }
    }
    
}
