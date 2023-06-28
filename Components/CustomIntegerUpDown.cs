using Xceed.Wpf.Toolkit;

namespace LOGrasper.Components
{
    public class CustomIntegerUpDown : IntegerUpDown
    {
        protected override int DecrementValue(int value, int increment)
        {
            if (value - increment < 1)
            {
                return value;
            }
            return value - increment;
        }
    }
}
