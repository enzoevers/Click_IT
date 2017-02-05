using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Click_IT
{
    public class WindowControle
    {
        // Fields

        private bool maximized;
        private WindowState state;
        private Window window;
        


        // Property

        /// <summary>
        /// Determines if the current WindowState is maximized or not.
        /// </summary>
        public bool Maximized
        {
            set
            {
                this.maximized = value;
            }
        }



        // Constructor

        /// <summary>
        /// Makes a new object of the WindowControle class of the current window.
        /// </summary>
        /// <param name="WindowState"></param>
        /// <param name="Window"></param>
        public WindowControle(WindowState WindowState, Window Window)
        {
            state = WindowState;
            window = Window;
        }



        // Methods

        /// <summary>
        /// Exit the application.
        /// </summary>
        public void Exit()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Minimize the application.
        /// </summary>
        public void minimize()
        {
            state = WindowState.Minimized;
        }

        /// <summary>
        /// Maximizes the application.
        /// </summary>
        public void maximize()
        {
            if (!maximized)
            {
                state = WindowState.Maximized;
                Maximized = true;
            }
            else if (maximized)
            {
                state = WindowState.Normal;
                Maximized = false;
            }
        }

        /// <summary>
        /// Enables the application to be dragged.
        /// </summary>
        /// <param name="e"></param>
        public void dragWindow(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                window.DragMove();
            }
        }

    }
}
