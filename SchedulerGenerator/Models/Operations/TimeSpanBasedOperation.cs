﻿namespace SchedulerGenerator.Models.Operations
{
    public abstract record TimeSpanBasedOperation
    {
        public TimeSpan Start;
        public TimeSpan End;        
    }

}