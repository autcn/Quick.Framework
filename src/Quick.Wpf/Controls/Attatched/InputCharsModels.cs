namespace Quick
{
    public static class InputCharsModels
    {
        public const InputChars IpAddress = InputChars.Number | InputChars.Dot | InputChars.MultiSymbols;
        public const InputChars NegativeInteger = InputChars.Number | InputChars.Negative;
        public const InputChars Time = InputChars.Number | InputChars.Colon;
        public const InputChars Float = InputChars.Number | InputChars.Dot;
        public const InputChars NegativeFloat = InputChars.Number | InputChars.Dot | InputChars.Negative;
    }
}
