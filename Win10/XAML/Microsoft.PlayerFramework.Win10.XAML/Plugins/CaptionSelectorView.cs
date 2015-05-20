﻿using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.Resources;

namespace Microsoft.PlayerFramework
{
    /// <summary>
    /// A control that allows the user to select which captions they want or to turn off closed captioning.
    /// </summary>
    public class CaptionSelectorView : Control
    {
        /// <summary>
        /// Creates a new instance of the control
        /// </summary>
        public CaptionSelectorView()
        {
            this.DefaultStyleKey = typeof(CaptionSelectorView);
        }

        /// <summary>
        /// Indicates the caption selector view should be closed.
        /// </summary>
        public event EventHandler Close;

        /// <summary>
        /// Indicates the selected caption changed.
        /// </summary>
        public event EventHandler SelectedCaptionChanged;

        DeselectableListBox CaptionsList;
        Panel LayoutRoot;

		/// <inheritdoc /> 
		protected override void OnApplyTemplate()
		{
            base.OnApplyTemplate();

            CaptionsList = GetTemplateChild("CaptionsList") as DeselectableListBox;
            LayoutRoot = GetTemplateChild("LayoutRoot") as Panel;

            CaptionsList.ItemsSource = AvailableCaptions;
            CaptionsList.SelectedItem = SelectedCaption;

            CaptionsList.SelectionChanged += ListBox_SelectionChanged;
			LayoutRoot.PointerPressed += LayoutRoot_PointerPressed;
		}
		void LayoutRoot_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
            if (Close != null) Close(this, EventArgs.Empty);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedCaption = CaptionsList.SelectedItem as Caption;
            if (SelectedCaptionChanged != null) SelectedCaptionChanged(this, EventArgs.Empty);
            if (Close != null) Close(this, EventArgs.Empty);
        }

        /// <summary>
        /// AvailableCaptions DependencyProperty definition.
        /// </summary>
        public static readonly DependencyProperty AvailableCaptionsProperty = DependencyProperty.Register("AvailableCaptions", typeof(IEnumerable<Caption>), typeof(CaptionSelectorView), new PropertyMetadata(null, (d, e) => ((CaptionSelectorView)d).OnAvailableCaptionsChanged(e.NewValue as IEnumerable<Caption>)));

        /// <summary>
        /// Gets or sets the collection of captions available.
        /// </summary>
        public IEnumerable<Caption> AvailableCaptions
        {
            get { return GetValue(AvailableCaptionsProperty) as IEnumerable<Caption>; }
            set { SetValue(AvailableCaptionsProperty, value); }
        }

        void OnAvailableCaptionsChanged(IEnumerable<Caption> availableCaptions)
        {
            if (CaptionsList != null) CaptionsList.ItemsSource = availableCaptions;
        }

        /// <summary>
        /// SelectedCaption DependencyProperty definition.
        /// </summary>
        public static readonly DependencyProperty SelectedCaptionProperty = DependencyProperty.Register("SelectedCaption", typeof(Caption), typeof(CaptionSelectorView), new PropertyMetadata(null, (d, e) => ((CaptionSelectorView)d).OnSelectedCaptionChanged(e.NewValue as Caption)));

        /// <summary>
        /// Gets or sets the selected caption.
        /// </summary>
        public Caption SelectedCaption
        {
            get { return GetValue(SelectedCaptionProperty) as Caption; }
            set { SetValue(SelectedCaptionProperty, value); }
        }

        void OnSelectedCaptionChanged(Caption caption)
        {
            if (CaptionsList != null) CaptionsList.SelectedItem = caption;
        }
    }
}
