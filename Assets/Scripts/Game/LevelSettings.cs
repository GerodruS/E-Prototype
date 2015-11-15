using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    [System.Serializable]
    public struct Start
    {
        public enum Position : int
        {
            Left = 0,
            Right = 1,
        }

        /// <summary>
        /// Стартовая скорость 
        /// </summary>
        [Tooltip("Стартовая скорость")]
        [Range(0.0f, 10.0f)]
        public float speed;

        /// <summary>
        /// Позиция игрока в "колодце" 
        /// </summary>
        [Tooltip("Позиция игрока в 'колодце'")]
        public Position position;
    }

    [System.Serializable]
    public struct Block
    {
        /// <summary>
        /// Количество препятствий в блоке 
        /// </summary>
        [Tooltip("Количество препятствий в блоке")]
        public int quantity;

        /// <summary>
        /// Сложность блока (0 - n): от сложности зависит расположение, частота, позиционирование и
        /// вид препятствий
        /// </summary>
        [Tooltip("Сложность блока (0 - n): от сложности зависит расположение, частота, позиционирование и вид препятствий")]
        public int difficultyLevel;

        /// <summary>
        /// Cкорость игрока при входе в зону блока 
        /// </summary>
        [Tooltip("Cкорость игрока при входе в зону блока")]
        public float startSpeed;

        /// <summary>
        /// Cкорость игрока при выходе из зону блока 
        /// </summary>
        [Tooltip("Cкорость игрока при выходе из зону блока")]
        public float endSpeed;

        /// <summary>
        /// Задержка между перескакиванием 
        /// </summary>
        [Tooltip("Задержка между перескакиванием")]
        public float switchDelay;
    }

    [System.Serializable]
    public struct DifficultyLevel
    {
        [System.Serializable]
        public struct SizeProbability
        {
            public float antennaType;
            public float ledgeType;
            public float flagpoleType;
            public float balconyType;
            public float flagpoleWithRopeType;

            public BarrierType getBarrierSize(double randomValue)
            {
                double sum = antennaType +
                             ledgeType +
                             flagpoleType +
                             balconyType +
                             flagpoleWithRopeType;
                double value = 0.0;
                value += antennaType / sum;
                if (randomValue < value)
                {
                    return BarrierType.Antenna;
                }
                value += ledgeType / sum;
                if (randomValue < value)
                {
                    return BarrierType.Ledge;
                }
                value += flagpoleType / sum;
                if (randomValue < value)
                {
                    return BarrierType.Flagpole;
                }
                value += balconyType / sum;
                if (randomValue < value)
                {
                    return BarrierType.Balcony;
                }
                return BarrierType.FlagpoleWithRope;
            }
        }

        /// <summary>
        /// Изначальная вероятность появления того или иного блока. После появления нового пересчитывается 
        /// </summary>
        [Tooltip("Изначальная вероятность появления того или иного блока. После появления нового пересчитывается")]
        public SizeProbability sizeProbability;

        /// <summary>
        /// Вероятность появления препятствий на одной стороне: чем ближе к единице, тем чаще
        /// чередуются стороны
        /// </summary>
        [Tooltip("Вероятность появления препятствий на одной стороне: чем ближе к единице, тем чаще чередуются стороны")]
        public float sideProbability;

        /// <summary>
        /// Коэффициент расстояния между блоками: чем выше сложность, тем меньше коэффициент 
        /// </summary>
        [Tooltip("Коэффициент расстояния между блоками: чем выше сложность, тем меньше коэффициент")]
        public float gapMultiplier;
    }

    public enum BarrierType : int
    {
        None,
        Antenna,
        Ledge,
        Flagpole,
        Balcony,
        FlagpoleWithRope,
    }

    [Space(10.0f)]
    public Start start = new Start()
    {
        speed = 1.0f,
        position = Start.Position.Left
    };

    /// <summary>
    /// Массив из блоков, в которых содержатся препятствия количество элементов может быть динамическим 
    /// </summary>
    [Tooltip("Массив из блоков, в которых содержатся препятствия количество элементов может быть динамическим")]
    [Space(10.0f)]
    public Block[] blocks;

    /// <summary>
    /// Уровни сложности 
    /// </summary>
    [Tooltip("Уровни сложности")]
    [Space(10.0f)]
    public DifficultyLevel[] difficultyLevels;
}