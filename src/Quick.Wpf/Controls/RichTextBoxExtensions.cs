using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Quick
{
    public static class RichTextBoxExtensions
    {
        private static void InnerWrite(RichTextBox richTextBox, string text, Brush brush, int maxLine, bool scrollToEnd)
        {
            if (richTextBox.Document.Blocks.Count > maxLine)
            {
                richTextBox.Document.Blocks.Clear();
            }

            if (richTextBox.Document.Blocks.Count >= maxLine)
            {
                richTextBox.Document.Blocks.Clear();
            }

            Paragraph paragraph = new Paragraph();
            paragraph.LineHeight = 5;
            paragraph.Foreground = brush;
            Run run = new Run(text);
            paragraph.Inlines.Add(run);

            richTextBox.Document.Blocks.Add(paragraph);
            richTextBox.ScrollToEnd();

        }
        public static void Write(this RichTextBox richTextBox, string text, Brush brush, int maxLine)
        {
            richTextBox.Write(text, brush, maxLine, true);
        }
        public static void Write(this RichTextBox richTextBox, string text, Brush brush)
        {
            richTextBox.Write(text, brush, int.MaxValue, true);
        }
        public static void Write(this RichTextBox richTextBox, string text, Brush brush, int maxLine, bool scrollToEnd)
        {
            if (richTextBox.Dispatcher.Thread.ManagedThreadId != Dispatcher.CurrentDispatcher.Thread.ManagedThreadId)
            {
                richTextBox.Dispatcher.BeginInvoke(new Action(() =>
                 {
                     InnerWrite(richTextBox, text, brush, maxLine, scrollToEnd);
                 }));
            }
            else
            {
                InnerWrite(richTextBox, text, brush, maxLine, scrollToEnd);
            }
        }
    }
}
