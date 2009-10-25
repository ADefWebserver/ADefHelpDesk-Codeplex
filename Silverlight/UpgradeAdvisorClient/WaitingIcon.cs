// file: waitingIcon.cs
// author:cokkiy2001@hotmail.com
// You can distribute this file under Ms-PL license.
//
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace UpgradeAdvisorClient
{
    /// <summary>
    /// Represents a Vista busy cursor like control, can be used as a budy indicator.
    /// </summary>
    [TemplateVisualState(Name = WaitingIcon.BusyStateName, GroupName = WaitingIcon.BusyIdleStatesGroupName)]
    [TemplateVisualState(Name = WaitingIcon.IdleStateName, GroupName = WaitingIcon.BusyIdleStatesGroupName)]
    public class WaitingIcon : Control
    {
        #region VisualStateName
        /// <summary>
        /// Busy Idle State group name
        /// </summary>
        private const string BusyIdleStatesGroupName = "BusyIdleStates";
        /// <summary>
        /// Busy State name
        /// </summary>
        private const string BusyStateName = "BusyState";
        /// <summary>
        /// Idle state name
        /// </summary>
        private const string IdleStateName = "IdleState"; 
        #endregion


        #region IsBusy Property
        /// <summary>
        /// Gets or sets a value indicating is busy or not
        /// </summary>
        /// <value>A value indicating whether the control is in busy state or not.
        /// <para>The default value is <c>false</c>.</para></value>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Identifies the <see cref="IsBusy"/> dependency property. 
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.Register("IsBusy", typeof(bool), typeof(WaitingIcon),
            new PropertyMetadata(false, IsBusyPropertyChanged));

        /// <summary>
        /// The <see cref="IsBusy"/> property changed callback function.
        /// </summary>
        /// <param name="d">The <see cref="WaitingIcon"/> control whose <see cref="IsBusy"/> property changed.</param>
        /// <param name="e">The DependencyPropertyChangedEventArgs contains old and new value.</param>
        private static void IsBusyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WaitingIcon wi = d as WaitingIcon;
            wi.IsBusyChanged((bool)e.OldValue, (bool)e.NewValue);
        } 
        #endregion


        #region StrokeThickness Property
        /// <summary>
        /// Gets or sets the width of the <see cref="WaitingIcon"/> stroke outline. 
        /// </summary>
        /// <value>The width of the <see cref="WaitingIcon"/>  outline, in pixels. 
        /// The default value is 0. </value>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StrokeThickness.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> dependency property. 
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(WaitingIcon),
            new PropertyMetadata(6.0)); 
        #endregion



        /// <summary>
        /// Initialize a new instance of <see cref="WaitingIcon"/> class.
        /// </summary>
        public WaitingIcon()
        {
            // The default style key
            this.DefaultStyleKey = typeof(WaitingIcon);
        }

        /// <summary>
        /// Apply new template
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.IsBusy)
            {
                // if set to busy in beginner, we must goto BusyState here
                VisualStateManager.GoToState(this, WaitingIcon.BusyStateName, false);
            }
        }

        /// <summary>
        /// The <see cref="IsBusy "/> property changed.
        /// </summary>
        /// <param name="oldValue">The old value of the <see cref="IsBusy"/> property.</param>
        /// <param name="newValue">The new  value of the <see cref="IsBusy"/> property.</param>
        protected virtual void IsBusyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                VisualStateManager.GoToState(this, WaitingIcon.BusyStateName, false);
            }
            else
            {
                VisualStateManager.GoToState(this, WaitingIcon.IdleStateName, false);
            }
        }
    }
}
