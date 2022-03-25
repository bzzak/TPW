using System.Text;

namespace String_Example
{
    public class MyString
    {
        string text;

        public MyString(string text)
        {
            this.text = text;
        }

        public string GetText()
        {
            return text;
        }

        public string SetText(string text)
        {
            return this.text = text;
        }

        public string ConcatText(params string[] texts)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                text += texts[i];
            }
            return text;
        }

        public string ToLower()
        {
            StringBuilder sb = new StringBuilder(text);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 64 && text[i] < 91)
                {
                    sb[i] = (char)(sb[i] + 32);
                }
            }
            text = sb.ToString();
            return text;
        }

        public string ToUpper()
        {
            StringBuilder sb = new StringBuilder(text);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] > 96 && text[i] < 123)
                {
                    sb[i] = (char)(sb[i] - 32);
                }
            }
            text = sb.ToString();
            return text;
        }
    }
}
