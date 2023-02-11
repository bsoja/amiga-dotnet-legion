namespace AmigaNet.Amos
{
    public class Data
    {
        private readonly List<string> data = new List<string>();
        private int pos;

        public Data(string name, string content)
        {
            Name = name;
            pos = 0;
            LoadData(content);
        }

        public string Name { get; private set; }

        public int Read()
        {
            return int.Parse(data[pos++]);
        }

        public string ReadS()
        {
            return data[pos++];
        }

        private void LoadData(string content)
        {
            var lines = content.Split('\r', '\n');
            foreach (var l in lines)
            {
                var line = l.Trim();
                if (string.IsNullOrEmpty(line)) continue;
                if (line.StartsWith('\'')) continue;
                if (line.StartsWith("Data"))
                {
                    line = line.Substring(4).Trim();
                    var parts = line.Split(',');
                    bool textStarted = false;
                    foreach (var p in parts)
                    {
                        var part = p;

                        if (textStarted)
                        {
                            if (part.EndsWith('"'))
                            {
                                textStarted = false;
                                part = part.Substring(0, part.Length - 2);
                            }
                            else
                            {
                                part += ",";
                            }
                            data[data.Count - 1] = data[data.Count - 1] + part;
                            continue;
                        }

                        if (part.StartsWith('"'))
                        {
                            if (part.EndsWith('"'))
                            {
                                part = part.Substring(1, part.Length - 2);
                            }
                            else
                            {
                                part = part.Substring(1);
                                textStarted = true;
                            }
                        }
                        else
                        {
                            var plusParts = part.Split('+');
                            if (plusParts.Length > 1)
                            {
                                part = plusParts.Select(p => int.Parse(p)).Sum().ToString();
                            }
                        }
                        data.Add(part);
                    }
                }
            }
        }
    }
}
