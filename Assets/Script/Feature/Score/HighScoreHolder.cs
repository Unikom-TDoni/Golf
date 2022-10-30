using System;

namespace Edu.Golf.Score
{
    [Serializable]
    public sealed class HighScoreHolder
    {
        public uint[] HighScore = new uint[5];
    }
}