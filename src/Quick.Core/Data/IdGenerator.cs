using System;
using System.Threading;

namespace Quick
{
    public class IdGenerator
    {
        public IdGenerator(int machineId) : this(machineId, 10)
        {

        }
        public IdGenerator() : this(-1)
        {
        }
        public IdGenerator(int machineId, int machineBits)
        {
            if (machineId == -1)
            {
                Random rand = new Random(Environment.TickCount);
                machineId = rand.Next();
            }
            _machineBits = machineBits;
            int machineMask = int.MaxValue;
            machineMask = machineMask >> (31 - _machineBits);
            _machineId = machineId & machineMask;
            _startTime = DateTimeOffset.Parse("2021-01-01").ToUnixTimeMilliseconds();
            _seqBits = 22 - _machineBits;

            _seqMask = long.MaxValue;
            _seqMask = _seqMask >> (63 - _seqBits);
        }
        private static long _machineId;
        private static int _machineBits;
        private static long _startTime;

        private static int _seqBits;
        private static long _curSeq = 0;
        private static long _seqMask = 0;
        private static IdGenerator _default = new IdGenerator();
        private static long _lastDuration = 0;
        private static long _correction = 0;  //用于存储回拨后的修正值
        public long Next()
        {
            long curTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            long duration = curTime - _startTime + Interlocked.Read(ref _correction);
            long lastDuration = Interlocked.Read(ref _lastDuration);
            long diff = lastDuration - duration; //判断时间回拨
            //说明时钟回拨了
            if (diff > 0)
            {
                //给予修正值
                long correction = diff + 1000;
                Interlocked.Exchange(ref _correction, correction); //存储修正值
                duration += correction;
            }
            Interlocked.Exchange(ref _lastDuration, duration);//保存duration
            duration = duration << 22;
            long machineId = _machineId;
            machineId = machineId << _seqBits;
            long seq = Interlocked.Increment(ref _curSeq);
            seq = seq & _seqMask;
            return duration | machineId | seq;
        }
        public static long New()
        {
            return _default.Next();
        }
    }
}
