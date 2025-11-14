using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Labb3_QuizApp.Converters
{
    public class AnswerToStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine($"Converter called with value: {value} of type {value?.GetType().Name}");
            if ((AnswerState)value is AnswerState state)
            {
                return GetBrushForState(state);
            }

            if (value is string stateStr)
            {
                if (Enum.TryParse<AnswerState>(stateStr, out var parsedState))
                {
                    return GetBrushForState(parsedState);
                }

                return stateStr switch
                {
                    "Correct" => new SolidColorBrush(Colors.Green),
                    "Incorrect" => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.LightGray)
                };
            }

            return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        private SolidColorBrush GetBrushForState(AnswerState state)
        {
            return state switch
            {
                AnswerState.Correct => new SolidColorBrush(Colors.Green),
                AnswerState.Incorrect => new SolidColorBrush(Colors.Red),
                _ => new SolidColorBrush(Colors.LightGray)
            };
        }
    }
}

public enum AnswerState
{
    Neutral,
    Correct,
    Incorrect
}