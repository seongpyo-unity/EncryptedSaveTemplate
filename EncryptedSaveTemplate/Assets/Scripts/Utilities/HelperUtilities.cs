using System.Collections;
using UnityEngine;

/// <summary>
/// 유틸리티 함수들을 제공하는 클래스
/// </summary>
public class HelperUtilities : MonoBehaviour
{
    /// <summary>
    /// 문자열이 비어 있는지 검사하고 경고 로그를 출력하는 함수
    /// </summary>
    /// <param name="thisObject">검사 대상 오브젝트</param>
    /// <param name="fieldName">검사할 필드 이름</param>
    /// <param name="stringToCheck">검사할 문자열</param>
    /// <returns>비어 있으면 true</returns>
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log("is empty and must contain a value in object" + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// Object 타입 필드가 null인지 검사하고 경고 로그를 출력하는 함수
    /// </summary>
    /// <param name="thisObject">검사 대상 오브젝트</param>
    /// <param name="fieldName">검사할 필드 이름</param>
    /// <param name="objectTocheck">검사할 UnityEngine.Object</param>
    /// <returns>null이면 true</returns>
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, Object objectTocheck)
    {
        if (objectTocheck == null)
        {
            Debug.Log(fieldName + "is null and must contain a value in object" + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// 컬렉션이 null이거나 비어 있거나, null 항목이 포함되어 있는지 검사하는 함수
    /// </summary>
    /// <param name="thisObject">검사 대상 오브젝트</param>
    /// <param name="fieldName">검사할 필드 이름</param>
    /// <param name="enumerableObjectToCheck">검사할 IEnumerable 객체</param>
    /// <returns>에러가 있으면 true</returns>
    public static bool ValidateCheckEnumerableValues(Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fieldName + "is null in object " + thisObject.name.ToString());
            return true;
        }

        foreach (var item in enumerableObjectToCheck)
        {
            if (item == null)
            {
                Debug.Log(fieldName + "has null values in object" + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fieldName + "has no values in object" + thisObject.name.ToString());
            error = true;
        }

        return error;
    }

    /// <summary>
    /// float 값이 0 이상 또는 양수인지 검사하는 함수
    /// </summary>
    /// <param name="thisObject">검사 대상 오브젝트</param>
    /// <param name="fieldName">검사할 필드 이름</param>
    /// <param name="valueToCheck">검사할 float 값</param>
    /// <param name="isZeroAllowed">0 허용 여부</param>
    /// <returns>유효하지 않으면 true</returns>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value or zero in object" + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object" + thisObject.name.ToString());
                error = true;
            }
        }
        return error;
    }

    /// <summary>
    /// int 값이 0 이상 또는 양수인지 검사하는 함수
    /// </summary>
    /// <param name="thisObject">검사 대상 오브젝트</param>
    /// <param name="fieldName">검사할 필드 이름</param>
    /// <param name="valueToCheck">검사할 int 값</param>
    /// <param name="isZeroAllowed">0 허용 여부</param>
    /// <returns>유효하지 않으면 true</returns>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " must contain a positive value or zero in object " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " must contain a positive value in object" + thisObject.name.ToString());
                error = true;
            }
        }

        return error;
    }

    /// <summary>
    /// float 최소/최대 값의 범위가 올바른지 및 유효한 양수인지 검사하는 함수
    /// </summary>
    /// <param name="thisObject">검사 대상 오브젝트</param>
    /// <param name="fieldNameMinimum">최소값 필드 이름</param>
    /// <param name="valueToCheckMinimum">검사할 최소 float 값</param>
    /// <param name="fieldNameMaximum">최대값 필드 이름</param>
    /// <param name="valueToCheckMaximum">검사할 최대 float 값</param>
    /// <param name="isZeroAllowed">0 허용 여부</param>
    /// <returns>에러가 있으면 true</returns>
    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, float valueToCheckMinimum, string fieldNameMaximum,
        float valueToCheckMaximum, bool isZeroAllowed)
    {
        bool error = false;
        if (valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log(fieldNameMinimum + " must be less than or equal to " + fieldNameMaximum + " in object " + thisObject.name.ToString());
            error = true;

        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed)) error = true;

        if (ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed)) error = true;

        return error;
    }

    /// <summary>
    /// int 최소/최대 값의 범위가 올바른지 및 유효한 양수인지 검사하는 함수
    /// </summary>
    /// <param name="thisObject">검사 대상 오브젝트</param>
    /// <param name="fieldNameMinimum">최소값 필드 이름</param>
    /// <param name="valueToCheckMinimum">검사할 최소 int 값</param>
    /// <param name="fieldNameMaximum">최대값 필드 이름</param>
    /// <param name="valueToCheckMaximum">검사할 최대 int 값</param>
    /// <param name="isZeroAllowed">0 허용 여부</param>
    /// <returns>에러가 있으면 true</returns>
    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, int valueToCheckMinimum, string fieldNameMaximum,
       int valueToCheckMaximum, bool isZeroAllowed)
    {
        bool error = false;
        if (valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log(fieldNameMinimum + " must be less than or equal to " + fieldNameMaximum + " in object " + thisObject.name.ToString());
            error = true;

        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed)) error = true;

        if (ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed)) error = true;

        return error;
    }

    /// <summary>
    /// 0~20 선형 볼륨 값을 dB(데시벨)로 변환하는 함수
    /// </summary>
    /// <param name="linear">선형 볼륨 (0~20)</param>
    /// <returns>변환된 데시벨 값</returns>
    public static float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f;

        // formula to convert from the linear scale to the logarithmic decibel scale
        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }
}
