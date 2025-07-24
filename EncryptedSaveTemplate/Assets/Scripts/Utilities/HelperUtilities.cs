using System.Collections;
using UnityEngine;

/// <summary>
/// ��ƿ��Ƽ �Լ����� �����ϴ� Ŭ����
/// </summary>
public class HelperUtilities : MonoBehaviour
{
    /// <summary>
    /// ���ڿ��� ��� �ִ��� �˻��ϰ� ��� �α׸� ����ϴ� �Լ�
    /// </summary>
    /// <param name="thisObject">�˻� ��� ������Ʈ</param>
    /// <param name="fieldName">�˻��� �ʵ� �̸�</param>
    /// <param name="stringToCheck">�˻��� ���ڿ�</param>
    /// <returns>��� ������ true</returns>
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
    /// Object Ÿ�� �ʵ尡 null���� �˻��ϰ� ��� �α׸� ����ϴ� �Լ�
    /// </summary>
    /// <param name="thisObject">�˻� ��� ������Ʈ</param>
    /// <param name="fieldName">�˻��� �ʵ� �̸�</param>
    /// <param name="objectTocheck">�˻��� UnityEngine.Object</param>
    /// <returns>null�̸� true</returns>
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
    /// �÷����� null�̰ų� ��� �ְų�, null �׸��� ���ԵǾ� �ִ��� �˻��ϴ� �Լ�
    /// </summary>
    /// <param name="thisObject">�˻� ��� ������Ʈ</param>
    /// <param name="fieldName">�˻��� �ʵ� �̸�</param>
    /// <param name="enumerableObjectToCheck">�˻��� IEnumerable ��ü</param>
    /// <returns>������ ������ true</returns>
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
    /// float ���� 0 �̻� �Ǵ� ������� �˻��ϴ� �Լ�
    /// </summary>
    /// <param name="thisObject">�˻� ��� ������Ʈ</param>
    /// <param name="fieldName">�˻��� �ʵ� �̸�</param>
    /// <param name="valueToCheck">�˻��� float ��</param>
    /// <param name="isZeroAllowed">0 ��� ����</param>
    /// <returns>��ȿ���� ������ true</returns>
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
    /// int ���� 0 �̻� �Ǵ� ������� �˻��ϴ� �Լ�
    /// </summary>
    /// <param name="thisObject">�˻� ��� ������Ʈ</param>
    /// <param name="fieldName">�˻��� �ʵ� �̸�</param>
    /// <param name="valueToCheck">�˻��� int ��</param>
    /// <param name="isZeroAllowed">0 ��� ����</param>
    /// <returns>��ȿ���� ������ true</returns>
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
    /// float �ּ�/�ִ� ���� ������ �ùٸ��� �� ��ȿ�� ������� �˻��ϴ� �Լ�
    /// </summary>
    /// <param name="thisObject">�˻� ��� ������Ʈ</param>
    /// <param name="fieldNameMinimum">�ּҰ� �ʵ� �̸�</param>
    /// <param name="valueToCheckMinimum">�˻��� �ּ� float ��</param>
    /// <param name="fieldNameMaximum">�ִ밪 �ʵ� �̸�</param>
    /// <param name="valueToCheckMaximum">�˻��� �ִ� float ��</param>
    /// <param name="isZeroAllowed">0 ��� ����</param>
    /// <returns>������ ������ true</returns>
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
    /// int �ּ�/�ִ� ���� ������ �ùٸ��� �� ��ȿ�� ������� �˻��ϴ� �Լ�
    /// </summary>
    /// <param name="thisObject">�˻� ��� ������Ʈ</param>
    /// <param name="fieldNameMinimum">�ּҰ� �ʵ� �̸�</param>
    /// <param name="valueToCheckMinimum">�˻��� �ּ� int ��</param>
    /// <param name="fieldNameMaximum">�ִ밪 �ʵ� �̸�</param>
    /// <param name="valueToCheckMaximum">�˻��� �ִ� int ��</param>
    /// <param name="isZeroAllowed">0 ��� ����</param>
    /// <returns>������ ������ true</returns>
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
    /// 0~20 ���� ���� ���� dB(���ú�)�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="linear">���� ���� (0~20)</param>
    /// <returns>��ȯ�� ���ú� ��</returns>
    public static float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f;

        // formula to convert from the linear scale to the logarithmic decibel scale
        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }
}
