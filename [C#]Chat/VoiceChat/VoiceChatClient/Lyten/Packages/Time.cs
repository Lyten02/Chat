namespace Lyten.Packages
{
    public class Time
    {
        private int _days;
        public int Days
        {
            get => _days;
            set
            {
                if (value < 0)
                    _days = 0;
                else
                    _days = value;
            }
        }

        private int _hours;
        public int Hours
        {
            get => _hours;
            set
            {
                if (value < 0)
                    _hours = 0;
                else if (value > 23)
                {
                    Days += value / 24;
                    _hours = value % 24;
                }
                else
                    _hours = value;
            }
        }

        private int _minutes;
        public int Minutes
        {
            get => _minutes;
            set
            {
                if (value < 0)
                    _minutes = 0;
                else if (value > 59)
                {
                    Hours += value / 60;
                    _minutes = value % 60;
                }
                else
                    _minutes = value;
            }
        }

        private int _seconds;
        public int Seconds
        {
            get => _seconds;
            set
            {
                if (value < 0)
                    _seconds = 0;
                else if (value > 59)
                {
                    Minutes += value / 60;
                    _seconds = value % 60;
                }
                else
                    _seconds = value;
            }
        }

        public Time(int days = 0, int hours = 0, int minutes = 0, int seconds = 0)
        {
            _days = days;
            _hours = hours % 24;
            _minutes = minutes % 60;
            _seconds = seconds % 60;

            Days += hours / 24;
            Days += minutes / 60;
            Minutes += seconds / 60;
        }

        public void SetTime(int days, int hours, int minutes, int seconds)
        {
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }

        public string GetTimeThree()
        {
            return $"{Hours:D2}:{Minutes:D2}:{Seconds:D2}";
        }

        public string GetTimeAll()
        {
            return $"{Days:D2}:{Hours:D2}:{Minutes:D2}:{Seconds:D2}";
        }

        public void AddTime(int days, int hours, int minutes, int seconds)
        {
            Days += days;
            Hours += hours;
            Minutes += minutes;
            Seconds += seconds;
        }
    }
}
