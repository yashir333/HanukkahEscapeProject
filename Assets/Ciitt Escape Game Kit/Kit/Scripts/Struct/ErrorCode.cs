using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ciitt.EscapeGameKit
{

    public enum ErrorCode
    {

        Success = 0,
        InvalidFilePath,
        EmptyData,
        FailedEncryptTextData,
        FailedDecryptTextData,
        FailedPlayerPrefs,

        Unknown

    }

}
