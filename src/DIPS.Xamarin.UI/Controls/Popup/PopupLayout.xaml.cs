﻿using System;
using DIPS.Xamarin.UI.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Controls.Popup
{
    /// <summary>
    /// Layout used to add content showing popups
    /// </summary>
    [ContentProperty(nameof(MainContent))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupLayout : ContentView
    {
        private TapGestureRecognizer m_closePopupRecognizer;
        private View? m_content;
        private Lazy<Frame> m_blockingFrame;

        /// <summary>
        /// Create an instance
        /// </summary>
        public PopupLayout()
        {
            InitializeComponent();
            m_closePopupRecognizer = new TapGestureRecognizer { Command = new Command(HidePopup) };
            m_blockingFrame = new Lazy<Frame>(CreateBlockingFrame);
        }

        /// <summary>
        /// <see cref="MainContent" />
        /// </summary>
        public static readonly BindableProperty MainContentProperty =
            BindableProperty.Create(nameof(MainContent), typeof(View), typeof(PopupLayout), propertyChanged: OnMainContentPropertyChanged);

        /// <summary>
        /// Main Content of the layout. This is routed from the Content property, so you don't have to use it.
        /// </summary>
        public View MainContent
        {
            get { return (View)GetValue(MainContentProperty); }
            set { SetValue(MainContentProperty, value); }
        }

        private static void OnMainContentPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (!(bindable is PopupLayout popupLayout)) return;
            if (!(newvalue is View newView)) return;
            if (newvalue == popupLayout.relativeLayout)
            {
                popupLayout.Content = popupLayout.relativeLayout;
            }
            else
            {
                popupLayout.relativeLayout.Children.Clear();
                popupLayout.relativeLayout.Children.Add(newView, Constraint.RelativeToParent(parent => parent.X), Constraint.RelativeToParent(parent => parent.Y), Constraint.RelativeToParent(parent => parent.Width), Constraint.RelativeToParent(parent => parent.Height));
            }
        }

        internal void ShowPopup(View popupView, View relativeView, PopupDirection popupDirection)
        {
            relativeLayout.Children.Add(m_blockingFrame.Value,
                widthConstraint: Constraint.RelativeToParent(r => r.Width),
                heightConstraint: Constraint.RelativeToParent(r => r.Height),
                xConstraint: Constraint.RelativeToParent(r => 0.0),
                yConstraint: Constraint.RelativeToParent(r => 0.0));

            var direction = popupDirection;
            if (direction == PopupDirection.Auto)
            {
                var height = Height;
                var center = height / 2.0;
                var itemPosition = relativeView.GetY(this) + relativeView.Height / 2.0;
                if (itemPosition > center) direction = PopupDirection.Above;
                else direction = PopupDirection.Below;
            }

            relativeLayout.Children.Add(m_content = popupView,
                yConstraint: Constraint.RelativeToParent((r) => relativeView.GetY(this) + relativeView.Height));
            var sumMarginY = popupView.Margin.Top + popupView.Margin.Bottom;
            var diffY = direction == PopupDirection.Below ? relativeView.Height : (-popupView.Height - sumMarginY);

            RelativeLayout.SetYConstraint(popupView, Constraint.RelativeToParent((r) => Math.Max(0, Math.Min(r.Height - popupView.Height - sumMarginY, relativeView.GetY(this) + diffY))));
            RelativeLayout.SetXConstraint(popupView, Constraint.RelativeToParent((r) => Math.Max(0, Math.Min(r.Width - popupView.Width - popupView.Margin.Left - popupView.Margin.Right, relativeView.GetX(this)))));
        }

        internal void HidePopup()
        {
            relativeLayout.Children.Remove(m_blockingFrame.Value);
            if (m_content != null)
            {
                relativeLayout.Children.Remove(m_content);
            }
            m_content = null;
        }

        internal void AddOnCloseRecognizer(View view)
        {
            if (view is Button button)
            {
                button.Clicked -= OnClicked;
                button.Clicked += OnClicked;
            }
            else
            {
                view.GestureRecognizers.Remove(m_closePopupRecognizer);
                view.GestureRecognizers.Add(m_closePopupRecognizer);
            }
        }

        private void OnClicked(object sender, EventArgs args)
        {
            HidePopup();
        }

        private Frame CreateBlockingFrame()
        {
            var background = new Frame
            {
                BackgroundColor = Color.Gray,
                IsVisible = true,
                Opacity = 0.5
            };

            background.GestureRecognizers.Add(m_closePopupRecognizer);
            return background;
        }
    }
}