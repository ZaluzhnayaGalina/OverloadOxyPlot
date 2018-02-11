using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace OverloadOxyPlot
{
    internal class TextBoxInteraction : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                base.OnAttached();
                AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
        }

        private void AssociatedObject_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox is null)
                return;
            if (e.Key == Key.Return || e.Key == Key.Enter || e.Key == Key.Escape)
                textBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
