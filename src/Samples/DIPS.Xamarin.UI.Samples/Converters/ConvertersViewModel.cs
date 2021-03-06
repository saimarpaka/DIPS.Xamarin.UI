﻿using System.Windows.Input;
using DIPS.Xamarin.UI.Samples.Converters.ValueConverters;
using Xamarin.Forms;

namespace DIPS.Xamarin.UI.Samples.Converters
{
    public class ConvertersViewModel
    {
        private readonly INavigation m_navigation;

        public ConvertersViewModel(INavigation navigation)
        {
            m_navigation = navigation;
            NavigateToCommand = new Command<string>(NavigateTo);
        }

        public ICommand NavigateToCommand { get; }

        private void NavigateTo(string parameter)
        {
            switch (parameter)
            {
                case "BoolToObject":
                    m_navigation.PushAsync(new BoolToObjectConverterPage(){Title = parameter});
                    break;
                case "InvertedBool":
                    m_navigation.PushAsync(new InvertedBoolConverterPage() { Title = parameter });
                    break;
                case "IsEmptyConverter":
                    m_navigation.PushAsync(new IsEmptyConverterPage() { Title = parameter });
                    break;
                case "IsEmptyToObjectConverter":
                    m_navigation.PushAsync(new IsEmptyToObjectConverterPage() { Title = parameter });
                    break;
                case "DateConverter":
                    m_navigation.PushAsync(new DateConverterPage() { Title = parameter });
                    break;
                case "TimeConverter":
                    m_navigation.PushAsync(new TimeConverterPage() { Title = parameter });
                    break;
                case "DateAndTimeConverter":
                    m_navigation.PushAsync(new DateAndTimeConverterPage() { Title = parameter });
                    break;
                case "MultiplicationConverter":
                    m_navigation.PushAsync(new MultiplicationConverterPage() { Title = parameter });
                    break;
                case "AdditionConverter":
                    m_navigation.PushAsync(new AdditionConverterPage() { Title = parameter });
                    break;
            }
        }
    }
}