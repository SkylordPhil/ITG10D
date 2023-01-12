using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helper
{
    public class MathP : MonoBehaviour
    {
        /// <summary>
        /// Converts a dezibel Value from -80 to 0 into a linear Scale from 0 to 1
        /// </summary>
        /// <param name="dB"> 
        /// Volume [-80f - 0f] <br/> <br/>
        /// 0 is full Volume <br/>
        /// -80 is no Volume
        /// </param>
        public static float ConvertDBToLin(float dB)
        {
            float linear = Mathf.Pow(10.0f, dB / 20.0f);
            return linear;

        }


        /// <summary>
        /// Converts a value from 0 to 1 into dezibel scale between -80 to 0
        /// </summary>
        /// <param name="ln"> 
        /// Noise Level  [0f - 1f]  <br/> <br/>
        /// 1 is full Volume <br/>
        /// 0 is no Volume
        /// </param>
        public static float ConvertLnToDB(float ln)
        {
            float dB = 0;
            if (ln > 0)
                dB = 20.0f * Mathf.Log10(ln);
            else
                dB = -80.0f;
            return dB;
        }





    }
}
